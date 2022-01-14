using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Behaviours;
using Appalachia.Prototype.KOC.Components.Styling.Fonts;
using Appalachia.Prototype.KOC.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.Text
{
    public abstract class DynamicTextMesh<T> : AppalachiaApplicationBehaviour<T>
        where T : DynamicTextMesh<T>
    {
        #region Constants and Static Readonly

        private const string INITKEY_LABEL_LAYOUT = nameof(INITKEY_LABEL_LAYOUT);
        private const string INITKEY_LAYOUT = nameof(INITKEY_LAYOUT);
        private const string INITKEY_TMP_CREATE = nameof(INITKEY_TMP_CREATE);

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup("Settings")]
        [OnValueChanged(nameof(InitializeSynchronous))]
        public bool updateName;

        [FoldoutGroup("Layout")]
        [OnValueChanged(nameof(InitializeSynchronous))]
        public bool createLayoutElement;

        [FoldoutGroup("Layout")]
        [OnValueChanged(nameof(InitializeSynchronous))]
        public int minHeight = 30;

        [SerializeField]
        [FoldoutGroup("Text")]
        [OnValueChanged(nameof(InitializeSynchronous))]
        public FontStyleOverride font;

        [FoldoutGroup("Label")]
        [OnValueChanged(nameof(InitializeSynchronous))]
        public bool createLabel;

        [SerializeField]
        [FoldoutGroup("Label")]
        [OnValueChanged(nameof(InitializeSynchronous))]
        public FontStyleOverride labelFont;

        [FoldoutGroup("Label")]
        [OnValueChanged(nameof(InitializeSynchronous))]
        [PropertyRange(0f, .5f)]
        public float labelWidth = .25f;

        [FoldoutGroup("Label")]
        [OnValueChanged(nameof(InitializeSynchronous))]
        [PropertyRange(0f, .1f)]
        public float labelPadding = .05f;

        [FoldoutGroup("References")]
        [FormerlySerializedAs("textLabel")]
        [ReadOnly]
        public TextMeshProUGUI textMeshLabel;

        [FoldoutGroup("References")]
        [FormerlySerializedAs("textMesh")]
        [ReadOnly]
        public TextMeshProUGUI textMeshValue;

        [ReadOnly]
        [FoldoutGroup("References")]
        public LayoutElement layoutElement;

        [SerializeField, HideInInspector]
        protected string _lastText;

        #endregion

        protected abstract string GetUpdatedText();

        protected virtual string GetLabel()
        {
            return "Label";
        }

        protected virtual string GetName()
        {
            return name;
        }

        protected virtual void OnUpdateText()
        {
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                enabled = true;

                if (updateName)
                {
                    name = GetName();
                }
            }
#if UNITY_EDITOR

            initializer.Do(
                this,
                nameof(font),
                font == null,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        font = FontStyleOverride.LoadOrCreateNew(nameof(T));
                    }
                }
            );

            initializer.Do(
                this,
                nameof(labelFont),
                labelFont == null,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        labelFont = FontStyleOverride.LoadOrCreateNew(nameof(T) + "_Label");
                    }
                }
            );
#endif
            using (_PRF_Initialize.Auto())
            {
                font.StyleChanged -= FontOnStyleChanged;
                font.StyleChanged += FontOnStyleChanged;

                labelFont.StyleChanged -= LabelFontOnStyleChanged;
                labelFont.StyleChanged += LabelFontOnStyleChanged;
            }

            initializer.Do(
                this,
                nameof(INITKEY_TMP_CREATE),
                (textMeshValue == null) || (textMeshLabel == null),
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        gameObject.GetOrCreateComponentInChild(ref textMeshValue, "Text");
                        gameObject.GetOrCreateComponentInChild(ref textMeshLabel, "Label");

                        textMeshValue.rectTransform.Reset(RectResetOptions.All);
                        textMeshLabel.rectTransform.Reset(RectResetOptions.All);
                    }
                }
            );

            using (_PRF_Initialize.Auto())
            {
                if (textMeshValue.text.IsNullOrWhiteSpace())
                {
                    textMeshValue.text = _lastText;
                }

                if (createLabel)
                {
                    textMeshLabel.enabled = true;
                    var labelAnchorMax = textMeshLabel.rectTransform.anchorMax;
                    var valueAnchorMin = textMeshValue.rectTransform.anchorMin;

                    labelAnchorMax.x = labelWidth;
                    valueAnchorMin.x = labelWidth + labelPadding;

                    textMeshLabel.rectTransform.anchorMax = labelAnchorMax;
                    textMeshValue.rectTransform.anchorMin = valueAnchorMin;
                }
            }

            initializer.Do(
                this,
                nameof(INITKEY_LAYOUT),
                layoutElement == null,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        gameObject.GetOrCreateComponent(ref layoutElement);
                    }
                }
            );

            using (_PRF_Initialize.Auto())
            {
                UpdateText();
            }
        }

        [Button]
        protected void UpdateText()
        {
            using (_PRF_UpdateText.Auto())
            {
                if (createLayoutElement)
                {
                    if (!layoutElement.enabled)
                    {
                        layoutElement.enabled = true;
                    }

                    layoutElement.minHeight = minHeight;
                }
                else if (layoutElement.enabled)
                {
                    layoutElement.enabled = false;
                }

                font.ToApplicable.Apply(textMeshValue);

                textMeshValue.text = GetUpdatedText();

                if (createLabel)
                {
                    labelFont.ToApplicable.Apply(textMeshLabel);

                    var correctLabel = GetLabel();

                    if (textMeshLabel.text != correctLabel)
                    {
                        textMeshLabel.text = correctLabel;
                    }
                }
                else if (textMeshLabel.enabled)
                {
                    textMeshLabel.enabled = false;
                }

                OnUpdateText();
            }
        }

        private void FontOnStyleChanged(IFontStyle style)
        {
            UpdateText();
        }

        private void LabelFontOnStyleChanged(IFontStyle style)
        {
            UpdateText();
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateText =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateText));

        #endregion
    }
}
