using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Controls.Sets.DesignTemplate;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas
{
    [SmartLabelChildren]
    [InspectorIcon(Brand.AreaMetadata.Icon)]
    [AssetLabel(Brand.AreaManager.Label)]
    public abstract partial class AreaMetadata<TManager, TMetadata>
    {
        #region Constants and Static Readonly

        protected const string FOLDOUT_GROUP = FOLDOUT_GROUP_ + "/";

        private const string FOLDOUT_GROUP_ = APPASTR.Common;

        #endregion

        #region Fields and Autoproperties

        [FormerlySerializedAs("scaledTemplates")]
        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Templates, Expanded = false)]
        public DesignTemplateComponentSetStyle templates;

        #endregion

        protected virtual void InitializeEditor(Initializer initializer)
        {
            using (_PRF_InitializeEditor.Auto())
            {
                initializer.Do(
                    this,
                    APPASTR.Scaled_Templates,
                    templates == null,
                    () =>
                    {
                        using (_PRF_Initialize.Auto())
                        {
                            templates = LoadOrCreateNew<DesignTemplateComponentSetStyle>(
                                $"{Area}Scaled{nameof(DesignTemplateComponentSetStyle)}"
                            );
                        }
                    }
                );
            }
        }

        protected override string GetBackgroundColor()
        {
            return Brand.AreaMetadata.Banner;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.AreaMetadata.Fallback;
        }

        protected override string GetTitle()
        {
            return Brand.AreaMetadata.Text;
        }

        protected override string GetTitleColor()
        {
            return Brand.AreaMetadata.Color;
        }

        #region IAreaMetadata Members

        public DesignTemplateComponentSetStyle Templates => templates;

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_InitializeEditor =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEditor));

        #endregion
    }
}
