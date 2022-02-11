using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Controls.Sets.UnscaledCanvas;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
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

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP_INNER + APPASTR.Unscaled_Canvas, Expanded = false)]
        public UnscaledCanvasComponentSetData unscaledCanvas;

        #endregion

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

        public UnscaledCanvasComponentSetData UnscaledCanvas => unscaledCanvas;
        
        #region IDeveloperInterfaceMetadata Members

        public override ApplicationArea Area => ApplicationArea.DeveloperInterface;

        #endregion
    }
}
