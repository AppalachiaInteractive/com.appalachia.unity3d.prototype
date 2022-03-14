using Appalachia.UI.Functionality.Canvas.Controls.Root;

namespace Appalachia.Prototype.KOC.Areas
{
    public partial interface IAreaMetadata
    {
        ApplicationArea Area { get; }
        public AreaMetadataConfigurations.AreaAudioConfiguration Audio { get; }
        public AreaMetadataConfigurations.AreaInputConfiguration Input { get; }
        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration SceneBehaviour { get; }
        public RootCanvasControlConfig RootCanvas { get; }
    }
}
