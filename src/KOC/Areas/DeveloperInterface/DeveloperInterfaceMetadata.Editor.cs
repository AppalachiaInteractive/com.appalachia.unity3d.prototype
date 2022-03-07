#if UNITY_EDITOR
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class DeveloperInterfaceMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup(COMMON_FOLDOUT_GROUP_INNER + APPASTR.Templates, Expanded = false)]
        [SerializeField] public Appalachia.UI.Controls.Sets2.DesignTemplate.DesignTemplateComponentSetData.Optional unscaledTemplates;

        #endregion

        /// <inheritdoc />
        protected override void InitializeEditor(Initializer initializer)
        {
            using (_PRF_InitializeEditor.Auto())
            {
                base.InitializeEditor(initializer);
            }
        }

        #region IDeveloperInterfaceMetadata Members

        public Appalachia.UI.Controls.Sets2.DesignTemplate.DesignTemplateComponentSetData.Optional UnscaledTemplates => unscaledTemplates;

        #endregion
    }
}

#endif