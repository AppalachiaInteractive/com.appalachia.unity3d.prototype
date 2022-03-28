namespace Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Control.Contracts
{
    public interface ITooltipOwner
    {
        string GetTooltipText();
        void OnTooltipUpdateRequested();
    }
}
