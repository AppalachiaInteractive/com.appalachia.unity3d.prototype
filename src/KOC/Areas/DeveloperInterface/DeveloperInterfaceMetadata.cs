using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Functionality.Canvas.Controls.Unscaled;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class
        DeveloperInterfaceMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
                                                          IDeveloperInterfaceMetadata
        where TManager : DeveloperInterfaceManager<TManager, TMetadata>
        where TMetadata : DeveloperInterfaceMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField] public UnscaledCanvasControlConfig unscaledCanvas;
        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                UnscaledCanvasControlConfig.Refresh(ref unscaledCanvas, this);
                
#if UNITY_EDITOR
                InitializeEditor(initializer);
#endif
            }
        }

        #region IDeveloperInterfaceMetadata Members

        public UnscaledCanvasControlConfig UnscaledCanvas => unscaledCanvas;

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.DeveloperInterface;

        #endregion
    }
}
