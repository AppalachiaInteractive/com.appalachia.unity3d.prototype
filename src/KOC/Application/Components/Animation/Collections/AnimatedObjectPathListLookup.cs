#if UNITY_EDITOR
using System;
using Appalachia.Core.Collections;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Animation.Collections
{
    [Serializable, HideLabel]
    [ListDrawerSettings(
        ShowItemCount = false,
        DraggableItems = false,
        HideAddButton = true,
        HideRemoveButton = true,
        Expanded = false
    )]
    public class AnimatedObjectPathListLookup : AppaLookup<AnimationClip, AnimatedObjectPathList,
        AnimationClipList, AnimatedObjectPath2DList>
    {
        protected override bool ShouldDisplayTitle => true;

        protected override Color GetDisplayColor(AnimationClip key, AnimatedObjectPathList value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(AnimationClip key, AnimatedObjectPathList value)
        {
            return ZString.Format("{0} bindings", Values.Count);
        }

        protected override string GetDisplayTitle(AnimationClip key, AnimatedObjectPathList value)
        {
            return key.name;
        }
    }
}

#endif
