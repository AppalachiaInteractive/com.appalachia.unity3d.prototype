using System;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Character.Settings
{
    [Serializable]
    public struct PlayerLocomotion : IEquatable<PlayerLocomotion>
    {
        #region Fields and Autoproperties

        [PropertyRange(0f, 10f)]
        [Tooltip("[0, 10]")]
        public float runSpeed;

        [PropertyRange(0f, 10f)]
        [Tooltip("[0, 10]")]
        public float walkSpeed;

        #endregion

        #region IEquatable

        [DebuggerStepThrough]
        public bool Equals(PlayerLocomotion other)
        {
            return walkSpeed.Equals(other.walkSpeed) && runSpeed.Equals(other.runSpeed);
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return obj is PlayerLocomotion other && Equals(other);
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            unchecked
            {
                return (walkSpeed.GetHashCode() * 397) ^ runSpeed.GetHashCode();
            }
        }

        [DebuggerStepThrough]
        public static bool operator ==(PlayerLocomotion left, PlayerLocomotion right)
        {
            return left.Equals(right);
        }

        [DebuggerStepThrough]
        public static bool operator !=(PlayerLocomotion left, PlayerLocomotion right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
