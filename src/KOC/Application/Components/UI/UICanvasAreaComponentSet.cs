using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Components.Fading;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Doozy.Engine.Nody;
using Doozy.Engine.UI;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Components.UI
{
    [Serializable, SmartLabelChildren]
    public struct UICanvasAreaComponentSet : IComponentSet
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

        #region IComponentSet Members

        public GameObject GameObject => gameObject;

        public void Configure(GameObject parent, string name)
        {
            using (_PRF_Configure.Auto())
            {
                var targetName = ZString.Format("Canvas - {0}", name);

                parent.GetOrCreateChild(ref gameObject, targetName, true);

                gameObject.GetOrCreateComponent(ref rect);
                gameObject.GetOrCreateComponent(ref canvasFadeManager);
                gameObject.GetOrCreateComponent(ref canvasGroup);
                gameObject.GetOrCreateComponent(ref canvas);
                gameObject.GetOrCreateComponent(ref canvasScaler);
                gameObject.GetOrCreateComponent(ref graphicRaycaster);
                gameObject.GetOrCreateComponent(ref uiCanvas);
                gameObject.GetOrCreateComponent(ref graphController);

                uiCanvas.DontDestroyCanvasOnLoad = false;
                graphController.DontDestroyControllerOnLoad = false;
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(UICanvasAreaComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
