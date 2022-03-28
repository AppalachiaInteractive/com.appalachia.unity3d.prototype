using Appalachia.Core.Objects.Models.Selectable;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Utility.Enums;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core
{
    public abstract partial class ActivityBarSubwidget<TSubwidget, TSubwidgetMetadata> : IActivable
    {
        protected abstract void OnActivate();
        protected abstract void OnDeactivate();

        [ButtonGroup(nameof(Activate))]
        protected void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                Widget.SetActiveSubwidget(this);
                button.selectionIndicator.State = button.selectionIndicator.State.SetFlag(SelectableState.Selected);

                OnActivate();
            }
        }

        [ButtonGroup(nameof(Activate))]
        protected void Deactivate()
        {
            using (_PRF_Deactivate.Auto())
            {
                button.selectionIndicator.State = button.selectionIndicator.State.UnsetFlag(SelectableState.Selected);

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

        #region IActivable Members

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

        protected static readonly ProfilerMarker
            _PRF_OnDeactivate = new ProfilerMarker(_PRF_PFX + nameof(OnDeactivate));

        #endregion
    }
}
