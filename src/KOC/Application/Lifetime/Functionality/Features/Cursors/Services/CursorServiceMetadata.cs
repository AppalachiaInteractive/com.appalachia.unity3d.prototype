using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Services
{
    public class CursorServiceMetadata : LifetimeServiceMetadata<CursorService, CursorServiceMetadata,
        CursorFeature, CursorFeatureMetadata>
    {
        #region Fields and Autoproperties

        public SoftwareCursorStateFlags initialSoftwareCursorState;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(initialSoftwareCursorState),
                    () => initialSoftwareCursorState =
                        SoftwareCursorStateFlags.Hidden | SoftwareCursorStateFlags.Confined
                );
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(CursorService target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(CursorService functionality)
        {
            using (_PRF_UpdateFunctionality.Auto())
            {
            }
        }
    }
}
