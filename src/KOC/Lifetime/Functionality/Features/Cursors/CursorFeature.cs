using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Features;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors
{
    public class CursorFeature : LifetimeFeature<CursorFeature, CursorFeatureMetadata>
    {
        protected override async AppaTask BeforeDisable()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask BeforeEnable()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask BeforeFirstEnable()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask OnHide()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask OnShow()
        {
            await AppaTask.CompletedTask;
        }
    }
}
