using Appalachia.Core.Objects.Initialization;
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

        [SerializeField] public Appalachia.UI.Controls.Sets2.Canvases.UnscaledCanvas.UnscaledCanvasComponentSetData unscaledCanvas;
        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
#if UNITY_EDITOR
                InitializeEditor(initializer);
#endif
            }
        }

        #region IDeveloperInterfaceMetadata Members

        public Appalachia.UI.Controls.Sets2.Canvases.UnscaledCanvas.UnscaledCanvasComponentSetData UnscaledCanvas => unscaledCanvas;

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.DeveloperInterface;

        #endregion
    }
}
