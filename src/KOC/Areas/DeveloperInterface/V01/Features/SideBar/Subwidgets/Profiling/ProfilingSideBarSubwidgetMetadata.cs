using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core;
using Appalachia.UI.Controls.Sets.Layout.Foldout;
using Appalachia.UI.Core.Components.Data;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Profiling
{
    public class
        ProfilingSideBarSubwidgetMetadata : SideBarSubwidgetMetadata<ProfilingSideBarSubwidget,
            ProfilingSideBarSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField] public FoldoutComponentSetData fps;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField] public FoldoutComponentSetData memory;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField] public FoldoutComponentSetData audio;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public VerticalLayoutGroupData verticalLayoutGroup;

        #endregion


        /// <inheritdoc />
        protected override async AppaTask Initialize(
            Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }
    }
}
