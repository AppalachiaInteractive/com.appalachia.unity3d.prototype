using Appalachia.Core.Objects.Root;
using UnityEngine;
using UnityEngine.EventSystems;

// Listens to drag event on the DebugLogManager's resize button
namespace Appalachia.Prototype.KOC.Debugging.DebugConsole
{
    public sealed class DebugLogResizeListener : AppalachiaBehaviour<DebugLogResizeListener>,
                                                 IBeginDragHandler,
                                                 IDragHandler
    {
#pragma warning disable 0649
        [SerializeField] private DebugLogManager debugManager;
#pragma warning restore 0649

        // This interface must be implemented in order to receive drag events
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            debugManager.Resize(eventData);
        }
    }
}
