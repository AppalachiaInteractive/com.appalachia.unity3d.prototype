using System;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.OnScreenButtons.Controls
{
    [Serializable]
    public struct ControlButtonSpriteSet
    {
        #region Fields and Autoproperties

        [SerializeField] public Sprite illustrative;
        [SerializeField] public Sprite outline;
        [SerializeField] public Sprite reversedOutline;

        #endregion

        public Sprite GetSprite(OnScreenButtonSpriteStyle spriteStyle)
        {
            return spriteStyle switch
            {
                OnScreenButtonSpriteStyle.Outline => outline,
                OnScreenButtonSpriteStyle.ReversedOutline => reversedOutline,
                OnScreenButtonSpriteStyle.Illustrative => illustrative,
                _ => throw new ArgumentOutOfRangeException(nameof(spriteStyle), spriteStyle, null)
            };
        }
    }
}
