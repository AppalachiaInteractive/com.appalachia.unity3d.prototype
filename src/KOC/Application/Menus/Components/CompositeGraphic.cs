using System;
using System.Linq;
using Appalachia.Utility.Async;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Menus.Components
{
    [ExecuteAlways]
    public class CompositeGraphic : Graphic
    {
        #region Fields and Autoproperties

        public Graphic[] graphics;

        #endregion

        public override Color color
        {
            get => Color.clear;
            set => throw new NotImplementedException();
        }

        #region Event Functions

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            if ((graphics == null) || (graphics.Length == 0))
            {
                graphics = GetComponentsInChildren<Graphic>().Where(g => g != this).ToArray();
            }
        }

        #endregion

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
