using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Utility.Extensions;
using Doozy.Engine.Nody;
using Doozy.Engine.UI;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.UI
{
    [Serializable, SmartLabelChildren]
    public class UICanvasComponentSet : CanvasComponentSet
    {
        #region Fields and Autoproperties

        public CanvasScaler canvasScaler;
        public GraphController graphController;
        public UICanvas uiCanvas;

        #endregion

        public override void Configure(GameObject parent, string name, string prefixOverride = null)
        {
            using (_PRF_Configure.Auto())
            {
                base.Configure(parent, name, prefixOverride);

                gameObject.GetOrCreateComponent(ref canvasScaler);
                gameObject.GetOrCreateComponent(ref graphController);
                gameObject.GetOrCreateComponent(ref uiCanvas);

                uiCanvas.DontDestroyCanvasOnLoad = false;
                graphController.DontDestroyControllerOnLoad = false;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UICanvasComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
