using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Scriptables;

namespace Appalachia.Prototype.KOC.Application.Scriptables
{
    [SmartLabelChildren]
    public class SingletonAppalachiaApplicationObject<T> : SingletonAppalachiaObject<T>
        where T : SingletonAppalachiaApplicationObject<T>
    {
        protected override bool InitializeAlways => true;
    }
}
