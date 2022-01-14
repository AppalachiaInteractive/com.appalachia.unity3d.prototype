namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings
{
    public interface IRuntimeGraphSettings
    {
        public int GraphResolution { get; }
        public int TextUpdateRate { get; }
        void Reset();
    }
}
