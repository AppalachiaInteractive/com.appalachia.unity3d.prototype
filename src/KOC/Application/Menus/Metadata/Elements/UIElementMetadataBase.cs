using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Prototype.KOC.Application.Components.UI;

namespace Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements
{
    [DoNotReorderFields]
    [Serializable]
    public abstract class UIElementMetadataBase<TC>
    where TC : IComponentSet
    {
        #region Fields and Autoproperties

        [NonSerialized] protected bool _hasConfiguredAlready;
        [NonSerialized] protected bool _haveValuesChanged;

        #endregion

        public bool hasConfiguredAlready => _hasConfiguredAlready;
        public bool haveValuesChanged => _haveValuesChanged;

        protected void OnValuesChanged()
        {
            _haveValuesChanged = true;
        }

        public abstract void Apply(TC component);
    }
}
