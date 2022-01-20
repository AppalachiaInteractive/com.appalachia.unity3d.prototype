using Appalachia.Core.Objects.Initialization;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class DeveloperInterfaceManager<TManager, TMetadata>
    {
        private void InitializeEditor(Initializer initializer, string fullObjectName)
        {
            using (_PRF_InitializeEditor.Auto())
            {
                areaMetadata.unscaledTemplates.PrepareAndConfigure(
                    ref unscaledTemplates,
                    unscaledCanvas.GameObject,
                    fullObjectName
                );
            }
        }
    }
}
