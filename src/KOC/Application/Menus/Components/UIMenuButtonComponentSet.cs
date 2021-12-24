using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Doozy.Engine.UI;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Menus.Components
{
    [Serializable, DoNotReorderFields]
    public struct UIMenuButtonComponentSet : IComponentSet
    {
        #region Fields and Autoproperties

        public GameObject gameObject;
        public RectTransform rect;

        public ButtonWrapperComponent buttonWrapper;
        public ButtonImageComponent buttonImage;
        public ButtonTextComponent buttonText;

        #endregion

        #region IComponentSet Members

        public GameObject GameObject => gameObject;

        public void Configure(GameObject parent, string baseName)
        {
            using (_PRF_Configure.Auto())
            {
                var targetName = ZString.Format("UI Menu Button - {0}", baseName);

                parent.GetOrCreateChild(ref gameObject, targetName, true);
                gameObject.GetOrCreateComponent(ref rect);

                gameObject.GetOrCreateChild(
                    ref buttonWrapper.gameObject,
                    ZString.Format("UI Menu Button - Wrapper - {0}", baseName),
                    true
                );
                buttonWrapper.gameObject.GetOrCreateComponent(ref buttonWrapper.rect);
                buttonWrapper.gameObject.GetOrCreateComponent(ref buttonWrapper.button);
                buttonWrapper.gameObject.GetOrCreateComponent(ref buttonWrapper.composite);
                buttonWrapper.gameObject.GetOrCreateComponent(ref buttonWrapper.doozyButton);

                buttonWrapper.gameObject.GetOrCreateChild(
                    ref buttonImage.gameObject,
                    ZString.Format("UI Menu Button - Image - {0}", baseName),
                    true
                );
                buttonImage.gameObject.GetOrCreateComponent(ref buttonImage.rect);
                buttonImage.gameObject.GetOrCreateComponent(ref buttonImage.image);

                buttonWrapper.gameObject.GetOrCreateChild(
                    ref buttonText.gameObject,
                    ZString.Format("UI Menu Button - TMP - {0}", baseName),
                    true
                );

                buttonText.gameObject.GetOrCreateComponent(ref buttonText.rect);
                buttonText.gameObject.GetOrCreateComponent(ref buttonText.textMesh);
            }
        }

        #endregion

        #region Nested type: ButtonImageComponent

        [DoNotReorderFields]
        public struct ButtonImageComponent
        {
            #region Fields and Autoproperties

            public GameObject gameObject;
            public RectTransform rect;
            public Image image;

            #endregion
        }

        #endregion

        #region Nested type: ButtonTextComponent

        [DoNotReorderFields]
        public struct ButtonTextComponent
        {
            #region Fields and Autoproperties

            public GameObject gameObject;
            public RectTransform rect;
            public TextMeshProUGUI textMesh;

            #endregion
        }

        #endregion

        #region Nested type: ButtonWrapperComponent

        [DoNotReorderFields]
        public struct ButtonWrapperComponent
        {
            #region Fields and Autoproperties

            public GameObject gameObject;
            public RectTransform rect;
            public Button button;
            public CompositeGraphic composite;
            public UIButton doozyButton;

            #endregion
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(UITemplateComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
