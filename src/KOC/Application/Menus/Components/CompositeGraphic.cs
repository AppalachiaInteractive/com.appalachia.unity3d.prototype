using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Menus.Components
{
    [ExecuteAlways]
    public class CompositeGraphic : Graphic
    {
        public Graphic[] graphics;

        public override Color color
        {
            get => Color.clear;
            set => throw new NotImplementedException();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if ((graphics == null) || (graphics.Length == 0))
            {
                graphics = GetComponentsInChildren<Graphic>().Where(g => g != this).ToArray();
            }
        }

        public override void CrossFadeColor(
            Color targetColor,
            float duration,
            bool ignoreTimeScale,
            bool useAlpha,
            bool useRGB)
        {
            if (graphics == null)
            {
                graphics = GetComponentsInChildren<Graphic>();
            }
            
            foreach (var graphic in graphics)
            {
                if (graphic == null)
                {
                    continue;
                }

                if (graphic == this)
                {
                    continue;
                }
                
                graphic.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, useRGB);
            }
        }
    }
}
