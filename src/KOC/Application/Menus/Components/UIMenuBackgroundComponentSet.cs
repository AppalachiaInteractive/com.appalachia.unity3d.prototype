using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Menus.Components
{
    [Serializable, DoNotReorderFields]
    public class UIMenuBackgroundComponentSet : IComponentSet
    {
        #region Fields and Autoproperties

        public GameObject gameObject;
        public RectTransform rect;
        public Image image;

        #endregion

        #region IComponentSet Members

        public GameObject GameObject => gameObject;

        public void Configure(GameObject parent, string baseName)
        {
            using (_PRF_Configure.Auto())
            {
                var targetName = ZString.Format("UI Menu Background - {0}", baseName);

                parent.GetOrCreateChild(ref gameObject, targetName, true);
                gameObject.GetOrCreateComponent(ref rect);
                gameObject.GetOrCreateComponent(ref image);
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(UITemplateComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
