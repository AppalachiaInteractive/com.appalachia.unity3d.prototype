using Appalachia.UI.Functionality.Canvas.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Images.Groups.Default;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Simple.Contracts
{
    public interface ISimpleCursorControl : ICanvasControl
    {
        ImageComponentGroup Image { get; }
        GameObject ImageParent { get; set; }
    }
}
