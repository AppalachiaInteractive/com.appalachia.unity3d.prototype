namespace Appalachia.Prototype.KOC.Application.Widgets
{
    public interface IApplicationWidget : IApplicationFunctionality
    {
        bool IsVisible { get; }
        float EffectiveAnchorHeight { get; }
        float EffectiveAnchorWidth { get; }
        void Hide();
        void SetVisibility(bool setVisibilityTo, bool doRaiseEvents = true);
        void Show();
        void ToggleVisibility();
        void UpdateSize(bool doRaiseEvents = true);
    }
}
