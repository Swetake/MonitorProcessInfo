using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Activities.Statements;
using System.ComponentModel;
using MonitorProcessInfo.Activities.Properties;
using MonitorProcessInfo;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using System.Activities.Expressions;

namespace MonitorProcessInfo.Activities
{
    /// <summary>
    /// MonitorProcessInfoScope
    /// </summary>
    [LocalizedDisplayName(nameof(Resources.MonitorProcessInfoScope_DisplayName))]
    [LocalizedDescription(nameof(Resources.MonitorProcessInfoScope_Description))]
    public class MonitorProcessInfoScope : ContinuableAsyncNativeActivity
    {
        #region Properties
        /// <summary>
        /// Body
        /// </summary>
        [Browsable(false)]
        public ActivityAction<IObjectContainer​> Body { get; set; }

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        /// <summary>
        /// Interval
        /// </summary>
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [LocalizedDisplayName(nameof(Resources.MonitorProcessInfoScope_Interval_DisplayName))]
        [LocalizedDescription(nameof(Resources.MonitorProcessInfoScope_Interval_Description))]
        public InArgument<int> Interval { get; set; } = 2;


        /// <summary>
        /// Number
        /// </summary>
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [LocalizedDisplayName(nameof(Resources.MonitorProcessInfoScope_Number_DisplayName))]
        [LocalizedDescription(nameof(Resources.MonitorProcessInfoScope_Number_Description))]
        public InArgument<int> Number { get; set; } = 5;



        // A tag used to identify the scope in the activity context
        internal static string ParentContainerPropertyTag => "ScopeActivity";

        // Object Container: Add strongly-typed objects here and they will be available in the scope's child activities.
        private readonly IObjectContainer _objectContainer;

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objectContainer"></param>
        public MonitorProcessInfoScope(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;



            Body = new ActivityAction<IObjectContainer>
            {
                Argument = new DelegateInArgument<IObjectContainer> (ParentContainerPropertyTag),
                Handler = new Sequence { DisplayName = Resources.Do }
            };
        }

        /// <summary>
        /// Constructor without arguments
        /// </summary>
        public MonitorProcessInfoScope() : this(new ObjectContainer())
        {

        }

        #endregion


        #region Protected Methods
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {

            base.CacheMetadata(metadata);

            if (Interval != null)
            {
                var intervalValue = (Literal<int>)Interval.Expression;
                if (intervalValue != null)
                {
                    string propertyName = Resources.MonitorProcessInfoScope_Interval_DisplayName;
                    if (intervalValue.Value <= 0) { metadata.AddValidationError(String.Format(Resources.MonitorProcessInfoScope_ValidationMsg_Not_Positive,propertyName));}
                }
            }

            if (Number != null)
            {
                var numberValue = (Literal<int>)Number.Expression;
                if (numberValue != null)
                {
                    string propertyName = Resources.MonitorProcessInfoScope_Number_DisplayName;
                    if (numberValue.Value <= 0) { metadata.AddValidationError(String.Format(Resources.MonitorProcessInfoScope_ValidationMsg_Not_Positive, propertyName));}
                }
            }

        }

        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<Action<NativeActivityContext>> ExecuteAsync(NativeActivityContext  context, CancellationToken cancellationToken)
        {
            var interval = Interval.Get(context);
            var number = Number.Get(context);

            MonitorProcess mi = new MonitorProcess(interval,number);

            return (ctx) => {
                // Schedule child activities
                if (Body != null)
				    ctx.ScheduleAction<IObjectContainer>(Body, _objectContainer, OnCompleted, OnFaulted);

                // Outputs
            };
        }

        #endregion


        #region Events

        private void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            faultContext.CancelChildren();
            Cleanup();
        }

        private void OnCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            Cleanup();
        }

        #endregion


        #region Helpers
        
        private void Cleanup()
        {
            MonitorProcess.TerminateMonitoring();

            Thread.Sleep(500);

            var disposableObjects = _objectContainer.Where(o => o is IDisposable);
            foreach (var obj in disposableObjects)
            {
                if (obj is IDisposable dispObject)
                    dispObject.Dispose();
            }
            _objectContainer.Clear();
        }

        #endregion
    }
}

