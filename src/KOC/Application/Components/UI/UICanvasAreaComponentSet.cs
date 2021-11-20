using System;
using Appalachia.Prototype.KOC.Application.Screens.Fading;
using Appalachia.Utility.Extensions;
using Doozy.Engine.Nody;
using Doozy.Engine.UI;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Components.UI
{
    [Serializable]
    public struct UICanvasAreaComponentSet
    {
        #region Fields and Autoproperties

        public Canvas canvas;
        public CanvasFadeManager canvasFadeManager;
        public CanvasGroup canvasGroup;
        public CanvasScaler canvasScaler;

        public GameObject gameObject;
        public GraphController graphController;
        public GraphicRaycaster graphicRaycaster;
        public RectTransform rect;
        public UICanvas uiCanvas;

        #endregion

        public void Configure(GameObject parent, string name)
        {
            using (_PRF_Configure.Auto())
            {
                var targetName = $"Canvas - {name}";

                parent.FindOrCreateChild(ref gameObject, targetName);

                gameObject.GetOrCreateComponent(ref rect);
                gameObject.GetOrCreateComponent(ref canvasFadeManager);
                gameObject.GetOrCreateComponent(ref canvasGroup);
                gameObject.GetOrCreateComponent(ref canvas);
                gameObject.GetOrCreateComponent(ref canvasScaler);
                gameObject.GetOrCreateComponent(ref graphicRaycaster);
                gameObject.GetOrCreateComponent(ref uiCanvas);
                gameObject.GetOrCreateComponent(ref graphController);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UICanvasAreaComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
