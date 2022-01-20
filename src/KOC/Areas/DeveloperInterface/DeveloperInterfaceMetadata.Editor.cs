using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Controls.Sets.DesignTemplate;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class DeveloperInterfaceMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Templates, Expanded = false)]
        public DesignTemplateComponentSetStyle unscaledTemplates;

        #endregion

        protected override void InitializeEditor(Initializer initializer)
        {
            using (_PRF_InitializeEditor.Auto())
            {
                base.InitializeEditor(initializer);

                initializer.Do(
                    this,
                    APPASTR.Unscaled_Templates,
                    unscaledTemplates == null,
                    () =>
                    {
                        using (_PRF_Initialize.Auto())
                        {
                            unscaledTemplates = LoadOrCreateNew<DesignTemplateComponentSetStyle>(
                                $"{Area}Unscaled{nameof(DesignTemplateComponentSetStyle)}"
                            );
                        }
                    }
                );
            }
        }

        #region IDeveloperInterfaceMetadata Members

        public DesignTemplateComponentSetStyle UnscaledTemplates => unscaledTemplates;

        #endregion
    }
}
