using System;
using Appalachia.Core.ArrayPooling;
using Appalachia.Core.Collections.NonSerialized;
using Appalachia.Core.ObjectPooling;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.UI.Controls.Components.Buttons;
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
        #region Static Fields and Autoproperties

        private static ColorList _colorList;

        #endregion

        #region Fields and Autoproperties

        public NonSerializedList<RectTransformVisualizationDataElement> elements;
        public Vector3[] corners;

        #endregion

        public override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                corners = ArrayPool<Vector3>.Shared.Rent(4);
                elements ??= new NonSerializedList<RectTransformVisualizationDataElement>();
                elements.ClearFast();
                _colorList ??= new ColorList();
            }
        }

        public override void Reset()
        {
            using (_PRF_Reset.Auto())
            {
                if (corners != null)
                {
                    ArrayPool<Vector3>.Shared.Return(corners);
                }

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
                rectTransform.GetWorldCorners(corners);

                _colorList.ClearFast();

                var uiBehaviours = rectTransform.GetComponents<Behaviour>();

                for (var behaviourIndex = 0; behaviourIndex < uiBehaviours.Length; behaviourIndex++)
                {
                    var behaviour = uiBehaviours[behaviourIndex];

                    if (behaviour is Canvas c)
                    {
                        AddColor(metadata.canvas);

                        if (c.renderMode == RenderMode.WorldSpace)
                        {
                            AddColor(metadata.worldSpace);
                        }
                        else
                        {
                            AddColor(metadata.screenSpace);
                        }
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
                }

                if (!rectTransform.gameObject.activeInHierarchy)
                {
                    for (var colorIndex = 0; colorIndex < _colorList.Count; colorIndex++)
                    {
                        var color = _colorList[colorIndex];

                        _colorList[colorIndex] = color * metadata.inactive;
                    }
                }

                elements.ClearFast();

                for (var elementIndex = 0; elementIndex < _colorList.Count; elementIndex++)
                {
                    var element = RectTransformVisualizationDataElement.Get();

                    element.CopyCorners(corners, metadata.z);
                    element.color = _colorList[elementIndex];

                    element.Grow(elementIndex + 1);

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

        #region Profiling

        private static readonly ProfilerMarker _PRF_AddColor =
            new ProfilerMarker(_PRF_PFX + nameof(AddColor));

        private static readonly ProfilerMarker _PRF_Create = new ProfilerMarker(_PRF_PFX + nameof(Create));

        private static readonly ProfilerMarker _PRF_Draw = new ProfilerMarker(_PRF_PFX + nameof(Draw));

        #endregion
    }
}
