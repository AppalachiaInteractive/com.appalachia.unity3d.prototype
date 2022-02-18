using System;
using Appalachia.Prototype.KOC.Behaviours;
using Appalachia.Utility.Async;
using Appalachia.Utility.Timing;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Gameplay
{
    public sealed class CameraRumble : AppalachiaApplicationBehaviour<CameraRumble>
    {
        #region Fields and Autoproperties

        public RumbleInfo innerDisplacement = new()
        {
            phase = 0.3f, frequency = 0.01f, amplitude = new Vector2(0.01f, 0.01f)
        };

        public RumbleInfo innerRotation = new()
        {
            phase = 0.5f, frequency = 0.1f, amplitude = new Vector2(0.001f, 0.001f)
        };

        public RumbleInfo outerDisplacement = new()
        {
            phase = 0f, frequency = 0.1f, amplitude = new Vector2(0.3f, 0.3f)
        };

        public RumbleInfo outerRotation = new()
        {
            phase = 0.5f, frequency = 0.01f, amplitude = new Vector2(0.01f, 0.01f)
        };

        [NonSerialized] private double _time;

        #endregion

        #region Event Functions

        private void LateUpdate()
        {
            _time += CoreClock.Instance.DeltaTime;

            var position = Rumble(_time, outerDisplacement, innerDisplacement);
            var rotation = Quaternion.LookRotation(
                Vector3.forward + Rumble(_time, outerRotation, innerRotation)
            );

            transform.localPosition = position;
            transform.localRotation = rotation;
        }

        private void OnValidate()
        {
            _time = 0;
        }

        #endregion

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _time = 0;
            }
        }

        private static Vector3 Rumble(double time, RumbleInfo outer, RumbleInfo inner)
        {
            const double twoPI = Mathf.PI * 2.0;
            double x, y;

            x = Mathf.Sin((float)((inner.phase + time) * twoPI * inner.frequency)) * inner.amplitude.x;
            x = Mathf.Cos((float)((x + (outer.phase + time)) * twoPI * outer.frequency)) * outer.amplitude.x;

            y = Mathf.Cos((float)((inner.phase + time) * twoPI * inner.frequency)) * inner.amplitude.y;
            y = Mathf.Sin((float)((y + (outer.phase + time)) * twoPI * outer.frequency)) * outer.amplitude.y;

            return new Vector3((float)x, (float)y, 0f);
        }

        #region Nested type: RumbleInfo

        [Serializable]
        public struct RumbleInfo
        {
            #region Fields and Autoproperties

            public float frequency;
            public float phase;
            public Vector2 amplitude;

            #endregion
        }

        #endregion
    }
} // Gameplay
