using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.UI.Controls.Animation;
using Appalachia.UI.Controls.Sets.MultiPartSubsetWithCanvas;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Extensions;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Complex
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a Cursor component set.
    /// </summary>
    /// <typeparam name="TSet">The component set.</typeparam>
    /// <typeparam name="TSetData">Styling metadata for the set.</typeparam>
    /// <typeparam name="TISetData">The interface that the style implements.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseComplexCursorComponentSet<TSet, TSetData, TISetData> :
        BaseMultiPartComponentSubsetWithCanvasSet<TSet, TSetData, TISetData, FadeableImageSubset,
            FadeableImageSubset.List, FadeableImageSubsetData, FadeableImageSubsetData.List>,
        IComplexCursorComponentSet
        where TSet : BaseComplexCursorComponentSet<TSet, TSetData, TISetData>, new()
        where TSetData : BaseComplexCursorComponentSetData<TSet, TSetData, TISetData>, TISetData
        where TISetData : IComplexCursorComponentSetData
    {
        #region Fields and Autoproperties

        [SerializeField] private Animator _animator;
#if UNITY_EDITOR
        [SerializeField] private AnimationRemapper _animationRemapper;
#endif

        #endregion

        public override void DestroySet()
        {
            using (_PRF_Destroy.Auto())
            {
                base.DestroySet();

                if (_animator)
                {
                    _animator.DestroySafely();
                }
#if UNITY_EDITOR
                if (_animationRemapper)
                {
                    _animationRemapper.DestroySafely();
                }
#endif
            }
        }

        public override void DisableSet()
        {
            using (_PRF_Disable.Auto())
            {
                base.DisableSet();

                if (_animator)
                {
                    _animator.enabled = false;
                }

#if UNITY_EDITOR
                if (_animationRemapper)
                {
                    _animationRemapper.enabled = false;
                }
#endif
            }
        }

        public override void EnableSet()
        {
            using (_PRF_Enable.Auto())
            {
                base.EnableSet();

                if (_animator)
                {
                    _animator.enabled = true;
                }

#if UNITY_EDITOR
                if (_animationRemapper)
                {
                    _animationRemapper.enabled = true;
                }
#endif
            }
        }

        protected override void GetOrAddComponents(ref TSetData data, GameObject parent, string name)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                base.GetOrAddComponents(ref data, parent, name);

                GameObject.GetOrAddComponent(ref _animator);
                GameObject.GetOrAddComponent(ref _animationRemapper);
            }
        }

        #region IComplexCursorComponentSet Members

        public Animator Animator => _animator;

#if UNITY_EDITOR
        public AnimationRemapper AnimationRemapper => _animationRemapper;
#endif

        #endregion
    }
}
