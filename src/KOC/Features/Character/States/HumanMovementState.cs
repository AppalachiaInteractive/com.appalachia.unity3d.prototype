using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Character.States
{
    [Serializable]
    public struct HumanMovementState : IEquatable<HumanMovementState>
    {
        #region Fields and Autoproperties

        [SerializeField] public bool jumping;
        [SerializeField] public bool jumpStart;
        [SerializeField] public bool swimming;
        [SerializeField] public float jumpingScalar;
        [SerializeField] public float jumpSpeedScalar;
        [SerializeField] public float speedScalar;
        [SerializeField] public Vector2 movingSpeed;

        #endregion

        #region IEquatable

        [DebuggerStepThrough]
        public bool Equals(HumanMovementState other)
        {
            return movingSpeed.Equals(other.movingSpeed) &&
                   speedScalar.Equals(other.speedScalar) &&
                   jumpSpeedScalar.Equals(other.jumpSpeedScalar) &&
                   jumpingScalar.Equals(other.jumpingScalar) &&
                   (swimming == other.swimming) &&
                   (jumping == other.jumping) &&
                   (jumpStart == other.jumpStart);
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return obj is HumanMovementState other && Equals(other);
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = movingSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ speedScalar.GetHashCode();
                hashCode = (hashCode * 397) ^ jumpSpeedScalar.GetHashCode();
                hashCode = (hashCode * 397) ^ jumpingScalar.GetHashCode();
                hashCode = (hashCode * 397) ^ swimming.GetHashCode();
                hashCode = (hashCode * 397) ^ jumping.GetHashCode();
                hashCode = (hashCode * 397) ^ jumpStart.GetHashCode();
                return hashCode;
            }
        }

        [DebuggerStepThrough]
        public static bool operator ==(HumanMovementState left, HumanMovementState right)
        {
            return left.Equals(right);
        }

        [DebuggerStepThrough]
        public static bool operator !=(HumanMovementState left, HumanMovementState right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
