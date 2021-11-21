using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Screens.Fading
{
    public class ScreenFadeManager : FadeManager<ScreenFadeManager>
    {
        #region Fields and Autoproperties

        public Image FullScreenBlack;

        #endregion

        protected override void ExecuteFade(float time)
        {
            var color = FullScreenBlack.color;
            color.a = Mathf.Clamp01(time);
            FullScreenBlack.color = color;
        }
    }
}
