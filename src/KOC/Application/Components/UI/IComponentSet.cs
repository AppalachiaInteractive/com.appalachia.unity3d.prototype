using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.UI
{
    public interface IComponentSet
    {
        void Configure(GameObject parent, string name);
        
        public GameObject GameObject { get; }
    }
}
