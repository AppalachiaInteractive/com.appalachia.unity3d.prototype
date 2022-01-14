using Appalachia.Core.Simulation.Solvers;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Interpolation.Interpolators;
using Appalachia.Utility.Interpolation.Modes;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Gameplay
{
    [DisallowMultipleComponent]
    public class FirstPersonCamera : PlayerCamera
    {
        #region Fields and Autoproperties

        [Space(9)]
        [Range(0, 10)]
        public float eyeHeight = 1.8f;

        [Range(0, 10)] public float interpolationSpeed = 6f;

        [Range(0, 10)] public float springDampening = 1.15f;

        [Space(9)]
        [Range(0, 10)]
        public float springMass = 2f;

        public Vector3 springCoefficients = new(0.3f, 0.45f, 0.3f);
        private Spring _spring;

        private TypedInterpolator<LinearAngle> _pitch;
        private TypedInterpolator<LinearAngle> _roll;
        private TypedInterpolator<LinearAngle> _yaw;

        #endregion

        public Quaternion targetRotation => Quaternion.Euler(_pitch.max, _yaw.max, _roll.max);

        public float pitch
        {
            get => _pitch;
            set => _pitch.Target(value);
        }

        public float roll
        {
            get => _roll;
            set => _roll.Target(value);
        }

        public float yaw
        {
            get => _yaw;
            set => _yaw.Target(value);
        }

        public override void Simulate(Vector3 playerPosition, Vector3 playerAngles, float deltaTime)
        {
            var t = transform;
            var interpolationDeltaTime = deltaTime * interpolationSpeed;

            _pitch.Update(interpolationDeltaTime);
            _yaw.Update(interpolationDeltaTime);
            _roll.Update(interpolationDeltaTime);

            _pitch.percentage = 0f;
            _yaw.percentage = 0f;
            _roll.percentage = 0f;

            var position = playerPosition;
            var angles = playerAngles;

            // spring

            position = _spring.Update(
                deltaTime * interpolationSpeed,
                position,
                springMass,
                springDampening,
                springCoefficients
            );

            // craning

            var ax = Angles.ToRelative(pitch);
            var ay = Mathf.DeltaAngle(angles.y, yaw);
            var dy = 1f - Mathf.Cos(ax * Mathf.Deg2Rad);
            var dz = 0f - Mathf.Sin(ax * Mathf.Deg2Rad);
            var ex = 0f - Mathf.Sin(ay * Mathf.Deg2Rad);
            var ez = 1f - Mathf.Cos(ay * Mathf.Deg2Rad);

            ex *= 1f - Mathf.Max(dz + 0.5f, 0f);
            ez *= 1f - Mathf.Max(dz + 0.5f, 0f);

            ex *= Mathf.Lerp(0.15f, 0.30f, Mathf.Abs(dz));
            ez *= Mathf.Lerp(0.15f, 0.25f, Mathf.Abs(dz));
            dy *= 0.05f;
            dz *= 0.35f;

            position.y += eyeHeight;
            position -= Quaternion.Euler(angles) * (new Vector3(0f, dy, dz) + new Vector3(ex, 0f, ez));

            t.localPosition = position;
            t.localRotation = Quaternion.Euler(pitch, yaw, roll);
        }

        public override void Warp(Vector3 position, Vector3 angles)
        {
            _spring.position = position;
            _spring.velocity = Vector3.zero;

            _pitch.current = angles.x + (_pitch.current - _pitch.max);
            _pitch.max = angles.x;
            _yaw.current = angles.y + (_yaw.current - _yaw.max);
            _yaw.max = angles.y;
            _roll.current = angles.z + (_roll.current - _roll.max);
            _roll.max = angles.z;
        }
    }
} // Gameplay
