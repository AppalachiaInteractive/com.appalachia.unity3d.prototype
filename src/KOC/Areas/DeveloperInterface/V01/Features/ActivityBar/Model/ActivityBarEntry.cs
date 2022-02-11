using System;
using Appalachia.Core.Events;
using Appalachia.Core.Events.Extensions;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Model
{
    [Serializable]
    public class ActivityBarEntry
    {
        #region Fields and Autoproperties

        public bool clampToTop;

        public AppaEvent.Data Clicked;
        public int priority;
        public Sprite sprite;

        public string name;
        public string tooltip;

        #endregion

        public void OnClicked()
        {
            Clicked.RaiseEvent();
        }
    }
}
