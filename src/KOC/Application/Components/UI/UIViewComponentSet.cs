using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Components.Fading;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
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

        public Canvas canvas;
        public CanvasFadeManager canvasFadeManager;
        public CanvasGroup canvasGroup;

        public GameObject gameObject;
        public GraphicRaycaster graphicRaycaster;
        public RectTransform rect;
        public UIView uiView;

        #endregion

        #region IComponentSet Members

        public GameObject GameObject => gameObject;

        public void Configure(GameObject parent, string baseName)
        {
            using (_PRF_ConfigureView.Auto())
            {
                var targetName = ZString.Format("View - {0}", baseName);

                parent.GetOrCreateChild(ref gameObject, targetName, true);

                gameObject.GetOrCreateComponent(ref rect);
                gameObject.GetOrCreateComponent(ref canvasFadeManager);
                gameObject.GetOrCreateComponent(ref canvasGroup);
                gameObject.GetOrCreateComponent(ref canvas);
                gameObject.GetOrCreateComponent(ref graphicRaycaster);
                gameObject.GetOrCreateComponent(ref uiView);
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(UIViewComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_ConfigureView =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
