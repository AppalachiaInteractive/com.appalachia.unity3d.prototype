using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core
{
    public abstract partial class SideBarSubwidget<TSubwidget, TSubwidgetMetadata>
    {
        protected abstract void OnActivate();
        protected abstract void OnDeactivate();

        [ButtonGroup(nameof(Activate))]
        protected void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                Widget.SetActiveSubwidget(this);

                canvas.Enable(metadata.canvas);

                OnActivate();
            }
        }

        [ButtonGroup(nameof(Activate))]
        protected void Deactivate()
        {
            using (_PRF_Deactivate.Auto())
            {
                canvas.Disable();
                OnDeactivate();
            }
        }

        [ButtonGroup(nameof(Activate))]
        protected void Toggle()
        {
            using (_PRF_Toggle.Auto())
            {
                if (IsActive)
                {
                    Widget.DeactivateActiveSubwidget();
                }
                else
                {
                    Activate();
                }
            }
        }

        #region ISideBarSubwidget Members

        [ShowInInspector] public bool IsActive => (Widget != null) && Equals(Widget.ActiveSubwidget, this);

        void IActivable.Activate()
        {
            Activate();
        }

        void IActivable.Deactivate()
        {
            Deactivate();
        }

        void IActivable.Toggle()
        {
            Toggle();
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));
        private static readonly ProfilerMarker _PRF_Deactivate = new ProfilerMarker(_PRF_PFX + nameof(Deactivate));
        protected static readonly ProfilerMarker _PRF_OnActivate = new ProfilerMarker(_PRF_PFX + nameof(OnActivate));

        protected static readonly ProfilerMarker _PRF_OnDeactivate =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivate));

        #endregion
    }
}
