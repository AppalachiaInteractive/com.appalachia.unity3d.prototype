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
    public class UIViewComponentSet : ComponentSet
    {
        public UIViewComponentSet(GameObject go) : base(go)
        {
        }

        #region Fields and Autoproperties

        public Canvas canvas;
        public CanvasFadeManager canvasFadeManager;
        public CanvasGroup canvasGroup;

        public GraphicRaycaster graphicRaycaster;
        public RectTransform rect;
        public UIView uiView;

        #endregion

        public override void Configure(GameObject parent, string baseName)
        {
            using (_PRF_ConfigureView.Auto())
            {
                var targetName = ZString.Format("View - {0}", baseName);

                parent.GetOrCreateChild(ref gameObject, targetName, true);

                GameObject.GetOrCreateComponent(ref rect);
                GameObject.GetOrCreateComponent(ref canvasFadeManager);
                GameObject.GetOrCreateComponent(ref canvasGroup);
                GameObject.GetOrCreateComponent(ref canvas);
                GameObject.GetOrCreateComponent(ref graphicRaycaster);
                GameObject.GetOrCreateComponent(ref uiView);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIViewComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_ConfigureView =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
