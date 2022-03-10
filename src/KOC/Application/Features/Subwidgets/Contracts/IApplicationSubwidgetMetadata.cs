using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Model;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.UI.Controls.Sets.Canvases.Canvas;
using Appalachia.UI.Controls.Sets.Images.Background;
using Appalachia.UI.Controls.Sets.Images.RoundedBackground;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Styling.Fonts;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts
{
    public interface IApplicationSubwidgetMetadata : IPrioritySubwidgetMetadata
    {
        BackgroundComponentSetData.Optional Background { get; }

        CanvasComponentSetData.Optional Canvas { get; }

        RectTransformData.Override RectTransform { get; }

        RoundedBackgroundComponentSetData.Optional RoundedBackground { get; }

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
