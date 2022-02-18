using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Features;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors
{
    public class CursorFeature : LifetimeFeature<CursorFeature, CursorFeatureMetadata>
    {
        /// <inheritdoc />
        protected override async AppaTask BeforeDisable()
        {
            await AppaTask.CompletedTask;
        }

        /// <inheritdoc />
        protected override async AppaTask BeforeEnable()
        {
            await AppaTask.CompletedTask;
        }

        /// <inheritdoc />
        protected override async AppaTask BeforeFirstEnable()
        {
            await AppaTask.CompletedTask;
        }

        /// <inheritdoc />
        protected override async AppaTask OnHide()
        {
            await AppaTask.CompletedTask;
        }

        /// <inheritdoc />
        protected override async AppaTask OnShow()
        {
            await AppaTask.CompletedTask;
        }
    }
}
