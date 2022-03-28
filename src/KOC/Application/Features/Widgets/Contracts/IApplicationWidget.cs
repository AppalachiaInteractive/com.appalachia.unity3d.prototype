using System;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.UI.ControlModel.Controls.Default.Contracts;

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
        void ForEachControl(Action<IAppaUIControl> action);
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
