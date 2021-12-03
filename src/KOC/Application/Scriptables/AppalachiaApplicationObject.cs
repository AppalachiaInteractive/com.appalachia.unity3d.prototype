using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Scriptables;

namespace Appalachia.Prototype.KOC.Application.Scriptables
{
    [SmartLabelChildren]
    public class AppalachiaApplicationObject : AppalachiaObject
    {
        protected override bool InitializeAlways => true;
    }
}
