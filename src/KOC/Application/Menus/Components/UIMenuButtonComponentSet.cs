using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Utility.Extensions;
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

        public GameObject GameObject => gameObject;
        
        public GameObject gameObject;
        public RectTransform rect;

        public ButtonWrapperComponent buttonWrapper;
        public ButtonImageComponent buttonImage;
        public ButtonTextComponent buttonText;

        #endregion

        #region IComponentSet Members

        public void Configure(GameObject parent, string baseName)
        {
            using (_PRF_Configure.Auto())
            {
                var targetName = $"UI Menu Button - {baseName}";

                parent.CreateOrGetChild(ref gameObject, targetName, true);
                gameObject.CreateOrGetComponent(ref rect);

                gameObject.CreateOrGetChild(
                    ref buttonWrapper.gameObject,
                    $"UI Menu Button - Wrapper - {baseName}", true
                );
                buttonWrapper.gameObject.CreateOrGetComponent(ref buttonWrapper.rect);
                buttonWrapper.gameObject.CreateOrGetComponent(ref buttonWrapper.button);
                buttonWrapper.gameObject.CreateOrGetComponent(ref buttonWrapper.composite);
                buttonWrapper.gameObject.CreateOrGetComponent(ref buttonWrapper.doozyButton);

                buttonWrapper.gameObject.CreateOrGetChild(
                    ref buttonImage.gameObject,
                    $"UI Menu Button - Image - {baseName}", true
                );
                buttonImage.gameObject.CreateOrGetComponent(ref buttonImage.rect);
                buttonImage.gameObject.CreateOrGetComponent(ref buttonImage.image);

                buttonWrapper.gameObject.CreateOrGetChild(
                    ref buttonText.gameObject,
                    $"UI Menu Button - TMP - {baseName}", true
                );

                buttonText.gameObject.CreateOrGetComponent(ref buttonText.rect);
                buttonText.gameObject.CreateOrGetComponent(ref buttonText.textMesh);
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