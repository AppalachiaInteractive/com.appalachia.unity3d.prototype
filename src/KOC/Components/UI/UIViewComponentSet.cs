using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Utility.Extensions;
using Doozy.Engine.UI;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.UI
{
    [Serializable, SmartLabelChildren]
    public class UIViewComponentSet : CanvasComponentSet
    {
        #region Fields and Autoproperties

        public UIView uiView;

        #endregion

        public override void Configure(GameObject parent, string name, string prefixOverride = null)
        {
            using (_PRF_ConfigureView.Auto())
            {
                base.Configure(parent, name, prefixOverride);

                GameObject.GetOrCreateComponent(ref uiView);
            }
        }

        protected override string GetGameObjectNameFormat()
        {
            return "View - {0}";
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIViewComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_ConfigureView =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
