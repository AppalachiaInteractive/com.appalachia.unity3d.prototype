using Appalachia.Prototype.KOC.Application.Services.Screenshot;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Services.Screenshots
{
    public sealed class DeveloperInterfaceScreenshotServiceMetadata : ScreenshotServiceMetadata<
        DeveloperInterfaceScreenshotService, DeveloperInterfaceScreenshotServiceMetadata>
    {
        public override void Apply(DeveloperInterfaceScreenshotService functionality)
        {
            using (_PRF_Apply.Auto())
            {
            }
        }
    }
}
