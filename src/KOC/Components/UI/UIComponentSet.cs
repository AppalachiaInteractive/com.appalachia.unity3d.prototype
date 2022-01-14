using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.UI
{
    [Serializable, SmartLabelChildren]
    public class UIComponentSet : ComponentSet
    {
        #region Fields and Autoproperties

        public RectTransform rect;

        #endregion

        public override void Configure(GameObject parent, string name, string prefixOverride = null)
        {
            using (_PRF_Configure.Auto())
            {
                base.Configure(parent, name, prefixOverride);

                gameObject.GetOrCreateComponent(ref rect);
            }
        }

        protected override string GetGameObjectNameFormat()
        {
            return "UI - {0}";
        }

        #region Profiling

        private const string _PRF_PFX = nameof(CanvasComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
