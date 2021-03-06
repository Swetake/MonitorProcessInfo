﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonitorProcessInfo.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MonitorProcessInfo.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exceeded number of processes.
        /// </summary>
        public static string ExceptionMsgExceededProcesses {
            get {
                return ResourceManager.GetString("ExceptionMsgExceededProcesses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid time format.
        /// </summary>
        public static string ExceptionMsgInvalidTimeFormat {
            get {
                return ResourceManager.GetString("ExceptionMsgInvalidTimeFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Key not found in Dict. .
        /// </summary>
        public static string ExceptionMsgKeyNotFoundInDict {
            get {
                return ResourceManager.GetString("ExceptionMsgKeyNotFoundInDict", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Order Shared memory file already exists or cannot be created. This activity should be used only once. .
        /// </summary>
        public static string ExceptionMsgOrderMemFileAlreadyExist {
            get {
                return ResourceManager.GetString("ExceptionMsgOrderMemFileAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The process has same ID but not same Starttime..
        /// </summary>
        public static string ExceptionMsgSameIdButNotSameStarttime {
            get {
                return ResourceManager.GetString("ExceptionMsgSameIdButNotSameStarttime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DateTime,PID,WorkingSet,WorkingSetMA,PrivteMemory,PrivateMemoryMA,TotalManagedMemory,CpuTotalTime,CpuTotalTimeDelta,Responding,ContinuousNonResponding.
        /// </summary>
        public static string LogHeaderProcess {
            get {
                return ResourceManager.GetString("LogHeaderProcess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DateTime,CPU,CPU MA,CPU00-07,CPU08-15,CPU16-23,CPU24-31,TotalPhysicalMemory,AvailablePhysicalMemory,TotalVirtualMemory,AvailableVirtualMemory.
        /// </summary>
        public static string LogHeaderSystem {
            get {
                return ResourceManager.GetString("LogHeaderSystem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MPI_Order.
        /// </summary>
        public static string OrderMemFileName {
            get {
                return ResourceManager.GetString("OrderMemFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ..
        /// </summary>
        public static string PerfomanceCounterMachineName {
            get {
                return ResourceManager.GetString("PerfomanceCounterMachineName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to _Total.
        /// </summary>
        public static string PerformaceCounterInstanceName {
            get {
                return ResourceManager.GetString("PerformaceCounterInstanceName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Processor.
        /// </summary>
        public static string PerformanceCounterCategoryName {
            get {
                return ResourceManager.GetString("PerformanceCounterCategoryName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to % Processor Time.
        /// </summary>
        public static string PerformanceCounterCounterName {
            get {
                return ResourceManager.GetString("PerformanceCounterCounterName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MPI_.
        /// </summary>
        public static string PrefixMemFileName {
            get {
                return ResourceManager.GetString("PrefixMemFileName", resourceCulture);
            }
        }
    }
}
