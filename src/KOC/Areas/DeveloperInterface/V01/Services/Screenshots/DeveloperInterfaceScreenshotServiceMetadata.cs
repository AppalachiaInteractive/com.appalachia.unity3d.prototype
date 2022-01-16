using Appalachia.Prototype.KOC.Areas.Common.Services.Screenshot;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Services.Screenshots
{
    public sealed class DeveloperInterfaceScreenshotServiceMetadata : ScreenshotServiceMetadata<
        DeveloperInterfaceScreenshotService, DeveloperInterfaceScreenshotServiceMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        public override void Apply(DeveloperInterfaceScreenshotService functionality)
        {
            using (_PRF_Apply.Auto())
            {
                base.Apply(functionality);
            }
        }
    }
}
