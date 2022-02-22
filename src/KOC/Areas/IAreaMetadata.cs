using Appalachia.UI.Controls.Sets.Canvases.RootCanvas;

namespace Appalachia.Prototype.KOC.Areas
{
    public partial interface IAreaMetadata
    {
        ApplicationArea Area { get; }
        public AreaMetadataConfigurations.AreaAudioConfiguration Audio { get; }
        public AreaMetadataConfigurations.AreaInputConfiguration Input { get; }
        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration SceneBehaviour { get; }
        public RootCanvasComponentSetData RootCanvas { get; }
    }
}
