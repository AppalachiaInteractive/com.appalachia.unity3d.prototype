using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Menus.Components
{
    [Serializable, DoNotReorderFields]
    public struct UIMenuBackgroundComponentSet : IComponentSet
    {
        #region Fields and Autoproperties

        public GameObject GameObject => gameObject;
        
        public GameObject gameObject;
        public RectTransform rect;
        public Image image;

        #endregion

        #region IComponentSet Members

        public void Configure(GameObject parent, string baseName)
        {
            using (_PRF_Configure.Auto())
            {
                var targetName = $"UI Menu Background - {baseName}";

                parent.CreateOrGetChild(ref gameObject, targetName, true);
                gameObject.CreateOrGetComponent(ref rect);
                gameObject.CreateOrGetComponent(ref image);
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