using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Subwidget.Contracts;
using Appalachia.UI.Functionality.Buttons.Controls.Default.Base;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Subwidget
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class ActivityBarSubwidgetControl : BaseAppaButtonControl<ActivityBarSubwidgetControl
                                                          , ActivityBarSubwidgetControlConfig>,
                                                      IActivityBarSubwidgetControl
    {
    }
}
