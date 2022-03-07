#if UNITY_EDITOR
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class DeveloperInterfaceManager<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField]
        [FoldoutGroup(APPASTR.Components + "/" + APPASTR.Unscaled_Templates)]
        protected Appalachia.UI.Controls.Sets2.DesignTemplate.DesignTemplateComponentSet unscaledTemplates;
        #endregion

        private void InitializeEditor(Initializer initializer, string setName)
        {
            using (_PRF_InitializeEditor.Auto())
            {
                Appalachia.UI.Controls.Sets2.DesignTemplate.DesignTemplateComponentSetData.RefreshAndUpdate(
                    ref areaMetadata.unscaledTemplates,
                    false,
                    ref unscaledTemplates,
                    unscaledCanvas.GameObject,
                    $"{Area}Unscaled"
                );
            }
        }
    }
}

#endif
