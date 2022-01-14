using System;
using Appalachia.Prototype.KOC.Components.Menus.Metadata.Elements;
using Appalachia.Prototype.KOC.Components.UI;

namespace Appalachia.Prototype.KOC.Components.Menus.Metadata.Wrappers
{
    [Serializable]
    public abstract class UIElementWrapper<TE, TC>
        where TE : UIElementMetadataBase<TE, TC>
        where TC : IComponentSet
    {
        #region Fields and Autoproperties

        public TC components;
        public TE metadata;

        #endregion
    }
}
