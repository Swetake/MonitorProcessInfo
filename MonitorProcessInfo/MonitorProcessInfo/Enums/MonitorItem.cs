using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorProcessInfo
{
    /// <summary>
    /// MonitorItem enum
    /// </summary>
    public enum MonitorItem
    {
        /// <summary>
        /// System total cpu
        /// </summary>
        SystemTotalCpu,

        /// <summary>
        /// Moving Average of Total CPU
        /// </summary>
        SystemTotalCpuMA,

        /// <summary>
        /// CPU for #00 -#07
        /// </summary>
        SystemEachCpu00to07,

        /// <summary>
        /// CPU for #08-#15
        /// </summary>
        SystemEachCpu08to15,

        /// <summary>
        /// CPU for #16-#23
        /// </summary>
        SystemEachCpu16to23,

        /// <summary>
        /// CPU for #24-#31
        /// </summary>
        SystemEachCpu24to31,

        /// <summary>
        /// SystemTotalPhysicalMemory
        /// </summary>
        SystemTotalPhysicalMemory,

        /// <summary>
        ///  SystemAvailablePhisycalMemory
        /// </summary>
        SystemAvailablePhisycalMemory,

        /// <summary>
        /// SystemTotalVirtualMemory
        /// </summary>
        SystemTotalVirtualMemory,

        /// <summary>
        /// SystemAvilableVirtualMemory
        /// </summary>
        SystemAvilableVirtualMemory,

        /// <summary>
        /// WorkingSet
        /// </summary>
        ProcessWorkingSet,

        /// <summary>
        /// WorkingSetmovingAverage
        /// </summary>
        ProcessWorkingSetMA,

        /// <summary>
        /// TotalManagedMemory
        /// </summary>
        ProcessTotalManagedMemory,

        /// <summary>
        /// TotalProcessorTime
        /// </summary>
        ProcessTotalProcessorTime,

        /// <summary>
        /// TotalProcessorTimeDelta
        /// </summary>
        ProcessTotalProcessorTimeDelta,

        /// <summary>
        /// Responding
        /// </summary>
        ProcessResponding,

        /// <summary>
        /// Responding Continuous Count
        /// </summary>
        ProcessRespondingCount
    }
}
