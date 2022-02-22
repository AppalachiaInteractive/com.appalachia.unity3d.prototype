using System.Collections.Generic;
using System.Linq;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Components;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInfoOverlayWidget : DeveloperInterfaceManager_V01.Widget<
        DeveloperInfoOverlayWidget, DeveloperInfoOverlayWidgetMetadata, DeveloperInfoFeature,
        DeveloperInfoFeatureMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup("Components")]
        [BoxGroup("Components/Header")]
        public RectTransform headerRect;

        [FoldoutGroup("Components")]
        [BoxGroup("Components/Header")]
        public Image headerImage;

        [BoxGroup("Components/Header")]
        public LayoutElement headerLayout;

        [BoxGroup("Components/Text")]
        public List<DeveloperInfoTextMesh> textMeshes;

        [FormerlySerializedAs("layoutGroup")]
        [BoxGroup("Components/Text")]
        public VerticalLayoutGroup verticalLayoutGroup;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                textMeshes ??= new List<DeveloperInfoTextMesh>();

                textMeshes = GetComponentsInChildren<DeveloperInfoTextMesh>().ToList();

                canvas.GameObject.GetOrAddComponent(ref verticalLayoutGroup);

                canvas.GameObject.GetOrAddComponentInChild(ref headerImage, "Header");

                headerImage.GetOrAddComponent(ref headerRect);
            }
        }
    }
}
