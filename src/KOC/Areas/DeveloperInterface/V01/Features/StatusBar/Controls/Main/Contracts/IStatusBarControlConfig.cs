using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.ControlModel.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Layout.Groups.Horizontal;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Main.Contracts
{
    public interface IStatusBarControlConfig : IAppaUIControlConfig
    {
        HorizontalLayoutGroupComponentGroupConfig LeftStatusBar { get; }
        HorizontalLayoutGroupComponentGroupConfig RightStatusBar { get; }
        RectTransformConfig IconRectTransform { get; }
    }
}
