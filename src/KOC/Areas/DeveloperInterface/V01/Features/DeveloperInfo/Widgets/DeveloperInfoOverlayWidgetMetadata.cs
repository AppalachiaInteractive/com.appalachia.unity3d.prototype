using System.Collections.Generic;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Components;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Models;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.Functionality.Images.Components;
using Appalachia.UI.Functionality.Layout.Components;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Widgets
{
    public sealed class DeveloperInfoOverlayWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<
        DeveloperInfoOverlayWidget, DeveloperInfoOverlayWidgetMetadata, DeveloperInfoFeature,
        DeveloperInfoFeatureMetadata>
    {
        #region Fields and Autoproperties

        [PropertyRange(0f, 1f)]
        [PropertyOrder(1000)]
        [OnValueChanged(nameof(OnChanged))]
        public float alpha;

        [FoldoutGroup("Header")]
        [OnValueChanged(nameof(OnChanged))]
        public RectTransformConfig headerRect;

        [FoldoutGroup("Header")]
        [OnValueChanged(nameof(OnChanged))]
        public ImageConfig headerImage;

        [FoldoutGroup("Header")]
        [OnValueChanged(nameof(OnChanged))]
        public LayoutElementConfig headerLayout;

        [OnValueChanged(nameof(OnChanged))]
        public List<DeveloperInfoType> types;

        [FormerlySerializedAs("textSettings")]
        [FormerlySerializedAs("settings")]
        [OnValueChanged(nameof(OnChanged))]
        public DeveloperInfoTextMeshControlConfig developerInfoTextMesh;

        [FormerlySerializedAs("verticalLayoutGroupData")]
        [FormerlySerializedAs("layoutData")]
        [OnValueChanged(nameof(OnChanged))]
        public VerticalLayoutGroupConfig verticalLayoutGroup;

        #endregion

        public override float GetCanvasGroupVisibleAlpha()
        {
            using (_PRF_GetCanvasGroupVisibleAlpha.Auto())
            {
                return alpha;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(alpha), () => alpha = .5f);

                initializer.Do(
                    this,
                    nameof(types),
                    (types == null) || (types.Count == 0),
                    () =>
                    {
                        types = new List<DeveloperInfoType>
                        {
                            DeveloperInfoType.ActiveSceneNameAndCount,
                            DeveloperInfoType.ApplicationBuildGuid,
                            DeveloperInfoType.ApplicationVersion,
                            DeveloperInfoType.ApplicationUnityVersion,
                            DeveloperInfoType.ApplicationPlatform,
                            DeveloperInfoType.ApplicationSystemLanguage,
                            DeveloperInfoType.ScreenResolution,
                            DeveloperInfoType.WindowSize,
                            DeveloperInfoType.SystemMemory,
                            DeveloperInfoType.ProcessorType,
                            DeveloperInfoType.GraphicsMemorySize,
                            DeveloperInfoType.GraphicsDeviceName,
                            DeveloperInfoType.GraphicsDeviceVersion,
                            DeveloperInfoType.OperatingSystem
                        };
                    }
                );
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(DeveloperInfoOverlayWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                headerRect.Changed.Event += OnChanged;
                headerImage.Changed.Event += OnChanged;
                headerLayout.Changed.Event += OnChanged;
                developerInfoTextMesh.Changed.Event += OnChanged;
                verticalLayoutGroup.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(DeveloperInfoOverlayWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                for (var i = types.Count; i < widget.textMeshes.Count; i++)
                {
                    widget.textMeshes[i].DestroySafely();
                }

                for (var i = widget.textMeshes.Count; i < types.Count; i++)
                {
                    var type = types[i];

                    GameObject childGameObject = null;
                    DeveloperInfoTextMeshControl textMesh = null;

                    widget.canvas.GameObject.GetOrAddChild(ref childGameObject, type.ToString(), true);
                    childGameObject.GetOrAddComponent(ref textMesh);

                    widget.textMeshes.Add(textMesh);
                }

                for (var i = 0; i < widget.textMeshes.Count; i++)
                {
                    var type = types[i];
                    var textMesh = widget.textMeshes[i];

                    if (textMesh.transform.parent != widget.canvas.RectTransform)
                    {
                        textMesh.transform.SetParent(widget.canvas.RectTransform);
                    }

                    textMesh.updateTiming = developerInfoTextMesh.updateTiming;
                    textMesh.createLabel = developerInfoTextMesh.createLabel;

                    textMesh.currentCalculation = type;

                    var maxPreferredHeight = Mathf.Max(
                        textMesh.textMeshValue.textMeshProUGUI.preferredHeight,
                        textMesh.textMeshLabel.textMeshProUGUI.preferredHeight
                    );

                    textMesh.layoutElement.layoutElement.minHeight = maxPreferredHeight;
                    textMesh.layoutElement.layoutElement.preferredHeight = maxPreferredHeight;

                    developerInfoTextMesh.Apply(textMesh);
                }

                if (widget.headerImage.transform.parent != widget.canvas.RectTransform)
                {
                    widget.headerImage.transform.SetParent(widget.canvas.RectTransform);
                }

                widget.headerImage.gameObject.GetOrAddComponent(ref widget.headerLayout);

                var roundedBackgroundIndex = widget.roundedBackground.GameObject.transform.GetSiblingIndex();
                widget.headerImage.transform.SetSiblingIndex(roundedBackgroundIndex + 1);

                RectTransformConfig.RefreshAndApply(ref headerRect, this, widget.headerRect);

                ImageConfig.RefreshAndApply(ref headerImage, this, widget.headerImage);

                LayoutElementConfig.RefreshAndApply(ref headerLayout, this, widget.headerLayout);

                VerticalLayoutGroupConfig.RefreshAndApply(ref verticalLayoutGroup, this, widget.verticalLayoutGroup);

                canvas.Value.Canvas.CanvasGroup.Value.alpha.Overriding = false;
                widget.canvas.Canvas.CanvasGroup.alpha = alpha;
            }
        }
    }
}
