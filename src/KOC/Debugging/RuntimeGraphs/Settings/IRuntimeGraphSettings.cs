namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings
{
    public interface IRuntimeGraphSettings
    {
        public int GraphResolution { get; }
        public int TextUpdateRate { get; }
        void Reset();
    }
}
