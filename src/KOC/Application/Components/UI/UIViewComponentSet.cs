using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Screens.Fading;
using Appalachia.Utility.Extensions;
using Doozy.Engine.UI;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Components.UI
{
    [Serializable, SmartLabelChildren]
    public struct UIViewComponentSet : IComponentSet
    {
        #region Fields and Autoproperties

        public GameObject GameObject => gameObject;
        
        public Canvas canvas;
        public CanvasFadeManager canvasFadeManager;
        public CanvasGroup canvasGroup;

        public GameObject gameObject;
        public GraphicRaycaster graphicRaycaster;
        public RectTransform rect;
        public UIView uiView;

        #endregion

        public void Configure(GameObject parent, string baseName)
        {
            using (_PRF_ConfigureView.Auto())
            {
                var targetName = $"View - {baseName}";

                parent.CreateOrGetChild(ref gameObject, targetName, true);

                gameObject.CreateOrGetComponent(ref rect);
                gameObject.CreateOrGetComponent(ref canvasFadeManager);
                gameObject.CreateOrGetComponent(ref canvasGroup);
                gameObject.CreateOrGetComponent(ref canvas);
                gameObject.CreateOrGetComponent(ref graphicRaycaster);
                gameObject.CreateOrGetComponent(ref uiView);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIViewComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_ConfigureView =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
