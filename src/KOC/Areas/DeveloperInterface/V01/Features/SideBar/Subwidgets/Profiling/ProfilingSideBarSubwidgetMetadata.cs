using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core;
using Appalachia.UI.Controls.Sets2.Layout.Foldout;
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

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public FoldoutComponentSetData fps2;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public FoldoutComponentSetData memory2;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public FoldoutComponentSetData audio2;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public FoldoutComponentSetData fps;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public FoldoutComponentSetData memory;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public FoldoutComponentSetData audio;

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
                fps = fps2;
                memory = memory2;
                audio = audio2;
            }
        }
    }
}
