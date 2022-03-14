using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Model;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.Functionality.Canvas.Controls.Default;
using Appalachia.UI.Styling.Fonts;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts
{
    public interface IApplicationSubwidgetMetadata : IPrioritySubwidgetMetadata
    {
        CanvasControlConfig.Optional Canvas { get; }

        RectTransformConfig.Override RectTransform { get; }

        bool TransitionsWithFade { get; }

        float AnimationDuration { get; }

        FontStyleOverride FontStyle { get; set; }

        SubwidgetVisibilityMode WidgetDisabledVisibilityMode { get; }

        SubwidgetVisibilityMode WidgetEnabledVisibilityMode { get; }
    }

    public interface IApplicationSubwidgetMetadata<in T> : IApplicationSubwidgetMetadata,
                                                           IApplicationFunctionalityMetadata<T>
        where T : IApplicationSubwidget
    {
    }

    public interface IApplicationSubwidgetMetadata<in T, TMetadata> : IApplicationSubwidgetMetadata<T>
        where T : IApplicationSubwidget<T, TMetadata>
        where TMetadata : IApplicationSubwidgetMetadata<T, TMetadata>
    {
    }
}
