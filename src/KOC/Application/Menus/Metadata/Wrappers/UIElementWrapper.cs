using System;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements;

namespace Appalachia.Prototype.KOC.Application.Menus.Metadata.Wrappers
{
    [Serializable]
    public abstract class UIElementWrapper<TE, TC>
    where TE : UIElementMetadataBase<TC>
    where TC : IComponentSet
    {
        #region Fields and Autoproperties

        public TC components;
        public TE metadata;

        #endregion
    }
}
