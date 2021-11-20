using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Character.States
{
    [Serializable]
    public struct PlayerState : IEquatable<PlayerState>
    {
        

        [SerializeField] public BreathingState breathing;
        [SerializeField] public HumanMovementState movement;
        [SerializeField] public HumanPositioningState positioning;
        [SerializeField] public LookingState looking;


        #region IEquatable

        [DebuggerStepThrough]
        public bool Equals(PlayerState other)
        {
            return positioning.Equals(other.positioning) &&
                   looking.Equals(other.looking) &&
                   movement.Equals(other.movement);
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return obj is PlayerState other && Equals(other);
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = positioning.GetHashCode();
                hashCode = (hashCode * 397) ^ looking.GetHashCode();
                hashCode = (hashCode * 397) ^ movement.GetHashCode();
                return hashCode;
            }
        }

        [DebuggerStepThrough]
        public static bool operator ==(PlayerState left, PlayerState right)
        {
            return left.Equals(right);
        }

        [DebuggerStepThrough]
        public static bool operator !=(PlayerState left, PlayerState right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
