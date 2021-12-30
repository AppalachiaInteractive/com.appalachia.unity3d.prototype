using System;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.UI
{
    [Serializable]
    public abstract class ComponentSet : IComponentSet
    {
        protected ComponentSet(GameObject go)
        {
            gameObject = go;
        }

        #region Fields and Autoproperties

        [SerializeField] protected GameObject gameObject;

        #endregion

        #region IComponentSet Members

        public GameObject GameObject => gameObject;

        public abstract void Configure(GameObject parent, string name);

        #endregion
    }
}
