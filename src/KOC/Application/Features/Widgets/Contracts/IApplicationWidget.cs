using Appalachia.Prototype.KOC.Application.Functionality.Contracts;

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
        void OnEnableFeature();
        void OnDisableFeature();
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
