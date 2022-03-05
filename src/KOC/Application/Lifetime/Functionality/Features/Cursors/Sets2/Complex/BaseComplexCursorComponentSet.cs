using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets.Complex;
using Appalachia.UI.Controls.Animation;
using Appalachia.UI.Controls.Sets2.MultiPart.MultiPartSubsetWithCanvas;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Extensions;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets2.Complex
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a Cursor component set.
    /// </summary>
    /// <typeparam name="TComponentSet">The component set.</typeparam>
    /// <typeparam name="TComponentSetData">Styling metadata for the set.</typeparam>
    /// <typeparam name="TIComponentSetData">The interface that the style implements.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseComplexCursorComponentSet<TComponentSet, TComponentSetData, TIComponentSetData> :
        BaseMultiPartComponentSubsetWithCanvasSet<TComponentSet, TComponentSetData, TIComponentSetData, FadeableImageSubset,
            FadeableImageSubset.List, FadeableImageSubsetData, FadeableImageSubsetData.List>,
        IComplexCursorComponentSet
        where TComponentSet : BaseComplexCursorComponentSet<TComponentSet, TComponentSetData, TIComponentSetData>, new()
        where TComponentSetData : BaseComplexCursorComponentSetData<TComponentSet, TComponentSetData, TIComponentSetData>, TIComponentSetData, new()
        where TIComponentSetData : IComplexCursorComponentSetData
    {
        #region Fields and Autoproperties

        [SerializeField] private Animator _animator;
#if UNITY_EDITOR
        [SerializeField] private AnimationRemapper _animationRemapper;
#endif

        #endregion

        /// <inheritdoc />
        public override void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                base.DestroySafely();

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

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                base.Disable();

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

        /// <param name="data"></param>
        /// <inheritdoc />
        public override void Enable(TComponentSetData data)
        {
            using (_PRF_Enable.Auto())
            {
                base.Enable(data);

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

        /// <inheritdoc />
        protected override void OnGetOrAddComponents()
        {
            using (_PRF_OnGetOrAddComponents.Auto())
            {
                base.OnGetOrAddComponents();

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
