using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.ControlModel.Controls.Model;
using Appalachia.UI.Functionality.Buttons.Controls.Default.Base;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Sets
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class StatusBarSubwidgetControl : BaseAppaButtonControl<StatusBarSubwidgetControl
        , StatusBarSubwidgetControlConfig>
    {
        /// <inheritdoc />
        public override ControlSorting DesiredComponentOrder => ControlSorting.CloseToLast;

        
    }
}
