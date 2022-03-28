using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Subwidget.Contracts;
using Appalachia.UI.Functionality.Buttons.Controls.Default.Base;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Subwidget
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class StatusBarSubwidgetControl : BaseAppaButtonControl<StatusBarSubwidgetControl
                                                        , StatusBarSubwidgetControlConfig>,
                                                    IStatusBarSubwidgetControl
    {
    }
}
