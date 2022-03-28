#if UNITY_EDITOR
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Functionality.Design.Controls.Template;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class DeveloperInterfaceMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup(COMMON_FOLDOUT_GROUP_INNER + APPASTR.Templates, Expanded = false)]
        [SerializeField] public TemplateControlConfig.Optional unscaledTemplates;

        #endregion

        /// <inheritdoc />
        protected override void InitializeEditor(Initializer initializer)
        {
            using (_PRF_InitializeEditor.Auto())
            {
                base.InitializeEditor(initializer);

                TemplateControlConfig.Refresh(ref unscaledTemplates, false, this);
            }
        }

        #region IDeveloperInterfaceMetadata Members

        public TemplateControlConfig.Optional UnscaledTemplates => unscaledTemplates;

        #endregion
    }
}

#endif
