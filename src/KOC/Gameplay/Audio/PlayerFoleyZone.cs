#region

using System.Collections.Generic;
using Appalachia.Audio.Components;
using UnityEngine;

#endregion

namespace Appalachia.Prototype.KOC.Gameplay.Audio
{
    public class PlayerFoleyZone : Zone
    {
        #region Constants and Static Readonly

        public static readonly List<PlayerFoleyZone> overrides = new();

        #endregion

        

        internal bool isOverride;

        internal int lastFrame = -1;

     

        #region Event Functions

        protected new void OnDisable()
        {
            base.OnDisable();
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
