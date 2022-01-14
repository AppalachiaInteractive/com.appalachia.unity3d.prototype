using Appalachia.Core.Attributes;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.Controls
{
    [CallStaticConstructorInEditor]
    public class DummySelectable : Selectable
    {
        static DummySelectable()
        {
            if (s_SelectableCount < 0)
            {
                s_SelectableCount = 0;
            }
        }
    }
}
