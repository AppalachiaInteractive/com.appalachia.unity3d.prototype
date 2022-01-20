using Appalachia.Prototype.KOC.Application.Services.Screenshot;
using Appalachia.Prototype.KOC.Areas.Common.Services;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Services.Screenshots
{
    public sealed class DeveloperInterfaceScreenshotService : ScreenshotService<
                                                                  DeveloperInterfaceScreenshotService,
                                                                  DeveloperInterfaceScreenshotServiceMetadata>,
                                                              IAreaService
    {
        protected override bool NestUnderApplicationManager => false;

        protected override void SubscribeToAllFunctionalties()
        {
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
        }
    }
}
