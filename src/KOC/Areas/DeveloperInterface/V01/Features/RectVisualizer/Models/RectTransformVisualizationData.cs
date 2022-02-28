using System;
using Appalachia.Core.Collections.NonSerialized;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.UI.Controls.Components.Buttons;
using Appalachia.UI.Controls.Components.Layout;
using Appalachia.UI.Controls.Extensions;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Pooling.Objects;
using Drawing;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Models
{
#pragma warning disable CS0612
    [Serializable]
    [HideReferenceObjectPicker]
    public class RectTransformVisualizationData : SelfPoolingObject<RectTransformVisualizationData>
#pragma warning restore CS0612
    {
        #region Constants and Static Readonly

        private const float GROWTH_STEP = 4F;
        private const float LINE_THICKNESS = 2F;

        #endregion

        #region Static Fields and Autoproperties

        private static ColorList _colorList;

        #endregion

        #region Fields and Autoproperties

        public NonSerializedList<RectTransformVisualizationDataElement> elements;
        public Rect rect;

        #endregion

        /// <inheritdoc />
        public override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                elements ??= new NonSerializedList<RectTransformVisualizationDataElement>();
                elements.ClearFast();
                _colorList ??= new ColorList();
            }
        }

        /// <inheritdoc />
        public override void Reset()
        {
            using (_PRF_Reset.Auto())
            {
                for (var i = 0; i < elements.Count; i++)
                {
                    elements[i].Return();
                }

                elements.ClearFast();
            }
        }

        public void Create(RectTransform rectTransform, RectVisualizerFeatureMetadata metadata)
        {
            using (_PRF_Create.Auto())
            {
                var isSelected = false;
#if UNITY_EDITOR
                isSelected = rectTransform.gameObject.IsSelected();
#endif
                rect = rectTransform.GetScreenSpaceRect();
                
                _colorList.ClearFast();

                var uiBehaviours = rectTransform.GetComponents<Behaviour>();

                ApplyColors(metadata, uiBehaviours);

                if (!rectTransform.gameObject.activeInHierarchy)
                {
                    for (var colorIndex = 0; colorIndex < _colorList.Count; colorIndex++)
                    {
                        var color = _colorList[colorIndex];

                        _colorList[colorIndex] = color * metadata.inactive;
                    }
                }

                for (var colorIndex = 0; colorIndex < _colorList.Count; colorIndex++)
                {
                    var color = _colorList[colorIndex];
                    color.a *= metadata.globalAlpha;

                    _colorList[colorIndex] = color;
                }

                var drawLocatorLine = rect.IsFullyOutsideScreen();

                elements.ClearFast();

                for (var elementIndex = 0; elementIndex < _colorList.Count; elementIndex++)
                {
                    var element = RectTransformVisualizationDataElement.Get();

                    element.CopyRect(rect, metadata.z, LINE_THICKNESS, isSelected);
                    element.color = _colorList[elementIndex];
                    element.drawLocatorLine = drawLocatorLine;

                    if ((Math.Abs(element.color.r - 1f) < float.Epsilon) &&
                        (Math.Abs(element.color.g - 1f) < float.Epsilon) &&
                        (Math.Abs(element.color.b - 1f) < float.Epsilon))
                    {
                        element.thickness = 1f;
                    }

                    element.Shrink(elementIndex * GROWTH_STEP);

                    elements.Add(element);
                }

                _colorList.ClearFast();
            }
        }

        public void Draw(CommandBuilder drawCommandBuilder)
        {
            using (_PRF_Draw.Auto())
            {
                for (var elementIndex = 0; elementIndex < elements.Count; elementIndex++)
                {
                    var element = elements[elementIndex];

                    element.DrawAsCube(drawCommandBuilder);
                }
            }
        }

        private void AddColor(OverridableColor color)
        {
            using (_PRF_AddColor.Auto())
            {
                if (color.Overriding)
                {
                    _colorList.Add(color.Value);
                }
            }
        }

        private void ApplyColors(RectVisualizerFeatureMetadata metadata, Behaviour[] uiBehaviours)
        {
            AddColor(metadata.rectTransform);

            for (var behaviourIndex = 0; behaviourIndex < uiBehaviours.Length; behaviourIndex++)
            {
                var behaviour = uiBehaviours[behaviourIndex];

                if (behaviour is Canvas c)
                {
                    AddColor(metadata.canvas);

                    AddColor(
                        c.renderMode == RenderMode.WorldSpace ? metadata.worldSpace : metadata.screenSpace
                    );
                }
                else if (behaviour is Mask)
                {
                    AddColor(metadata.mask);
                }
                else if (behaviour is UIBehaviour uiBehaviour)
                {
                    if (uiBehaviour is Selectable selectable)
                    {
                        AddColor(metadata.selectable);

                        if (selectable is Button or AppaButton)
                        {
                            AddColor(metadata.button);
                        }
                        else if (selectable is Dropdown or TMP_Dropdown)
                        {
                            AddColor(metadata.dropdown);
                        }
                        else if (selectable is Toggle)
                        {
                            AddColor(metadata.toggle);
                        }
                        else if (selectable is InputField or TMP_InputField)
                        {
                            AddColor(metadata.inputField);
                        }
                        else if (selectable is Slider)
                        {
                            AddColor(metadata.slider);
                        }
                        else if (behaviour is Scrollbar)
                        {
                            AddColor(metadata.scrollbar);
                        }
                    }
                    else if (uiBehaviour is Graphic graphic)
                    {
                        AddColor(metadata.graphic);

                        if (graphic.raycastTarget)
                        {
                            AddColor(metadata.raycastTarget);
                        }

                        if (graphic is Image i)
                        {
                            AddColor(metadata.image);

                            if (i.maskable)
                            {
                                AddColor(metadata.maskable);
                            }
                        }
                        else if (graphic is Text or TMP_Text)
                        {
                            AddColor(metadata.text);
                        }
                    }
                    else if (uiBehaviour is CanvasScaler)
                    {
                        AddColor(metadata.canvasScaler);
                    }
                    else if (uiBehaviour is AspectRatioFitter)
                    {
                        AddColor(metadata.aspectRatioFitter);
                    }
                    else if (uiBehaviour is ContentSizeFitter)
                    {
                        AddColor(metadata.contentSizeFitter);
                    }
                    else if (uiBehaviour is LayoutElement)
                    {
                        AddColor(metadata.layoutElement);
                    }
                    else if (uiBehaviour is LayoutGroup)
                    {
                        AddColor(metadata.layoutGroup);
                    }
                    else if (uiBehaviour is ScrollRect)
                    {
                        AddColor(metadata.scrollRect);
                    }
                }
                else if (behaviour is AppaCanvasScaler)
                {
                    AddColor(metadata.appaCanvasScaler);
                }
                else if (behaviour is IApplicationFeature)
                {
                    AddColor(metadata.feature);
                }
                else if (behaviour is IApplicationWidget)
                {
                    AddColor(metadata.widget);
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_AddColor =
            new ProfilerMarker(_PRF_PFX + nameof(AddColor));

        private static readonly ProfilerMarker _PRF_Create = new ProfilerMarker(_PRF_PFX + nameof(Create));

        private static readonly ProfilerMarker _PRF_Draw = new ProfilerMarker(_PRF_PFX + nameof(Draw));

        #endregion
    }
}
