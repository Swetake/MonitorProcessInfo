using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using MonitorProcessInfo.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace MonitorProcessInfo.Activities
{
    /// <summary>
    /// SetMonitorProcess
    /// </summary>
    [LocalizedDisplayName(nameof(Resources.SetMonitorProcess_DisplayName))]
    [LocalizedDescription(nameof(Resources.SetMonitorProcess_Description))]
    public class SetMonitorProcess : ContinuableAsyncCodeActivity
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
        /// Process to monitor
        /// </summary>
        [LocalizedDisplayName(nameof(Resources.SetMonitorProcess_Process_DisplayName))]
        [LocalizedDescription(nameof(Resources.SetMonitorProcess_Process_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<Process> Process { get; set; }

        #endregion


        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public SetMonitorProcess()
        {
        }

        #endregion


        #region Protected Methods
        /// <summary>
        /// Cachemetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Process == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Process)));

            base.CacheMetadata(metadata);
        }

        /// <summary>
        /// ExecuteAync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var process = Process.Get(context);

            MonitorProcess.SetOrder(process);
    
            // Outputs
            return (ctx) => {
            };
        }

        #endregion
    }
}

