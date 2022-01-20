using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Controls.Sets.RootCanvas;
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

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Unscaled_Canvas, Expanded = false)]
        protected RootCanvasComponentSetStyle unscaledCanvas;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    APPASTR.Unscaled_View,
                    unscaledCanvas == null,
                    () =>
                    {
                        using (_PRF_Initialize.Auto())
                        {
                            unscaledCanvas =
                                RootCanvasComponentSetStyle.LoadOrCreateNew<RootCanvasComponentSetStyle>(
                                    $"Unscaled{nameof(RootCanvasComponentSetStyle)}",
                                    ownerType: typeof(ApplicationManager)
                                );
                        }
                    }
                );
            }
        }

        #region IDeveloperInterfaceMetadata Members

        public RootCanvasComponentSetStyle UnscaledCanvas => unscaledCanvas;

        public override ApplicationArea Area => ApplicationArea.DeveloperInterface;

        #endregion
    }
}
