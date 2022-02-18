using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Features.Character.Settings
{
    [Serializable]
    public struct PlayerJumping : IEquatable<PlayerJumping>
    {
        #region Fields and Autoproperties

        [PropertyRange(0f, 20f)]
        [PropertyTooltip("[0, 20]")]
        public float dampSpeed;

        [PropertyRange(0f, 10f)]
        [PropertyTooltip("[0, 10]")]
        public float force;

        [PropertyRange(0f, 10f)]
        [PropertyTooltip("[0, 10]")]
        public float gravityFactor;

        #endregion

        #region IEquatable

        [DebuggerStepThrough]
        public bool Equals(PlayerJumping other)
        {
            return force.Equals(other.force) &&
                   dampSpeed.Equals(other.dampSpeed) &&
                   gravityFactor.Equals(other.gravityFactor);
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return obj is PlayerJumping other && Equals(other);
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = force.GetHashCode();
                hashCode = (hashCode * 397) ^ dampSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ gravityFactor.GetHashCode();
                return hashCode;
            }
        }

        [DebuggerStepThrough]
        public static bool operator ==(PlayerJumping left, PlayerJumping right)
        {
            return left.Equals(right);
        }

        [DebuggerStepThrough]
        public static bool operator !=(PlayerJumping left, PlayerJumping right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
