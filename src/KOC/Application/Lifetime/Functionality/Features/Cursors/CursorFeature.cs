using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors
{
    public class CursorFeature : LifetimeFeature<CursorFeature, CursorFeatureMetadata>
    {
        /// <inheritdoc />
        protected override async AppaTask BeforeDisable()
        {
            await AppaTask.CompletedTask;
        }

        


    }
}
