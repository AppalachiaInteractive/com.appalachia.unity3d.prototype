using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.UI
{
    public interface IComponentSet
    {
        public GameObject GameObject { get; }
        void Configure(GameObject parent, string name, string prefixOverride = null);
    }
}
