using Appalachia.UI.Controls.Sets.Canvases.Canvas;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Simple
{
    public interface ISimpleCursorComponentSet : ICanvasComponentSet
    {
        Image Image { get; }
    }
}
