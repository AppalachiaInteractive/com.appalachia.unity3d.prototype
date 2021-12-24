using System;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Playables.TMPTextSwitcher
{
    [Serializable]
    public class TMPTextSwitcherBehaviour : AppalachiaPlayable
    {
        public Color color = Color.white;
        public float fontSize = 14;
        public string text;
    }
}
