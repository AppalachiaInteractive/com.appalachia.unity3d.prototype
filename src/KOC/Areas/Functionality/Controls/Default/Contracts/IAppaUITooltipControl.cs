using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Functionality.Controls.DevTooltips.Control;
using Appalachia.UI.ControlModel.Controls.Default.Contracts;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Functionality.Controls.Default.Contracts
{
    public interface IAppaUITooltipControl : IAppaUIControl
    {
        DevTooltipControl Tooltip { get; }
    }

    public interface IAppaUITooltipControl<TControl> : IAppaUITooltipControl, IAppaUIControl<TControl>
    {
    }

    public interface IAppaUITooltipControl<TControl, TConfig> : IAppaUITooltipControl<TControl>, IAppaUIControl<TControl, TConfig>
        where TControl : IAppaUITooltipControl<TControl, TConfig>
        where TConfig : IAppaUITooltipControlConfig<TControl, TConfig>, new()
    {
    }
}
