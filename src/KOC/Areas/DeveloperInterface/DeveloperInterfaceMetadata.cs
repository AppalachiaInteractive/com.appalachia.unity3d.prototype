using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Controls.Sets2.Canvases.UnscaledCanvas;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class DeveloperInterfaceMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
        IDeveloperInterfaceMetadata
        where TManager : DeveloperInterfaceManager<TManager, TMetadata>
        where TMetadata : DeveloperInterfaceMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField] public UnscaledCanvasComponentSetData unscaledCanvas;

        [SerializeField] public UnscaledCanvasComponentSetData unscaledCanvas2;

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

                unscaledCanvas = unscaledCanvas2;
            }
        }

        #region IDeveloperInterfaceMetadata Members

        public UnscaledCanvasComponentSetData UnscaledCanvas => unscaledCanvas;

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.DeveloperInterface;

        #endregion
    }
}
