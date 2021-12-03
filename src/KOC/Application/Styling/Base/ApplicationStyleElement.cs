using Appalachia.Prototype.KOC.Application.Scriptables;

namespace Appalachia.Prototype.KOC.Application.Styling.Base
{
    public abstract class ApplicationStyleElement<TInterface> : AppalachiaApplicationObject
    where TInterface : IApplicationStyle
    {
        public TInterface ToApplicable => (TInterface)(object)this;
    }
}
