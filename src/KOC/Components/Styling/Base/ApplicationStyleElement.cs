using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;

namespace Appalachia.Prototype.KOC.Components.Styling.Base
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationStyleElement<T, TInterface> : AppalachiaObject<T>
        where T : ApplicationStyleElement<T, TInterface>
        where TInterface : IApplicationStyle
    {
        public delegate void OnStyleChanged(TInterface style);

        public event OnStyleChanged StyleChanged;

        static ApplicationStyleElement()
        {
            RegisterDependency<ApplicationStyleElementDefaultLookup>(
                i => _applicationStyleElementDefaultLookup = i
            );
        }

        #region Static Fields and Autoproperties

        private static ApplicationStyleElementDefaultLookup _applicationStyleElementDefaultLookup;

        #endregion

        protected static ApplicationStyleElementDefaultLookup ApplicationStyleElementDefaultLookup =>
            _applicationStyleElementDefaultLookup;

        public TInterface ToApplicable => (TInterface)(object)this;

        protected virtual void InvokeStyleChanged()
        {
            if (this is TInterface ti)
            {
                StyleChanged?.Invoke(ti);
            }
        }
    }
}
