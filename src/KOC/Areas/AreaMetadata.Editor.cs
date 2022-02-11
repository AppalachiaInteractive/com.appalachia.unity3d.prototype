#if UNITY_EDITOR
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

        protected const string FOLDOUT_GROUP_INNER = FOLDOUT_GROUP + "/";

        protected const string FOLDOUT_GROUP = APPASTR.Common;

        #endregion

        #region Fields and Autoproperties

        [FormerlySerializedAs("scaledTemplates")]
        [SerializeField, FoldoutGroup(FOLDOUT_GROUP_INNER + APPASTR.Templates, Expanded = false)]
        public DesignTemplateComponentSetData templates;

        #endregion

        protected virtual void InitializeEditor(Initializer initializer)
        {
            using (_PRF_InitializeEditor.Auto())
            {
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

        public DesignTemplateComponentSetData Templates => templates;

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_InitializeEditor =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEditor));

        #endregion
    }
}

#endif