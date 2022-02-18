#if UNITY_EDITOR
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Controls.Sets.DesignTemplate;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class DeveloperInterfaceManager<TManager, TMetadata>
    {
        private void InitializeEditor(Initializer initializer, string setName)
        {
            using (_PRF_InitializeEditor.Auto())
            {
                DesignTemplateComponentSetData.RefreshAndUpdateComponentSet(
                    ref areaMetadata.unscaledTemplates,
                    ref unscaledTemplates,
                    unscaledCanvas.GameObject,
                    $"{Area}Unscaled"
                );
            }
        }
    }
}

#endif
