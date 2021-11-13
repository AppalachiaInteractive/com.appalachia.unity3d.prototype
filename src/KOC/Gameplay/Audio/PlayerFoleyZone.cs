#region

using System.Collections.Generic;
using Appalachia.Audio.Components;
using UnityEngine;

#endregion

namespace Appalachia.Prototype.KOC.Gameplay.Audio
{
    public class PlayerFoleyZone : Zone
    {
        public static readonly List<PlayerFoleyZone> overrides = new();

        internal bool isOverride;

        internal int lastFrame = -1;

        protected new void OnDisable()
        {
            base.OnDisable();
            overrides.Remove(this);
        }

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
