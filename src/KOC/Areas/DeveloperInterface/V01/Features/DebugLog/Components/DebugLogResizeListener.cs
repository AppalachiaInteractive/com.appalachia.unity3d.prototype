using Appalachia.Core.Objects.Root;
using UnityEngine;
using UnityEngine.EventSystems;

// Listens to drag event on the DebugLogManager's resize button
namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Components
{
    public sealed class DebugLogResizeListener : AppalachiaBehaviour<DebugLogResizeListener>,
                                                 IBeginDragHandler,
                                                 IDragHandler
    {
        #region Fields and Autoproperties

#pragma warning disable 0649
        [SerializeField] private DebugLogManager debugManager;
#pragma warning restore 0649

        #endregion

        #region IBeginDragHandler Members

        // This interface must be implemented in order to receive drag events
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
        }

        #endregion

        #region IDragHandler Members

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            debugManager.Resize(eventData);
        }

        #endregion
    }
}
