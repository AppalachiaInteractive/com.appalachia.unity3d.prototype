using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Character.States
{
    [Serializable]
    public struct HumanPositioningState : IEquatable<HumanPositioningState>
    {
        [SerializeField] public FootPlacementState leftFoot;

        [SerializeField] public FootPlacementState rightFoot;

        [SerializeField] public HumanFeet feetPlanted;

        [SerializeField] public Vector3 lastVegetationPosition;
        public bool hasAnyFootPlanted => feetPlanted != HumanFeet.Neither;
        public bool hasBothFeetPlanted => feetPlanted == HumanFeet.Both;

        public bool hasNoFeetPlanted => feetPlanted == HumanFeet.Neither;

        #region IEquatable

        [DebuggerStepThrough] public bool Equals(HumanPositioningState other)
        {
            return leftFoot.Equals(other.leftFoot) &&
                   rightFoot.Equals(other.rightFoot) &&
                   (feetPlanted == other.feetPlanted) &&
                   lastVegetationPosition.Equals(other.lastVegetationPosition);
        }

        [DebuggerStepThrough] public override bool Equals(object obj)
        {
            return obj is HumanPositioningState other && Equals(other);
        }

        [DebuggerStepThrough] public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = leftFoot.GetHashCode();
                hashCode = (hashCode * 397) ^ rightFoot.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) feetPlanted;
                hashCode = (hashCode * 397) ^ lastVegetationPosition.GetHashCode();
                return hashCode;
            }
        }

        [DebuggerStepThrough] public static bool operator ==(HumanPositioningState left, HumanPositioningState right)
        {
            return left.Equals(right);
        }

        [DebuggerStepThrough] public static bool operator !=(HumanPositioningState left, HumanPositioningState right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
