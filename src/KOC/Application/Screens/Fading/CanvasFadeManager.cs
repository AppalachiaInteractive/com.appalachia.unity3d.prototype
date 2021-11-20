using Appalachia.Utility.Extensions;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Screens.Fading
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFadeManager : UIFadeManager<CanvasFadeManager>
    {
        #region Fields and Autoproperties

        public CanvasGroup menuCanvasGroup;

        #endregion

        #region Event Functions

        protected override void Awake()
        {
            base.Awake();
            
            this.GetOrCreateComponent(ref menuCanvasGroup);
        }

        #endregion

        protected override void ExecuteFade(float time)
        {
            menuCanvasGroup.alpha = Mathf.Clamp01(time);
        }
    }
}
