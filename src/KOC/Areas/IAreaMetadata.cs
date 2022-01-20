using Appalachia.UI.Controls.Sets.Canvas;
using Appalachia.UI.Controls.Sets.RootCanvas;

namespace Appalachia.Prototype.KOC.Areas
{
    public partial interface IAreaMetadata
    {
        ApplicationArea Area { get; }
        public AreaMetadataConfigurations.AreaAudioConfiguration Audio { get; }
        public AreaMetadataConfigurations.AreaInputConfiguration Input { get; }
        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration SceneBehaviour { get; }
        public CanvasComponentSetStyle ScaledView { get; }
        public RootCanvasComponentSetStyle RootCanvas { get; }
    }
}
