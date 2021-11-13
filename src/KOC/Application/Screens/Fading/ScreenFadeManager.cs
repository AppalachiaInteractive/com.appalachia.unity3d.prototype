using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Screens.Fading
{
    public class ScreenFadeManager : UIFadeManager<ScreenFadeManager>
    {
        public Image FullScreenBlack;

        protected override void ExecuteFade(float time)
        {
            var color = FullScreenBlack.color;
            color.a = Mathf.Clamp01(time);
            FullScreenBlack.color = color;
        }
    }
}
