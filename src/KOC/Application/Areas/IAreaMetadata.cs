using Appalachia.Prototype.KOC.Application.Components.UI;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    public interface IAreaMetadata
    {
        ApplicationArea Area { get; }

        public AreaMetadataConfigurations.AreaCursorConfiguration Cursor { get; }
        public AreaMetadataConfigurations.AreaInputConfiguration Input { get; }
        public AreaMetadataConfigurations.AreaCanvasConfiguration Canvas { get; }
        public AreaMetadataConfigurations.AreaViewConfiguration View { get; }
        public AreaMetadataConfigurations.AreaGraphicRaycasterConfiguration GraphicRaycaster { get; }
        public AreaMetadataConfigurations.AreaDoozyCanvasConfiguration DoozyCanvas { get; }
        public AreaMetadataConfigurations.AreaDoozyGraphConfiguration DoozyGraph { get; }
        public AreaMetadataConfigurations.AreaDoozyViewConfiguration DoozyView { get; }
        public AreaMetadataConfigurations.AreaMenuConfiguration Menu { get; }
        public AreaMetadataConfigurations.AreaTemplatesConfiguration Templates { get; }
        public AreaMetadataConfigurations.AreaDefaultReferencesConfiguration DefaultReferences { get; }
        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration SceneBehaviour { get; }
        public AreaMetadataConfigurations.AreaAudioConfiguration Audio { get; }
        
        void Apply(UITemplateComponentSet target);
        void Apply(UIViewComponentSet target);
        void Apply(UICanvasAreaComponentSet target);
    }
}
