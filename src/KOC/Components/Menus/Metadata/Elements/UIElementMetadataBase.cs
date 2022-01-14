using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Components.Menus.Metadata.Elements
{
    [DoNotReorderFields]
    [Serializable]
    public abstract class UIElementMetadataBase<T, TC> : AppalachiaBase<T>
        where T : AppalachiaBase<T>
        where TC : IComponentSet
    {
        protected UIElementMetadataBase(Object owner) : base(owner)
        {
        }

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

        public abstract void Apply(GameObject parent, string baseName, TC component);
    }
}
