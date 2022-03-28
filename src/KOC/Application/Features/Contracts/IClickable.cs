using Appalachia.UI.Functionality.Buttons.Components;

namespace Appalachia.Prototype.KOC.Application.Features.Contracts
{
    public interface IClickable
    {
        AppaButton ClickableControl { get; }
        void OnClicked();
    }
}
