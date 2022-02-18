#region

using Appalachia.Audio.Behaviours;
using Appalachia.Utility.Async;
using UnityEngine;

#endregion

namespace Appalachia.Prototype.KOC.Features.Gameplay.Audio
{
    public class PlayerFoleyZone : Zone<PlayerFoleyZone>
    {
        #region Constants and Static Readonly

        public static readonly System.Collections.Generic.List<PlayerFoleyZone> overrides = new();

        #endregion

        #region Fields and Autoproperties

        internal bool isOverride;

        internal int lastFrame = -1;

        #endregion

        /// <inheritdoc />
        protected override void OnProbe(Vector3 lpos, int thisFrame)
        {
            if (lastFrame != thisFrame)
            {
                lastFrame = thisFrame;

                var pos = transform.position;
                var sqrDistance = (lpos - pos).sqrMagnitude;
                var sqrRadius = radius * radius;
                var active = sqrDistance <= sqrRadius;

                if (active)
                {
                    if (!isOverride)
                    {
                        isOverride = true;
                        overrides.Add(this);
                    }
                }
                else
                {
                    if (isOverride)
                    {
                        isOverride = false;
                        overrides.Remove(this);
                    }
                }

                SetActive(active);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();
            overrides.Remove(this);
        }
    }
} // Gameplay
