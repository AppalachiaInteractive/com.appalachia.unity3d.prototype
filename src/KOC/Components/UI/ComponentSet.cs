using System;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.UI
{
    [Serializable]
    [InlineProperty, FoldoutGroup("Components"), HideLabel]
    public abstract class ComponentSet : IComponentSet
    {
        #region Fields and Autoproperties

        [SerializeField] protected GameObject gameObject;

        #endregion

        protected virtual string GetGameObjectNameFormat()
        {
            return "{0}";
        }

        #region IComponentSet Members

        public GameObject GameObject => gameObject;

        public virtual void Configure(GameObject parent, string name, string prefixOverride = null)
        {
            using (_PRF_Configure.Auto())
            {
                var namePrefix = prefixOverride.IsNullOrWhiteSpace()
                    ? GetGameObjectNameFormat()
                    : prefixOverride;
                var targetName = ZString.Format(namePrefix, name);

                if (gameObject == null)
                {
                    parent.GetOrCreateChild(ref gameObject, targetName, true);
                }

                gameObject.name = targetName;
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentSet) + ".";

        private static readonly ProfilerMarker _PRF_Configure =
            new ProfilerMarker(_PRF_PFX + nameof(Configure));

        #endregion
    }
}
