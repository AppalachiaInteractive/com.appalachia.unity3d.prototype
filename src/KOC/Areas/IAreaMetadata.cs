using Appalachia.UI.Controls.Sets;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas
{
    public interface IAreaMetadata
    {
        ApplicationArea Area { get; }
        public AreaMetadataConfigurations.AreaAudioConfiguration Audio { get; }
        public AreaMetadataConfigurations.AreaCanvasConfiguration Canvas { get; }

        public AreaMetadataConfigurations.AreaCursorConfiguration Cursor { get; }
        public AreaMetadataConfigurations.AreaDefaultReferencesConfiguration DefaultReferences { get; }

        public AreaMetadataConfigurations.AreaGraphicRaycasterConfiguration GraphicRaycaster { get; }
        public AreaMetadataConfigurations.AreaInputConfiguration Input { get; }
        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration SceneBehaviour { get; }
        public AreaMetadataConfigurations.AreaTemplatesConfiguration Templates { get; }
        public AreaMetadataConfigurations.AreaViewConfiguration View { get; }

        void Apply(TemplateComponentSet target, GameObject manager, GameObject canvas, GameObject view);

    }
}
