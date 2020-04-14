using System;
using System.Activities;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MonitorProcessInfo.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace MonitorProcessInfo.Activities
{
    /// <summary>
    /// Get Process from UiPath.Core.Window
    /// </summary>
    [LocalizedDisplayName(nameof(Resources.GetProcessFromMainWindow_DisplayName))]
    [LocalizedDescription(nameof(Resources.GetProcessFromMainWindow_Description))]
#pragma warning disable CS0436 // 型がインポートされた型と競合しています
    public class GetProcessFromMainWindow : ContinuableAsyncCodeActivity
#pragma warning restore CS0436 // 型がインポートされた型と競合しています
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
        /// UiPath.Core.Window
        /// </summary>
        [RequiredArgument]
        [LocalizedDisplayName(nameof(Resources.GetProcessFromMainWindow_Window_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetProcessFromMainWindow_Window_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<UiPath.Core.Window> Window { get; set; }

        /// <summary>
        /// Process
        /// </summary>
        [RequiredArgument]
        [LocalizedDisplayName(nameof(Resources.GetProcessFromMainWindow_Process_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetProcessFromMainWindow_Process_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<System.Diagnostics.Process> Process { get; set; }


        #endregion


        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public GetProcessFromMainWindow()
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
            // Inputs
            var window = Window.Get(context);
            var handle = window.Handle;
            Process ps = null;
            var processes = System.Diagnostics.Process.GetProcesses();
            foreach (Process p in processes)
            {
                if (p.MainWindowHandle == handle)
                {
                    ps = p;
                    break;
                }
            }
            if (ps == null)
            {
                throw (new Exception("Process not found"));
            }
 
            // Outputs
            return (ctx) => {
                Process.Set(ctx, ps);
            };
        }

        #endregion
    }
}

