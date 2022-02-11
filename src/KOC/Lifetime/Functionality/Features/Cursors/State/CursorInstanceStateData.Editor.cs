#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State
{
    public partial class CursorInstanceStateData<T, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField]
        [BoxGroup("Testing (Editor Only)")]
        private bool _animate;

        [SerializeField]
        [BoxGroup("Testing (Editor Only)")]
        private bool _animateMovement;

        [SerializeField]
        [BoxGroup("Testing (Editor Only)")]
        private bool _animateState;

        [SerializeField, PropertyRange(1f, 10f)]
        [BoxGroup("Testing (Editor Only)")]
        private float _animationMovementDuration;

        [SerializeField, PropertyRange(0.01f, .5f)]
        [BoxGroup("Testing (Editor Only)")]
        private float _animationRadius;

        [SerializeField, PropertyRange(1f, 10f)]
        [BoxGroup("Testing (Editor Only)")]
        private float _animationStateChangeDuration;

        #endregion

        #region IReadLimitedWriteCursorInstanceStateData Members

        public bool Animate => _animate;
        public bool AnimateMovement => _animateMovement;
        public bool AnimateState => _animateState;
        public float AnimationMovementDuration => _animationMovementDuration;
        public float AnimationRadius => _animationRadius;
        public float AnimationStateChangeDuration => _animationStateChangeDuration;

        #endregion
    }
}

#endif
