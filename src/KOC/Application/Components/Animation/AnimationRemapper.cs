#if UNITY_EDITOR
using System;
using System.Linq;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components.Animation.Collections;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Animation
{
    [AddComponentMenu(PKG.Prefix + nameof(AnimationRemapper))]
    [RequireComponent(typeof(Animator))]
    public class AnimationRemapper : AppalachiaApplicationBehaviour<AnimationRemapper>
    {
        #region Fields and Autoproperties

        [NonSerialized, ShowInInspector]
        public AnimatedObjectPathLookup mappings;

        [FoldoutGroup("References")]
        [SerializeField]
        public Animator animator;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                await initializer.Do(
                    this,
                    nameof(Animator),
                    animator == null,
                    () => { gameObject.GetOrCreateComponent(ref animator); }
                );

                await initializer.Do(
                    this,
                    nameof(AnimatedObjectPathLookup),
                    mappings == null,
                    () => { mappings ??= new AnimatedObjectPathLookup(); }
                );

                mappings.SetObjectOwnership(this);
                RefreshMappings();
            }
        }

        [ButtonGroup, PropertyOrder(10)]
        private void ApplyMappings()
        {
            using (_PRF_ApplyMappings.Auto())
            {
                var runtimeAnimationControllor = animator.runtimeAnimatorController;

                var animationClips = runtimeAnimationControllor.animationClips;

                for (var clipIndex = 0; clipIndex < animationClips.Length; clipIndex++)
                {
                    var animationClip = animationClips[clipIndex];

                    var curveBindings = UnityEditor.AnimationUtility.GetCurveBindings(animationClip);

                    for (var bindingIndex = 0; bindingIndex < curveBindings.Length; bindingIndex++)
                    {
                        var binding = curveBindings[bindingIndex];

                        var curve = UnityEditor.AnimationUtility.GetEditorCurve(animationClip, binding);

                        var nestedLookup = mappings.Get(binding.path);
                        var pathList = nestedLookup.Get(animationClip);

                        var matchingPath = pathList.First(m => m.OriginalBinding == binding);

                        if (matchingPath.ShouldUpdate)
                        {
                            binding = matchingPath.ToCurveBinding();

                            UnityEditor.AnimationUtility.SetEditorCurve(animationClip, binding, curve);
                            animationClip.MarkAsModified();
                        }
                    }
                }
            }
        }

        [ButtonGroup]
        private void RefreshMappings()
        {
            using (_PRF_RefreshMappings.Auto())
            {
                var runtimeAnimationControllor = animator.runtimeAnimatorController;

                var animationClips = runtimeAnimationControllor.animationClips;

                mappings.Clear();

                for (var clipIndex = 0; clipIndex < animationClips.Length; clipIndex++)
                {
                    var animationClip = animationClips[clipIndex];

                    var curveBindings = UnityEditor.AnimationUtility.GetCurveBindings(animationClip);

                    for (var bindingIndex = 0; bindingIndex < curveBindings.Length; bindingIndex++)
                    {
                        var binding = curveBindings[bindingIndex];

                        var path = new AnimatedObjectPath(this, binding);

                        var nestedLookup = mappings.GetOrCreate(
                            binding.path,
                            () =>
                            {
                                var result = new AnimatedObjectPathListLookup();
                                result.SetObjectOwnership(this);

                                return result;
                            }
                        );

                        var pathList = nestedLookup.GetOrCreate(
                            animationClip,
                            () => new AnimatedObjectPathList()
                        );

                        pathList.Add(path);
                    }
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AnimationRemapper) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_RefreshMappings =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshMappings));

        private static readonly ProfilerMarker _PRF_ApplyMappings =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyMappings));

        #endregion
    }
}

#endif
