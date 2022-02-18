using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Character.States
{
    [Serializable]
    public struct LookingState : IEquatable<LookingState>
    {
        #region Fields and Autoproperties

        [SerializeField] public Vector2 lookingAngles;

        #endregion

        #region IEquatable

        [DebuggerStepThrough]
        public bool Equals(LookingState other)
        {
            return lookingAngles.Equals(other.lookingAngles);
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return obj is LookingState other && Equals(other);
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            return lookingAngles.GetHashCode();
        }

        [DebuggerStepThrough]
        public static bool operator ==(LookingState left, LookingState right)
        {
            return left.Equals(right);
        }

        [DebuggerStepThrough]
        public static bool operator !=(LookingState left, LookingState right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
