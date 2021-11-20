using System;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Character.Settings
{
    [Serializable]
    public struct PlayerJumping : IEquatable<PlayerJumping>
    {
        

        [PropertyRange(0f, 20f)]
        [Tooltip("[0, 20]")]
        public float dampSpeed;

        [PropertyRange(0f, 10f)]
        [Tooltip("[0, 10]")]
        public float force;

        [PropertyRange(0f, 10f)]
        [Tooltip("[0, 10]")]
        public float gravityFactor;


        #region IEquatable

        [DebuggerStepThrough]
        public bool Equals(PlayerJumping other)
        {
            return force.Equals(other.force) &&
                   dampSpeed.Equals(other.dampSpeed) &&
                   gravityFactor.Equals(other.gravityFactor);
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return obj is PlayerJumping other && Equals(other);
        }

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
