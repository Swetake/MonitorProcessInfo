using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;	
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Threading;
using System.Diagnostics;
using MonitorProcessInfo;
using System.Drawing.Text;
using MonitorProcessInfo.Properties;

namespace MonitorProcessInfo
{
    /// <summary>
    /// MonitorProcess
    /// </summary>
    public class MonitorProcess
    {
        private const int ORDER_MEM_FILE_SIZE = 256;
        private const int PROC_INFO_MEM_FILE_SIZE = 256;
        private const int ORDER_CHECK_INTERVAL = 250;
        private const string PROC_INFO_MEM_FILE_TIMEFORMAT = "yyyyMMddHHmmss";
        private const int RETRY_MAX_NOEXISTS_MEM_FILE = 4;
        private const int CPU_TOTAL = -1;
        private const int NO_PROCESS= 0;
        private const string NO_PROCESS_TIMESTRNIG = "00000000000000";

        private const int EACH_CPU_MEM_FILE_COUNT = 4;

        private const int MAX_COUNT_OF_PROCESSES = 32;
        /// <summary>
        /// Max cpu count
        /// </summary>
        public const int MAX_CPU_COUNT = 32;

        private int interval;
        private int number;
        private bool logging;
        private string logDirectory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_interval"></param>
        /// <param name="_number"></param>
        /// <param name="_logging"></param>
        /// <param name="_logDirectory"></param>
        public MonitorProcess(int _interval, int _number, bool _logging, string _logDirectory)
        {
            interval = _interval;
            number = _number;
            logging = _logging;
            logDirectory = _logDirectory;

            var task = InitializeAsync();
        }

        /// <summary>
        /// InitilaizeAsync
        /// </summary>
        /// <returns></returns>
        private async Task InitializeAsync()
        {

            MemoryMappedFile orderMmf = null;
            List<Process> listPs = new List<Process>();

            Dictionary<int, Dictionary<string, Queue<ulong>>> multiCalcuDict = new Dictionary<int, Dictionary<string, Queue<ulong>>>();

            Dictionary<int, ulong> continuousNonResponding = new Dictionary<int, ulong>();

            Dictionary<int, MemoryMappedFile> mmfDict = new Dictionary<int, MemoryMappedFile>();

            try
            {
                //Create order mmf
                orderMmf = MemoryMappedFile.CreateNew(Resources.OrderMemFileName, ORDER_MEM_FILE_SIZE);
            }
            catch
            {
                throw new Exception(Resources.ExceptionMsgOrderMemFileAlreadyExist);
            }

            //Initialzie
            using (MemoryMappedViewAccessor accessor = orderMmf.CreateViewAccessor())
            {
                byte[] arrayZero = new byte[ORDER_MEM_FILE_SIZE];
                accessor.WriteArray<byte>(0, arrayZero, 0, ORDER_MEM_FILE_SIZE);
            }

            var task = Task.Run(() => MainLoopAsync(orderMmf)).ConfigureAwait(false);
        }


        /// <summary>
        /// MainLoopAsync
        /// </summary>
        /// <returns></returns>
        private async Task MainLoopAsync(MemoryMappedFile orderMmf) {

            Microsoft.VisualBasic.Devices.ComputerInfo systemInfo;

            List<Process> listPs = new List<Process>();

            Dictionary<int, Dictionary<string, Queue<ulong>>> multiCalcuDict = new Dictionary<int, Dictionary<string, Queue<ulong>>>();
            Dictionary<int, ulong> movingAverageWsSum = new Dictionary<int, ulong>();
            Dictionary<int, ulong> movingAveragePmSum = new Dictionary<int, ulong>();
            Dictionary<int, ulong> continuousNonResponding = new Dictionary<int, ulong>();

            Dictionary<int, MemoryMappedFile> mmfDict = new Dictionary<int, MemoryMappedFile>();


            System.Diagnostics.PerformanceCounter pcCpuTotal = new System.Diagnostics.PerformanceCounter(Resources.PerformanceCounterCategoryName, Resources.PerformanceCounterCounterName, Resources.PerformaceCounterInstanceName, Resources.PerfomanceCounterMachineName);

            System.Diagnostics.PerformanceCounter[] pcCpuCounterArray;


            Queue<ulong> cpuQueue = new Queue<ulong>();
            movingAverageWsSum[CPU_TOTAL] = 0;

            // Main loop preparation

            List<int> processIdList;
            List<string> startTimeList;
            List<int> orderList;

            bool shouldExit = false;
            int counter = 0;
            int v = interval * 1000 / ORDER_CHECK_INTERVAL;
            int cpuCount = Environment.ProcessorCount;
            if (cpuCount >= MAX_CPU_COUNT)
            {
                pcCpuCounterArray = new PerformanceCounter[MAX_CPU_COUNT];
            }else
            {
                pcCpuCounterArray = new PerformanceCounter[cpuCount];
            }
            int indedCpuCounter = 0;
            while (indedCpuCounter < pcCpuCounterArray.Length)
            {
                pcCpuCounterArray[indedCpuCounter]= new System.Diagnostics.PerformanceCounter(Resources.PerformanceCounterCategoryName, Resources.PerformanceCounterCounterName, indedCpuCounter.ToString(), Resources.PerfomanceCounterMachineName);
                indedCpuCounter++;
            }

            string fileNameSysLog = System.IO.Path.Combine(logDirectory, "MPI_SYS_"+System.DateTime.Now.ToString("yyyyMMdd-HHmmss")+".log");
            string fileNameProcLog = System.IO.Path.Combine(logDirectory, "MPI_PROC_" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".log");
            StringBuilder sbSysLog = new StringBuilder();
            StringBuilder sbProcLog = new StringBuilder();

            //Logging
            using (var writerSystemLog = new StreamWriter(fileNameSysLog))
            {
                using (var writerProcessLog = new StreamWriter(fileNameProcLog))
                {
                    if (logging)
                    {
                        await writerSystemLog.WriteLineAsync(Resources.LogHeaderSystem);
                        await writerProcessLog.WriteLineAsync(Resources.LogHeaderProcess);


                    }
                    //Main loop
                    while (!shouldExit)
                    {
                        counter++;

                        //Check Order
                        byte[] arraydata = new byte[ORDER_MEM_FILE_SIZE];
                        byte[] arrayzero = new byte[ORDER_MEM_FILE_SIZE];
                        using (MemoryMappedViewAccessor accessor = orderMmf.CreateViewAccessor())
                        {
                            accessor.ReadArray<byte>(0, arraydata, 0, ORDER_MEM_FILE_SIZE);
                            accessor.WriteArray<byte>(0, arrayzero, 0, ORDER_MEM_FILE_SIZE);
                        }

                        // Check Order
                        processIdList = new List<int>();
                        startTimeList = new List<string>();
                        orderList = new List<int>();
                        ExtractOrderInfoFromByteArray(arraydata, ref orderList, ref processIdList, ref startTimeList);

                        int index = 0;
                        while (index < orderList.Count())
                        {
                            int order = orderList[index];
                            if (order == 0) { break; }
                            switch (order)
                            {
                                case (int)Order.Add:
                                    //Create mmf for target process
                                    int pid;
                                    string pStartTime;
                                    Process p;

                                    if (processIdList.Count() > MAX_COUNT_OF_PROCESSES)
                                    {
                                        throw new Exception(Resources.ExceptionMsgExceededProcesses);
                                    }

                                    if (processIdList[index] == 0)
                                    {
                                        pid = NO_PROCESS;
                                        pStartTime = NO_PROCESS_TIMESTRNIG;
                                        p = null;
                                    }
                                    else
                                    {
                                        p = System.Diagnostics.Process.GetProcessById(processIdList[index]);
                                        pid = p.Id;
                                        pStartTime = p.StartTime.ToString(PROC_INFO_MEM_FILE_TIMEFORMAT);
                                    }


                                    if (pStartTime == startTimeList[index])
                                    {
                                        var memFileName = Resources.PrefixMemFileName + pid.ToString() + "_" + startTimeList[index];
                                        mmfDict[processIdList[index]] = MemoryMappedFile.CreateNew(memFileName, PROC_INFO_MEM_FILE_SIZE);
                                        multiCalcuDict.Add(pid, new Dictionary<string, Queue<ulong>>());
                                        multiCalcuDict[pid][MonitorItem.ProcessWorkingSet.ToString()] = new Queue<ulong>();
                                        multiCalcuDict[pid][MonitorItem.ProcessTotalProcessorTime.ToString()] = new Queue<ulong>();
                                        multiCalcuDict[pid][MonitorItem.ProcessPrivateMemorySize.ToString()] = new Queue<ulong>();
                                        movingAverageWsSum[pid] = 0;
                                        movingAveragePmSum[pid] = 0;
                                        continuousNonResponding[pid] = 0;
                                        listPs.Add(p);
                                    }
                                    else
                                    {
                                        throw (new Exception(Resources.ExceptionMsgSameIdButNotSameStarttime));
                                    }
                                    break;

                                case (int)Order.Terminate:
                                    shouldExit = true;
                                    break;
                            }
                            index++;
                        }

                        if (counter >= v)
                        {
                            counter = 0;
                            List<Process> nonexistsProcesses = new List<Process>();
                            // Start to Get Info

                            //Checking System CPU Total
                            ulong systemCpu = (ulong)(pcCpuTotal.NextValue() * 100);


                            //Checking System CPU MA
                            ulong systemCpuMA = 0;

                            cpuQueue.Enqueue(systemCpu);
                            movingAverageWsSum[CPU_TOTAL] += systemCpu;
                            if (cpuQueue.Count > number)
                            {
                                movingAverageWsSum[CPU_TOTAL] -= cpuQueue.Dequeue();
                                systemCpuMA = (ulong)movingAverageWsSum[CPU_TOTAL] / (ulong)number;
                            }

                            //Each CPU
                            ulong[] eachCpu = new ulong[EACH_CPU_MEM_FILE_COUNT];

                            int cpuIndex = 0;
                            int fileIndex = 0;
                            while (cpuIndex < pcCpuCounterArray.Length)
                            {
                                float tempCpu = pcCpuCounterArray[cpuIndex].NextValue();
                                int shift = (cpuIndex % 4) << 3;
                                eachCpu[fileIndex] = eachCpu[fileIndex] ^ ((((ulong)(tempCpu)) & 0xff) << shift);
                                cpuIndex++;
                                if (cpuIndex % 4 == 0)
                                {
                                    fileIndex++;
                                }
                            }

                            systemInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();

                            //Checking System Total Physical Memory
                            ulong systemTotalPhysicalMem = systemInfo.TotalPhysicalMemory;


                            //Checkinh System Available Phisycal Memory
                            ulong systemAvailableMem = systemInfo.AvailablePhysicalMemory;


                            //Checking System Total Virtual Memory
                            ulong systemTotalVirtualMem = systemInfo.TotalVirtualMemory;


                            //Checkinh System Avilable Virtual Memory
                            ulong systemAvailableVirtualMem = systemInfo.AvailableVirtualMemory;

                            if (logging)
                            {
                                //System value
                                sbSysLog = new StringBuilder(512);
                                sbSysLog.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.sss,"));

                                sbSysLog.Append(systemCpu.ToString());
                                sbSysLog.Append(',');

                                sbSysLog.Append(systemCpuMA.ToString());
                                sbSysLog.Append(',');

                                foreach (ulong cpuv in eachCpu)
                                {
                                    sbSysLog.Append(cpuv.ToString());
                                    sbSysLog.Append(',');
                                }

                                sbSysLog.Append(systemTotalPhysicalMem.ToString());
                                sbSysLog.Append(',');

                                sbSysLog.Append(systemAvailableMem.ToString());
                                sbSysLog.Append(',');

                                sbSysLog.Append(systemTotalVirtualMem.ToString());
                                sbSysLog.Append(',');

                                sbSysLog.Append(systemAvailableVirtualMem.ToString());

                                await writerSystemLog.WriteLineAsync(sbSysLog.ToString());
                            }

                            //for each processes
                            foreach (Process p in listPs)
                            {

                                var writeValueList = new List<ulong>();

                                //Checking System CPU
                                writeValueList.Add(systemCpu);  // #0

                                //Checking System CPU MA
                                writeValueList.Add(systemCpuMA);  // #1

                                //
                                writeValueList.Add(eachCpu[0]);   //#2

                                writeValueList.Add(eachCpu[1]);   //#3

                                writeValueList.Add(eachCpu[2]);   //#4

                                writeValueList.Add(eachCpu[3]);   //#5

                                //Checking System Total Physical Memory
                                writeValueList.Add(systemTotalPhysicalMem);  // #6

                                //Checkinh System Available Phisycal Memory
                                writeValueList.Add(systemAvailableMem);  // #7

                                //Checking System Total Virtual Memory
                                writeValueList.Add(systemTotalVirtualMem);  // #8

                                //Checkinh System Avilable Virtual Memory
                                writeValueList.Add(systemTotalVirtualMem);  // #9


                                int pid;
                                if (p != null)
                                {
                                    p.Refresh();
                                    pid = p.Id;

                                    try
                                    {
                                        if (p.HasExited)
                                        {
                                            nonexistsProcesses.Add(p);
                                            continue;
                                        }
                                    }
                                    catch
                                    {
                                        nonexistsProcesses.Add(p);
                                        continue;
                                    }

                                    if (!multiCalcuDict.ContainsKey(pid))
                                    {
                                        throw (new KeyNotFoundException(Resources.ExceptionMsgKeyNotFoundInDict + pid.ToString()));
                                    }




                                    // Get WorkingSet
                                    var workingset = (ulong)p.WorkingSet64;
                                    writeValueList.Add(workingset);  // #10



                                    // Calculate Moving Average of Workingset
                                    ulong workingsetMa = 0;
                                    multiCalcuDict[pid][MonitorItem.ProcessWorkingSet.ToString()].Enqueue(workingset);
                                    movingAverageWsSum[pid] += workingset;
                                    if (multiCalcuDict[pid][MonitorItem.ProcessWorkingSet.ToString()].Count > number)
                                    {
                                        movingAverageWsSum[pid] -= multiCalcuDict[pid][MonitorItem.ProcessWorkingSet.ToString()].Dequeue();
                                        workingsetMa = (ulong)movingAverageWsSum[pid] / (ulong)number;
                                    }
                                    writeValueList.Add(workingsetMa);  // #11


                                    // Get PrivateMemorySize
                                    var privatememory = (ulong)p.PrivateMemorySize64;
                                    writeValueList.Add(privatememory);  // #12



                                    //Get PrivateMemorySizeMA
                                    ulong privateMemoryMa = 0;
                                    multiCalcuDict[pid][MonitorItem.ProcessPrivateMemorySize.ToString()].Enqueue(privatememory);
                                    movingAveragePmSum[pid] += privatememory;
                                    if (multiCalcuDict[pid][MonitorItem.ProcessPrivateMemorySize.ToString()].Count > number)
                                    {
                                        movingAveragePmSum[pid] -= multiCalcuDict[pid][MonitorItem.ProcessPrivateMemorySize.ToString()].Dequeue();
                                        privateMemoryMa = (ulong)movingAveragePmSum[pid] / (ulong)number;
                                    }
                                    writeValueList.Add(privateMemoryMa);  // #13





                                    // TotalManagedMemoryFromGC
                                    var totalMangedMemory = (ulong)GC.GetTotalMemory(false);
                                    writeValueList.Add(totalMangedMemory);  // #14



                                    // Get Cputime
                                    var cpuTotalTime = (ulong)(p.TotalProcessorTime.TotalMilliseconds);
                                    writeValueList.Add(cpuTotalTime);  // #15


                                    // Calculate Delta CpuTime
                                    ulong cpuTotalTimeDelta = 0;
                                    multiCalcuDict[pid][MonitorItem.ProcessTotalProcessorTime.ToString()].Enqueue(cpuTotalTime);
                                    if (multiCalcuDict[pid][MonitorItem.ProcessTotalProcessorTime.ToString()].Count > number)
                                    {
                                        cpuTotalTimeDelta = cpuTotalTime - multiCalcuDict[pid][MonitorItem.ProcessTotalProcessorTime.ToString()].Dequeue();
                                    }
                                    writeValueList.Add(cpuTotalTimeDelta);  // #16

                                    //Check Responding
                                    var responding = p.Responding;
                                    writeValueList.Add((ulong)(responding ? 1 : 0)); // #17

                                    if (!responding)
                                    {
                                        continuousNonResponding[pid]++;
                                    }
                                    else
                                    {
                                        continuousNonResponding[pid] = 0;
                                    }
                                    writeValueList.Add(continuousNonResponding[pid]);  //#18


                                    //Logging
                                    if (logging)
                                    {
                                        sbProcLog = new StringBuilder(1024);
                                        sbProcLog.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.sss,"));
                                        sbProcLog.Append(pid.ToString());
                                        sbProcLog.Append(',');

                                        sbProcLog.Append(workingset.ToString());
                                        sbProcLog.Append(',');

                                        sbProcLog.Append(workingsetMa.ToString());
                                        sbProcLog.Append(',');

                                        sbProcLog.Append(privatememory.ToString());
                                        sbProcLog.Append(',');

                                        sbProcLog.Append(privateMemoryMa.ToString());
                                        sbProcLog.Append(',');

                                        sbProcLog.Append(totalMangedMemory.ToString());
                                        sbProcLog.Append(',');

                                        sbProcLog.Append(cpuTotalTime.ToString());
                                        sbProcLog.Append(',');

                                        sbProcLog.Append(cpuTotalTimeDelta.ToString());
                                        sbProcLog.Append(',');

                                        sbProcLog.Append(responding.ToString());
                                        sbProcLog.Append(',');

                                        sbProcLog.Append(continuousNonResponding[pid].ToString());


                                        await writerProcessLog.WriteLineAsync(sbProcLog.ToString());


                                    }

                                }
                                else
                                {
                                    pid = 0;
                                }

                                // Write to mmf of target pid
                                using (MemoryMappedViewAccessor accessor = mmfDict[pid].CreateViewAccessor())
                                {
                                    accessor.WriteArray<ulong>(0, writeValueList.ToArray(), 0, writeValueList.Count());
                                }
                            }
                            foreach (Process p in nonexistsProcesses)
                            {
                                if (p != null)
                                {
                                    mmfDict[p.Id].Dispose();
                                    multiCalcuDict.Remove(p.Id);
                                    listPs.Remove(p);
                                }
                            }
                        }

                        Thread.Sleep(ORDER_CHECK_INTERVAL);
                    }
                }
            }

            //Termination
            foreach(MemoryMappedFile mmf in mmfDict.Values)
            {
                mmf.Dispose();
            }
            orderMmf.Dispose();

        }
        /// <summary>
        /// TerminateMonitoring
        /// </summary>
        public static void TerminateMonitoring()
        {
            //Openorder mmf
            using (MemoryMappedFile orderMmf = MemoryMappedFile.OpenExisting(Resources.OrderMemFileName))
            {
                using (MemoryMappedViewAccessor accessor = orderMmf.CreateViewAccessor())
                {
                    byte[] arraydata = new byte[ORDER_MEM_FILE_SIZE];
                    arraydata[0] = (int)MonitorProcessInfo.Order.Terminate;
                    accessor.WriteArray<byte>(0, arraydata, 0, ORDER_MEM_FILE_SIZE);
                }
            }
        }

        /// <summary>
        /// SetOrder
        /// </summary>
        /// <param name="p"></param>
        public static void SetOrder(Process p)
        {
            //Openorder mmf
            using (MemoryMappedFile orderMmf = MemoryMappedFile.OpenExisting(Resources.OrderMemFileName))
            {
                using (MemoryMappedViewAccessor accessor = orderMmf.CreateViewAccessor())
                {
                    byte[] arraydata = new byte[ORDER_MEM_FILE_SIZE];
                    accessor.ReadArray<byte>(0, arraydata, 0, ORDER_MEM_FILE_SIZE);

                    List<int> processIdList = new List<int>();
                    List<string> startTimeList = new List<string>();
                    List<int> orderList = new List<int>();
                    ExtractOrderInfoFromByteArray(arraydata, ref orderList, ref processIdList, ref startTimeList);

                    List<int> newOrderList = new List<int>();
                    List<int> newProcessIdList = new List<int>();
                    List<string> newStartTimeList = new List<string>();


                    int index = 0;
                    while (index < orderList.Count())
                    {
                        if (orderList[index] == 0) { break; }
                        newOrderList.Add(orderList[index]);
                        newProcessIdList.Add(processIdList[index]);
                        newStartTimeList.Add(startTimeList[index]);
                        index++;
                    }

                    if (index < 8)
                    {
                        if (p != null)
                        {
                            newOrderList.Add((int)MonitorProcessInfo.Order.Add);
                            newProcessIdList.Add(p.Id);
                            newStartTimeList.Add(p.StartTime.ToString(PROC_INFO_MEM_FILE_TIMEFORMAT));
                        }
                        else
                        {
                            newOrderList.Add((int)MonitorProcessInfo.Order.Add);
                            newProcessIdList.Add(NO_PROCESS);
                            newStartTimeList.Add(NO_PROCESS_TIMESTRNIG);
                        }
                    }
                    arraydata = CreateByteArrayForOrder(newOrderList, newProcessIdList, newStartTimeList);

                    accessor.WriteArray<byte>(0, arraydata, 0, ORDER_MEM_FILE_SIZE);
                }
            }
        }

        /// <summary>
        /// GetProcessInfo
        /// </summary>
        public static Dictionary<MonitorItem, ulong> GetMonitoredValue(Process p)
        {
            
            var ret = new Dictionary<MonitorItem, ulong>();
            if (p==null || !p.HasExited)
            {
                string memFileName;
                if (p == null)
                {
                    memFileName = Resources.PrefixMemFileName + NO_PROCESS.ToString() + "_" + NO_PROCESS_TIMESTRNIG;
                }
                else
                {
                    memFileName = Resources.PrefixMemFileName + p.Id.ToString() + "_" + p.StartTime.ToString(PROC_INFO_MEM_FILE_TIMEFORMAT);
                }

                int retryCounter = 0;
                bool isSuccess = false;
                do
                {
                    try
                    {
                        using (var mmf = MemoryMappedFile.OpenExisting(memFileName))
                        {
                            using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
                            {
                                ulong[] resultArray = new ulong[PROC_INFO_MEM_FILE_SIZE>>3];
                                accessor.ReadArray<ulong>(0, resultArray, 0, resultArray.Length);

                                foreach (MonitorItem mi in Enum.GetValues(typeof(MonitorItem)))
                                {
                                    ret[mi] = resultArray[(int)mi];
                                }
                            }
                        }
                        isSuccess = true;
                    }
                    catch
                    {
                        if (retryCounter > RETRY_MAX_NOEXISTS_MEM_FILE) { throw; }
                        retryCounter++;
                        Thread.Sleep(ORDER_CHECK_INTERVAL*2);
                    }
                } while (!isSuccess);
            }
            return ret;
        }

        private static void ExtractOrderInfoFromByteArray(byte[] arraydata,ref List<int> orderList, ref List<int> processIdList, ref List<string> startTimeList )
        {
            int index = 0;
            while (index < 8)
            {
                int shift = index << 5;

                //Extract order
                int order = arraydata[shift] + (arraydata[shift + 1] << 8) + (arraydata[shift + 2] << 16) + (arraydata[shift + 3] << 24);
                orderList.Add(order);

                //Extract Process ID
                processIdList.Add((int)(arraydata[shift + 4] + (arraydata[shift + 5] << 8) + (arraydata[shift + 6] << 16) + (arraydata[shift + 7] << 24)));

                //Extract Start Time
                byte[] stringArray = new byte[14];
                Array.Copy(arraydata, shift + 8, stringArray, 0, 14);
                startTimeList.Add(System.Text.Encoding.ASCII.GetString(stringArray));

                if (order == 0) { break; }
                index++;
            }
        }

        private static byte[] CreateByteArrayForOrder(List<int> orderList, List<int> processIdList, List<string> startTimeList)
        {
            byte[] ret = new byte[ORDER_MEM_FILE_SIZE];
            int index = 0;
            while (index < orderList.Count())
            {
                int shift = index << 5;

                ret[shift] = (byte)(orderList[index] & 0xFF);
                ret[shift + 1]= (byte)((orderList[index]>>8) & 0xFF);
                ret[shift + 2] = (byte)((orderList[index] >> 16) & 0xFF);
                ret[shift + 3] = (byte)((orderList[index] >> 24) & 0xFF);

                ret[shift + 4]= (byte)(processIdList[index] & 0xFF);
                ret[shift + 5] = (byte)((processIdList[index]>>8) & 0xFF);
                ret[shift + 6] = (byte)((processIdList[index]>>16) & 0xFF);
                ret[shift + 7] = (byte)((processIdList[index]>>24) & 0xFF);
                if (startTimeList[index].Length == PROC_INFO_MEM_FILE_TIMEFORMAT.Length)
                {
                    for (int j = 0; j < PROC_INFO_MEM_FILE_TIMEFORMAT.Length; j++)
                    {
                        ret[shift + 8 + j] = Convert.ToByte(startTimeList[index][j]);
                    }
                }
                else
                {
                    throw (new ArgumentException(Resources.ExceptionMsgInvalidTimeFormat));
                }
                index++;
            }
            return ret;
        }

    }
}
