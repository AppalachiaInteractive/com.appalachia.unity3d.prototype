using Appalachia.Core.Objects.Root;

namespace Appalachia.Prototype.KOC.Application.Styling.Base
{
    public abstract class ApplicationStyleElement<TInterface> : AppalachiaObject<>
        where TInterface : IApplicationStyle
    {
        public delegate void OnStyleChanged(TInterface style);

        public TInterface ToApplicable => (TInterface)(object)this;

        public event OnStyleChanged StyleChanged;

        protected virtual void InvokeStyleChanged()
        {
            if (this is TInterface ti)
            {
                StyleChanged?.Invoke(ti);
            }
        }
    }
}
