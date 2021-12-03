using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Screens.Fading;
using Appalachia.Utility.Extensions;
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

        public GameObject GameObject => gameObject;
        
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

                parent.CreateOrGetChild(ref gameObject, targetName, true);

                gameObject.CreateOrGetComponent(ref rect);
                gameObject.CreateOrGetComponent(ref canvasFadeManager);
                gameObject.CreateOrGetComponent(ref canvasGroup);
                gameObject.CreateOrGetComponent(ref canvas);
                gameObject.CreateOrGetComponent(ref canvasScaler);
                gameObject.CreateOrGetComponent(ref graphicRaycaster);
                gameObject.CreateOrGetComponent(ref uiCanvas);
                gameObject.CreateOrGetComponent(ref graphController);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UICanvasAreaComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
