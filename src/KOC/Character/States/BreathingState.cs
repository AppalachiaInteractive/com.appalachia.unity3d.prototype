using System;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Character.States
{
    [Serializable]
    public struct BreathingState
    {
        [SerializeField] public BreathDirection state;
        [SerializeField] public float currentIntensity;

        [SerializeField] public float currentPace;
        [SerializeField] public float nextIntensity;

        [SerializeField] public float nextPace;
        [SerializeField] public float period;
        [SerializeField] public float time;

        [SerializeField] public RespirationSpeed speed;
        [SerializeField] public RespirationStyle style;
        public bool exhaling => state == BreathDirection.Exhale;

        public bool inhaling => state == BreathDirection.Inhale;
    }
}
