using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Components.Fading;
using Appalachia.Prototype.KOC.Extensions;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.UI
{
    [Serializable, SmartLabelChildren]
    public class CanvasComponentSet : UIComponentSet
    {
        #region Fields and Autoproperties

        public Canvas canvas;

        [FormerlySerializedAs("canvasFadeManager")]
        public CanvasFadeManager fadeManager;

        public CanvasGroup canvasGroup;
        public GraphicRaycaster graphicRaycaster;

        #endregion

        public override void Configure(GameObject parent, string name, string prefixOverride = null)
        {
            using (_PRF_Configure.Auto())
            {
                base.Configure(parent, name, prefixOverride);

                gameObject.GetOrCreateComponent(ref fadeManager);
                gameObject.GetOrCreateComponent(ref canvasGroup);
                gameObject.GetOrCreateComponent(ref canvas);
                gameObject.GetOrCreateComponent(ref graphicRaycaster);

                rect.Reset(RectResetOptions.All);
            }
        }

        protected override string GetGameObjectNameFormat()
        {
            return "Canvas - {0}";
        }

        #region Profiling

        private const string _PRF_PFX = nameof(CanvasComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
