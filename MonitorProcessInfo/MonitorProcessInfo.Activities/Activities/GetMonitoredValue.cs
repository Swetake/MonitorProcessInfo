using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using MonitorProcessInfo.Activities.Properties;
using MonitorProcessInfo;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace MonitorProcessInfo.Activities
{
    /// <summary>
    /// GetProcessInfo
    /// </summary>
    [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_DisplayName))]
    [LocalizedDescription(nameof(Resources.GetMonitoredValue_Description))]
    public class GetMonitoredValue : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }
        
        /// <summary>
        /// Process
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_Process_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_Process_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<Process> Process { get; set; }


        //Output category

        /// <summary>
        /// Raw Value
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_RawValue_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_RawValue_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<Dictionary<MonitorItem, ulong>> RawValue { get; set; }

        //System category

        /// <summary>
        /// SystemTotalCpu
        /// </summary>                
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_SystemTotalCpu_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_SystemTotalCpu_Description))]
        [LocalizedCategory(nameof(Resources.System_Category))]
        public OutArgument<float> SystemTotalCpu { get; set; }

        /// <summary>
        /// SystemTotalCpuMA
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_SystemTotalCpuMA_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_SystemTotalCpuMA_Description))]
        [LocalizedCategory(nameof(Resources.System_Category))]
        public OutArgument<float> SystemTotalCpuMA { get; set; }

        /// <summary>
        /// SystemEachCpu
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_SystemEachCpu_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_SystemEachCpu_Description))]
        [LocalizedCategory(nameof(Resources.System_Category))]
        public OutArgument<float[]> SystemEachCpu { get; set; }

        /// <summary>
        /// SystemTotalPhysicalmemory
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_SystemTotalPhysicalMemory_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_SystemTotalPhysicalMemory_Description))]
        [LocalizedCategory(nameof(Resources.System_Category))]
        public OutArgument<ulong> SystemTotalPhysicalMemory { get; set; }

        /// <summary>
        /// SystemAvailablePhisycalMemory
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_SystemAvailablePhysicalMemory_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_SystemAvailablePhysicalMemory_Description))]
        [LocalizedCategory(nameof(Resources.System_Category))]
        public OutArgument<ulong> SystemAvailablePhisycalMemory { get; set; }

        /// <summary>
        /// SystemTotalVirtualMemory
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_SystemTotalVirtualMemory_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_SystemTotalVirtualMemory_Description))]
        [LocalizedCategory(nameof(Resources.System_Category))]
        public OutArgument<ulong> SystemTotalVirtualMemory { get; set; }

        /// <summary>
        /// SystemAvailableVirtualMemory
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_SystemAvailableVirtualMemory_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_SystemAvailableVirtualMemory_Description))]
        [LocalizedCategory(nameof(Resources.System_Category))]
        public OutArgument<ulong> SystemAvilableVirtualMemory { get; set; }

        //Process

        /// <summary>
        /// ProcessWorkingSet
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_ProcessWorkingSet_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_ProcessWorkingSet_Description))]
        [LocalizedCategory(nameof(Resources.Process_Category))]
        public OutArgument<ulong> ProcessWorkingSet { get; set; }

        /// <summary>
        /// ProcessWorkingSetMA
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_ProcessWorkingSetMA_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_ProcessWorkingSetMA_Description))]
        [LocalizedCategory(nameof(Resources.Process_Category))]
        public OutArgument<ulong> ProcessWorkingSetMA { get; set; }


        /// <summary>
        /// ProcessPrivateMemorySize
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_ProcessPrivateMemorySize_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_ProcessPrivateMemorySize_Description))]
        [LocalizedCategory(nameof(Resources.Process_Category))]
        public OutArgument<ulong> ProcessPrivateMemorySize { get; set; }

        /// <summary>
        /// ProcessPrivateMemorySizeMA
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_ProcessPrivateMemorySizeMA_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_ProcessPrivateMemorySizeMA_Description))]
        [LocalizedCategory(nameof(Resources.Process_Category))]
        public OutArgument<ulong> ProcessPrivateMemorySizeMA { get; set; }



        /// <summary>
        /// TotalManagedMemory
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_ProcessTotalManagedMemory_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_ProcessTotalManagedMemory_Description))]
        [LocalizedCategory(nameof(Resources.Process_Category))]
        public OutArgument<ulong> ProcessTotalManagedMemory { get; set; }

        /// <summary>
        /// ProcessTotalProcessorTime
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_ProcessTotalProcessorTime_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_ProcessTotalProcessorTime_Description))]
        [LocalizedCategory(nameof(Resources.Process_Category))]
        public OutArgument<ulong> ProcessTotalProcessorTime { get; set; }

        /// <summary>
        /// ProsessTotalProcessorTimeDelta
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_ProcessTotalProcessorTimeDelta_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_ProcessTotalProcessorTimeDelta_Description))]
        [LocalizedCategory(nameof(Resources.Process_Category))]
        public OutArgument<ulong> ProcessTotalProcessorTimeDelta { get; set; }

        /// <summary>
        /// ProcessResponding
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_ProcessResponding_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_ProcessResponding_Description))]
        [LocalizedCategory(nameof(Resources.Process_Category))]
        public OutArgument<bool> ProcessResponding { get; set; }

        /// <summary>
        /// ProcessRespondingCount
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.GetMonitoredValue_ProcessRespondingCount_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetMonitoredValue_ProcessRespondingCount_Description))]
        [LocalizedCategory(nameof(Resources.Process_Category))]
        public OutArgument<int> ProcessRespondingCount { get; set; }

        #endregion


        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public GetMonitoredValue()
        {
        }

        #endregion


        #region Protected Methods

        /// <summary>
        /// CacheMetaData
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Process == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Process)));

            base.CacheMetadata(metadata);
        }

        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var process = Process.Get(context);

            var ret = MonitorProcess.GetMonitoredValue(process);

            ulong[] eachCpu = { ret[MonitorItem.SystemEachCpu00to07], ret[MonitorItem.SystemEachCpu08to15], ret[MonitorItem.SystemEachCpu16to23], ret[MonitorItem.SystemEachCpu24to31] };

            int cpuCount = Environment.ProcessorCount;
            if (cpuCount > MonitorProcess.MAX_CPU_COUNT)
            {
                cpuCount = MonitorProcess.MAX_CPU_COUNT;
            }

            float[] eachCpuArray = new float[cpuCount];
            for(int i=0; i < cpuCount; i++)
            {
                ulong cpuValue = eachCpu[i>>2];
                eachCpuArray[i] = (float)(cpuValue & 0xFF);
                eachCpu[i>>2] = eachCpu[i>>2] >> 8;
            }

            // Outputs
            return (ctx) => {
                RawValue.Set(ctx, ret);
                
                SystemTotalCpu.Set(ctx, (float)ret[MonitorItem.SystemTotalCpu]/100F);
                SystemTotalCpuMA.Set(ctx, (float)ret[MonitorItem.SystemTotalCpuMA]/100F);
                SystemEachCpu.Set(ctx, eachCpuArray);
                
                SystemTotalPhysicalMemory.Set(ctx, ret[MonitorItem.SystemTotalPhysicalMemory]);
                SystemAvailablePhisycalMemory.Set(ctx, ret[MonitorItem.SystemAvailablePhisycalMemory]);
                SystemTotalVirtualMemory.Set(ctx, ret[MonitorItem.SystemTotalVirtualMemory]);
                SystemAvilableVirtualMemory.Set(ctx, ret[MonitorItem.SystemAvilableVirtualMemory]);

                ProcessWorkingSet.Set(ctx, ret[MonitorItem.ProcessWorkingSet]);
                ProcessWorkingSetMA.Set(ctx, ret[MonitorItem.ProcessWorkingSetMA]);
                ProcessPrivateMemorySize.Set(ctx, ret[MonitorItem.ProcessPrivateMemorySize]);
                ProcessPrivateMemorySizeMA.Set(ctx, ret[MonitorItem.ProcessPrivateMemorySizeMA]);

                ProcessTotalManagedMemory.Set(ctx, ret[MonitorItem.ProcessTotalManagedMemory]);
                ProcessTotalProcessorTime.Set(ctx, ret[MonitorItem.ProcessTotalProcessorTime]);
                ProcessTotalProcessorTimeDelta.Set(ctx, ret[MonitorItem.ProcessTotalProcessorTimeDelta]);
                ProcessResponding.Set(ctx, ret[MonitorItem.ProcessResponding]!=0 ? true : false);
                ProcessRespondingCount.Set(ctx, (int)ret[MonitorItem.ProcessRespondingCount]);
            };
        }

        #endregion
    }
}

