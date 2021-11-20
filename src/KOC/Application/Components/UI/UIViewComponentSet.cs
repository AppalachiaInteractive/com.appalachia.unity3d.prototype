using System;
using Appalachia.Prototype.KOC.Application.Screens.Fading;
using Appalachia.Utility.Extensions;
using Doozy.Engine.UI;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Components.UI
{
    [Serializable]
    public struct UIViewComponentSet
    {
        #region Fields and Autoproperties

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

                parent.FindOrCreateChild(ref gameObject, targetName);

                gameObject.GetOrCreateComponent(ref rect);
                gameObject.GetOrCreateComponent(ref canvasFadeManager);
                gameObject.GetOrCreateComponent(ref canvasGroup);
                gameObject.GetOrCreateComponent(ref canvas);
                gameObject.GetOrCreateComponent(ref graphicRaycaster);
                gameObject.GetOrCreateComponent(ref uiView);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIViewComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_ConfigureView =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
