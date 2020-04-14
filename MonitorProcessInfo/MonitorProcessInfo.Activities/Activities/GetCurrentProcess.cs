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
    /// Get Current Process
    /// </summary>
    [LocalizedDisplayName(nameof(Resources.GetCurrentProcess_DisplayName))]
    [LocalizedDescription(nameof(Resources.GetCurrentProcess_Description))]
    public class GetCurrentProcess : ContinuableAsyncCodeActivity
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
        [LocalizedDisplayName(nameof(Resources.GetCurrentProcess_Process_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetCurrentProcess_Process_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<Process> Process { get; set; }

        #endregion


        #region Constructors
        /// <summary>
        /// Constrctor
        /// </summary>
        public GetCurrentProcess()
        {
        }

        #endregion


        #region Protected Methods
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {

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
            
            var ps = System.Diagnostics.Process.GetCurrentProcess();
            // Outputs
            return (ctx) => {
                Process.Set(ctx, ps);
            };
        }

        #endregion
    }
}

