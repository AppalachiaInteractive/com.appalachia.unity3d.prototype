using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Components.UI;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements
{
    [DoNotReorderFields]
    [Serializable]
    public abstract class UIElementMetadataBase<TC>
        where TC : IComponentSet
    {
        #region Fields and Autoproperties

        [SerializeField] private Initializer _initialer;

        #endregion

        protected Initializer initializer
        {
            get
            {
                if (_initialer == null)
                {
                    _initialer = new Initializer();
                }

                return _initialer;
            }
        }

        public abstract void Apply(GameObject parent, string baseName, ref TC component);
    }
}
