#if UNITY_EDITOR
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Controls.Sets2.DesignTemplate;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class DeveloperInterfaceMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField, FoldoutGroup(COMMON_FOLDOUT_GROUP_INNER + APPASTR.Templates, Expanded = false)]
        public DesignTemplateComponentSetData.Optional unscaledTemplates;

        [SerializeField, FoldoutGroup(COMMON_FOLDOUT_GROUP_INNER + APPASTR.Templates, Expanded = false)]
        public DesignTemplateComponentSetData.Optional unscaledTemplates2;
        #endregion

        /// <inheritdoc />
        protected override void InitializeEditor(Initializer initializer)
        {
            using (_PRF_InitializeEditor.Auto())
            {
                base.InitializeEditor(initializer);

                unscaledTemplates = unscaledTemplates2;
            }
        }

        #region IDeveloperInterfaceMetadata Members

        public DesignTemplateComponentSetData.Optional UnscaledTemplates => unscaledTemplates;

        #endregion
    }
}

#endif
