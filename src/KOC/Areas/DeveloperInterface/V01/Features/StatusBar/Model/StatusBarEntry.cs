using System;
using Appalachia.Core.Events;
using Appalachia.Core.Events.Extensions;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Model
{
    [Serializable]
    public class StatusBarEntry
    {
        #region Fields and Autoproperties

        public bool clampToLeft;

        public int priority;
        public Sprite sprite;

        public string name;
        public string tooltip;

        #endregion

        public AppaEvent.Data Clicked;

        public void OnClicked()
        {
            Clicked.RaiseEvent();
        }
    }
}
