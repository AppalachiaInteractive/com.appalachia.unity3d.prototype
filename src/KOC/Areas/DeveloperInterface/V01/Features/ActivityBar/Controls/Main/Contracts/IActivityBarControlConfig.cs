using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.ControlModel.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Layout.Groups.Vertical;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Main.Contracts
{
    public interface IActivityBarControlConfig : IAppaUIControlConfig
    {
        VerticalLayoutGroupComponentGroupConfig TopActivityBar { get; }
        VerticalLayoutGroupComponentGroupConfig BottomActivityBar { get; }
        RectTransformConfig IconRectTransform { get; }
    }
}
