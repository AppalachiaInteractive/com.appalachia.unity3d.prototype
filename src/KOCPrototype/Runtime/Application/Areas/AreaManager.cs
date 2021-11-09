using Appalachia.Core.Behaviours;
using Appalachia.Prototype.KOCPrototype.Application.Components;
using Appalachia.Prototype.KOCPrototype.Application.Screens.Fading;

namespace Appalachia.Prototype.KOCPrototype.Application.Areas
{
    public abstract class AreaManager<T> : SingletonAppalachiaBehaviour<T>, IAreaManager
        where T : AreaManager<T>
    {
        protected CanvasFadeManager _menuCanvasFader;

        protected ApplicationLifetimeComponents lifetimeComponents;

        protected override void OnAwake()
        {
            if (_menuCanvasFader == null)
            {
                _menuCanvasFader = FindObjectOfType<CanvasFadeManager>();
            }
            
            lifetimeComponents =  ApplicationLifetimeComponents.instance;
        }
    }
}
