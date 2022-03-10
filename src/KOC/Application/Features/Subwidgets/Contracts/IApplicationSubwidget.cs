using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts
{
    public interface IApplicationSubwidget : IApplicationFunctionality, IUIBehaviour
    {
        bool IsVisible { get; }
        float EffectiveAnchorHeight { get; }
        float EffectiveAnchorWidth { get; }
        void Hide();
        void OnDisableWidget();
        void OnEnableWidget();
        void SetVisibility(bool setVisibilityTo);
        void Show();
        void ToggleVisibility();
        int Priority { get; }
    }

    public interface IApplicationSubwidget<T> : IApplicationSubwidget
        where T : IApplicationSubwidget<T>
    {
    }

    public interface IApplicationSubwidget<T, out TMetadata> : IApplicationSubwidget<T>
        where T : IApplicationSubwidget<T, TMetadata>
        where TMetadata : IApplicationSubwidgetMetadata<T>
    {
        TMetadata Metadata { get; }
    }
}
