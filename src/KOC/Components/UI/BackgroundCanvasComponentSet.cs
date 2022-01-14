using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Extensions;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.UI
{
    [Serializable, SmartLabelChildren]
    public class BackgroundCanvasComponentSet : CanvasComponentSet
    {
        #region Fields and Autoproperties

        public GameObject backgroundObject;
        public Image background;

        #endregion

        public override void Configure(GameObject parent, string name, string prefixOverride = null)
        {
            using (_PRF_Configure.Auto())
            {
                base.Configure(parent, name, prefixOverride);

                var namePrefix = "Background - {0}";
                var targetName = ZString.Format(namePrefix, name);

                if (backgroundObject == null)
                {
                    gameObject.GetOrCreateChild(ref backgroundObject, targetName, true);
                }
                else
                {
                    backgroundObject.name = targetName;
                }

                backgroundObject.GetOrCreateComponent(ref background);
                background.rectTransform.Reset(RectResetOptions.All);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(BackgroundCanvasComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
