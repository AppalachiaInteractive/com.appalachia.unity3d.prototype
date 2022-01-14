using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.UI
{
    [Serializable, SmartLabelChildren]
    public class TemplateComponentSet : CanvasComponentSet
    {
        #region Fields and Autoproperties

        public Image image;

        #endregion

        public override void Configure(GameObject parent, string name, string prefixOverride = null)
        {
            using (_PRF_Configure.Auto())
            {
                base.Configure(parent, name, prefixOverride);

                gameObject.GetOrCreateComponent(ref image);
            }
        }

        protected override string GetGameObjectNameFormat()
        {
            return "Template - {0}";
        }

        #region Profiling

        private const string _PRF_PFX = nameof(TemplateComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
