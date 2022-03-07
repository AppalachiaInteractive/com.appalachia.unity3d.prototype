﻿using Appalachia.Core.Objects.Root;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Listens to scroll events on the scroll rect that debug items are stored
// and decides whether snap to bottom should be true or not
// 
// Procedure: if, after a user input (drag or scroll), scrollbar is at the bottom, then 
// snap to bottom shall be true, otherwise it shall be false
namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Components
{
    public sealed class DebugsOnScrollListener : AppalachiaBehaviour<DebugsOnScrollListener>,
                                                 IScrollHandler,
                                                 IBeginDragHandler,
                                                 IEndDragHandler
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("developerConsoleManager")]
        public DebugLogManager debugLogManager;

        public ScrollRect debugsScrollRect;

        #endregion

        public void OnScrollbarDragEnd(BaseEventData data)
        {
            if (IsScrollbarAtBottom())
            {
                debugLogManager.SetSnapToBottom(true);
            }
            else
            {
                debugLogManager.SetSnapToBottom(false);
            }
        }

        public void OnScrollbarDragStart(BaseEventData data)
        {
            debugLogManager.SetSnapToBottom(false);
        }

        private bool IsScrollbarAtBottom()
        {
            var scrollbarYPos = debugsScrollRect.verticalNormalizedPosition;
            if (scrollbarYPos <= 1E-6f)
            {
                return true;
            }

            return false;
        }

        #region IBeginDragHandler Members

        public void OnBeginDrag(PointerEventData data)
        {
            debugLogManager.SetSnapToBottom(false);
        }

        #endregion

        #region IEndDragHandler Members

        public void OnEndDrag(PointerEventData data)
        {
            if (IsScrollbarAtBottom())
            {
                debugLogManager.SetSnapToBottom(true);
            }
            else
            {
                debugLogManager.SetSnapToBottom(false);
            }
        }

        #endregion

        #region IScrollHandler Members

        public void OnScroll(PointerEventData data)
        {
            if (IsScrollbarAtBottom())
            {
                debugLogManager.SetSnapToBottom(true);
            }
            else
            {
                debugLogManager.SetSnapToBottom(false);
            }
        }

        #endregion
    }
}