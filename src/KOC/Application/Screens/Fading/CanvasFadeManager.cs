using Appalachia.Core.Extensions;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Screens.Fading
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFadeManager : UIFadeManager<CanvasFadeManager>
    {
        public CanvasGroup menuCanvasGroup;

        private void Awake()
        {
            this.GetOrCreateComponent(ref menuCanvasGroup);
        }

        protected override void ExecuteFade(float time)
        {
            menuCanvasGroup.alpha = Mathf.Clamp01(time);
        }
    }
}
