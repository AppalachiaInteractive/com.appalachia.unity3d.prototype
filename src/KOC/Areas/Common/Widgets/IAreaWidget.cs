namespace Appalachia.Prototype.KOC.Areas.Common.Widgets
{
    public interface IAreaWidget : IAreaFunctionality
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
