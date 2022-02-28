namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets
{
    public interface IDevTooltipSubwidgetController
    {
        string GetDevTooltipText();
        void OnDevTooltipUpdateRequested();
    }
}
