using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Character.Settings
{
    [Serializable]
    public struct PlayerSettings : IEquatable<PlayerSettings>
    {
        [SerializeField] public Breathing breathing;
        [SerializeField] public PlayerFootPlanting footPlanting;
        [SerializeField] public PlayerJumping jumping;
        [SerializeField] public PlayerLocomotion locomotion;
        [SerializeField] public PlayerLooking looking;

        public void Initialize()
        {
            looking = new PlayerLooking
            {
                lookSpeed = 0.5f,
                runLookSpeed = 1f,
                pitchLimitMin = -45f,
                pitchLimitMax = 90f
            };

            footPlanting = new PlayerFootPlanting
            {
                floorLayers = 1,
                walkStepDistance = 1.2f,
                runStepDistance = 2.5f,
                stopSpeedThreshold = 0f
            };

            jumping = new PlayerJumping {force = 1.15f, dampSpeed = 5f, gravityFactor = 1f};

            locomotion = new PlayerLocomotion {walkSpeed = 2f, runSpeed = 6f};

            breathing = new Breathing
            {
                initialDelay = 0.3f,
                initialDelayVariance = 0.1f,
                inhalePeriod = 1.6f,
                inhalePeriodVariance = 0.2f,
                inhalePeriodPacingFactor = 0.3f,
                exhalePeriod = 0.5f,
                exhalePeriodVariance = 0.15f,
                breathingPeriod = 3f,
                breathingPeriodVariance = 0.15f,
                breathingPeriodIntensityFactor = 1.5f,
                intensityDampening = 0.1f,
                intensityTransference = 0.1f,
                volumeOverPace = 0.8f,
                volumeOverPaceVariance = 0.2f
            };
        }

        public static PlayerSettings Create()
        {
            var instance = new PlayerSettings();

            instance.Initialize();

            return instance;
        }

        #region IEquatable

        [DebuggerStepThrough] public bool Equals(PlayerSettings other)
        {
            return looking.Equals(other.looking) &&
                   footPlanting.Equals(other.footPlanting) &&
                   jumping.Equals(other.jumping) &&
                   locomotion.Equals(other.locomotion);
        }

        [DebuggerStepThrough] public override bool Equals(object obj)
        {
            return obj is PlayerSettings other && Equals(other);
        }

        [DebuggerStepThrough] public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = looking.GetHashCode();
                hashCode = (hashCode * 397) ^ footPlanting.GetHashCode();
                hashCode = (hashCode * 397) ^ jumping.GetHashCode();
                hashCode = (hashCode * 397) ^ locomotion.GetHashCode();
                return hashCode;
            }
        }

        [DebuggerStepThrough] public static bool operator ==(PlayerSettings left, PlayerSettings right)
        {
            return left.Equals(right);
        }

        [DebuggerStepThrough] public static bool operator !=(PlayerSettings left, PlayerSettings right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
