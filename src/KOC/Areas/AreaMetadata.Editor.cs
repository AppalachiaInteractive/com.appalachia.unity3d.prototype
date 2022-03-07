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

        protected const string COMMON_FOLDOUT_GROUP = APPASTR.Common;

        protected const string COMMON_FOLDOUT_GROUP_INNER = COMMON_FOLDOUT_GROUP + "/";

        #endregion

        #region Fields and Autoproperties

        [FormerlySerializedAs("scaledTemplates")]
        [FoldoutGroup(COMMON_FOLDOUT_GROUP_INNER + APPASTR.Templates, Expanded = false)]
        [SerializeField] public DesignTemplateComponentSetData.Optional templates;

        #endregion

        protected virtual void InitializeEditor(Initializer initializer)
        {
            using (_PRF_InitializeEditor.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override string GetBackgroundColor()
        {
            return Brand.AreaMetadata.Banner;
        }

        /// <inheritdoc />
        protected override string GetFallbackTitle()
        {
            return Brand.AreaMetadata.Fallback;
        }

        /// <inheritdoc />
        protected override string GetTitle()
        {
            return Brand.AreaMetadata.Text;
        }

        /// <inheritdoc />
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
