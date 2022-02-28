using Appalachia.Prototype.KOC.Application.Functionality;

namespace Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts
{
    public interface IApplicationWidget : IApplicationFunctionality
    {
        bool IsVisible { get; }
        float EffectiveAnchorHeight { get; }
        float EffectiveAnchorWidth { get; }
        void Hide();
        void SetVisibility(bool setVisibilityTo);
        void Show();
        void EnableFeature();
        void DisableFeature();
        void ToggleVisibility();
    }

    public interface IApplicationWidget<T> : IApplicationWidget
        where T : IApplicationWidget<T>
    {
    }

    public interface IApplicationWidget<T, TMetadata> : IApplicationWidget<T>
        where T : IApplicationWidget<T, TMetadata>
        where TMetadata : IApplicationWidgetMetadata<T>
    {
    }
}
