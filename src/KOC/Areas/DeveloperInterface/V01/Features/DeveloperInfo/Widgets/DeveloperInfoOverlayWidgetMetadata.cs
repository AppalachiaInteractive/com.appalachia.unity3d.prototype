using System.Collections.Generic;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Components;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Models;
using Appalachia.UI.Core.Components.Data;
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

        [OnValueChanged(nameof(OnChanged))]
        public ImageData headerImage;

        [OnValueChanged(nameof(OnChanged))]
        public LayoutElementData headerLayout;

        [OnValueChanged(nameof(OnChanged))]
        public List<DeveloperInfoType> types;

        [FormerlySerializedAs("settings")]
        [OnValueChanged(nameof(OnChanged))]
        public DeveloperInfoTextMeshData textSettings;

        [FormerlySerializedAs("layoutData")]
        [OnValueChanged(nameof(OnChanged))]
        public VerticalLayoutGroupData verticalLayoutGroupData;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
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

        protected override void SubscribeResponsiveComponents(DeveloperInfoOverlayWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                headerImage.Changed.Event += OnChanged;
                headerLayout.Changed.Event += OnChanged;
                textSettings.Changed.Event += OnChanged;
                verticalLayoutGroupData.Changed.Event += OnChanged;
            }
        }

        protected override void UpdateFunctionality(DeveloperInfoOverlayWidget widget)
        {
            using (_PRF_Apply.Auto())
            {
                base.UpdateFunctionality(widget);

                for (var i = types.Count; i < widget.textMeshes.Count; i++)
                {
                    widget.textMeshes[i].DestroySafely();
                }

                for (var i = widget.textMeshes.Count; i < types.Count; i++)
                {
                    var type = types[i];

                    GameObject childGameObject = null;
                    DeveloperInfoTextMesh textMesh = null;

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

                    textMesh.data = textSettings;
                    textMesh.currentCalculation = type;

                    DeveloperInfoTextMeshData.UpdateComponent(ref textSettings, textMesh, this);
                }

                VerticalLayoutGroupData.UpdateComponent(
                    ref verticalLayoutGroupData,
                    widget.verticalLayoutGroup,
                    this
                );
                ImageData.UpdateComponent(ref headerImage, widget.headerImage, this);
                LayoutElementData.UpdateComponent(ref headerLayout, widget.headerLayout, this);

                if (widget.headerImage.transform.parent != widget.canvas.RectTransform)
                {
                    widget.headerImage.transform.SetParent(widget.canvas.RectTransform);
                }

                widget.headerImage.gameObject.GetOrAddComponent(ref widget.headerLayout);

                widget.headerImage.transform.SetSiblingIndex(1);
            }
        }
    }
}
