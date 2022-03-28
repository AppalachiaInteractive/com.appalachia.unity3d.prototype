using System.Collections.Generic;
using System.Linq;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Components;
using Appalachia.UI.Functionality.Images.Controls.Default;
using Appalachia.UI.Functionality.Layout.Groups.Vertical;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInfoOverlayWidget : DeveloperInterfaceManager_V01.Widget<DeveloperInfoOverlayWidget,
        DeveloperInfoOverlayWidgetMetadata, DeveloperInfoFeature, DeveloperInfoFeatureMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup("Components")]
        [SerializeField]
        public ImageControl headerImage;

        [FoldoutGroup("Components")]
        [SerializeField]
        public VerticalLayoutGroupComponentGroup verticalLayoutGroup;

        [FoldoutGroup("Components")]
        [SerializeField]
        public List<DeveloperInfoTextMeshControl> textMeshes;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                VerticalLayoutGroupComponentGroup.Refresh(
                    ref verticalLayoutGroup,
                    canvas.ChildContainer.gameObject,
                    nameof(verticalLayoutGroup)
                );

                ImageControl.Refresh(ref headerImage, verticalLayoutGroup.gameObject, nameof(headerImage));

                textMeshes ??= new List<DeveloperInfoTextMeshControl>();
                textMeshes = GetComponentsInChildren<DeveloperInfoTextMeshControl>().ToList();

                for (var index = 0; index < textMeshes.Count; index++)
                {
                    var textMesh = textMeshes[index];

                    textMesh.gameObject.SetParentTo(verticalLayoutGroup.gameObject);
                }
            }
        }
    }
}
