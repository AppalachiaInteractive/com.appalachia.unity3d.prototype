#region

using System.Collections.Generic;
using Appalachia.Audio.Behaviours;
using Appalachia.Utility.Async;
using UnityEngine;

#endregion

namespace Appalachia.Prototype.KOC.Gameplay.Audio
{
    public class PlayerFoleyZone : Zone<PlayerFoleyZone>
    {
        #region Constants and Static Readonly

        public static readonly List<PlayerFoleyZone> overrides = new();

        #endregion

        

        internal bool isOverride;

        internal int lastFrame = -1;

     

        #region Event Functions

        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();
            overrides.Remove(this);
        }

        #endregion

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
    }
} // Gameplay
