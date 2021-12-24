using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.UI
{
    public interface IComponentSet
    {
        public GameObject GameObject { get; }
        void Configure(GameObject parent, string name);
    }
}
