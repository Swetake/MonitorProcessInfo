using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using MonitorProcessInfo.Activities.Design.Designers;
using MonitorProcessInfo.Activities.Design.Properties;

namespace MonitorProcessInfo.Activities.Design
{
    /// <summary>
    /// 
    /// </summary>
    public class DesignerMetadata : IRegisterMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        public void Register()
        {
            #region Setup

            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            #endregion Setup


            builder.AddCustomAttributes(typeof(GetProcessFromMainWindow), categoryAttribute);
            builder.AddCustomAttributes(typeof(GetProcessFromMainWindow), new DesignerAttribute(typeof(GetProcessFromMainWindowDesigner)));
            builder.AddCustomAttributes(typeof(GetProcessFromMainWindow), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(GetCurrentProcess), categoryAttribute);
            builder.AddCustomAttributes(typeof(GetCurrentProcess), new DesignerAttribute(typeof(GetCurrentProcessDesigner)));
            builder.AddCustomAttributes(typeof(GetCurrentProcess), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(SetMonitorProcess), categoryAttribute);
            builder.AddCustomAttributes(typeof(SetMonitorProcess), new DesignerAttribute(typeof(SetMonitorProcessDesigner)));
            builder.AddCustomAttributes(typeof(SetMonitorProcess), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(GetMonitoredValue), categoryAttribute);
            builder.AddCustomAttributes(typeof(GetMonitoredValue), new DesignerAttribute(typeof(GetProcessInfoDesigner)));
            builder.AddCustomAttributes(typeof(GetMonitoredValue), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(MonitorProcessInfoScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(MonitorProcessInfoScope), new DesignerAttribute(typeof(MonitorProcessInfoScopeDesigner)));
            builder.AddCustomAttributes(typeof(MonitorProcessInfoScope), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
