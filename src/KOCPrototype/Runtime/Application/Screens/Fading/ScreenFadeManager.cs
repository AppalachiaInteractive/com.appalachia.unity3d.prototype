using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOCPrototype.Application.Screens.Fading
{
    public class ScreenFadeManager : UIFadeManager
    {
        [Header("Target")]
        public Image FullScreenBlack;

        protected override void ExecuteFade(float time)
        {
            var color = FullScreenBlack.color;
            color.a = Mathf.Clamp01(time);
            FullScreenBlack.color = color;
        }
    }
}
