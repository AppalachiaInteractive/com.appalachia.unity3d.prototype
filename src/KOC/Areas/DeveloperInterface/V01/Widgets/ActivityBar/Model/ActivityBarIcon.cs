using System;
using Appalachia.Core.Objects.Delegates.Extensions;
using UnityEngine;
using EventHandler = Appalachia.Core.Objects.Delegates.EventHandler;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.ActivityBar.Model
{
    [Serializable]
    public class ActivityBarEntry
    {
        #region Fields and Autoproperties

        public EventHandler Clicked;
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
