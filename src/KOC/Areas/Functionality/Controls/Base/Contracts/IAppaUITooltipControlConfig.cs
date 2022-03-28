using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Functionality.Controls.DevTooltips.Control;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.ControlModel.Controls.Default.Contracts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Functionality.Controls.Default.Contracts
{
    public interface IAppaUITooltipControlConfig : IAppaUIControlConfig
    {
        DevTooltipControlConfig Tooltip { get; }
    }

    public interface IAppaUITooltipControlConfig<in TControl> : IAppaUITooltipControlConfig, IAppaUIControlConfig<TControl>
    {
    }

    public interface IAppaUITooltipControlConfig<in TControl, TConfig> : IAppaUITooltipControlConfig<TControl>,
                                                                  IAppaUIControlConfig<TControl, TConfig>
        where TControl : IAppaUITooltipControl<TControl, TConfig>
        where TConfig : IAppaUITooltipControlConfig<TControl, TConfig>, new()
    {
    }
}
