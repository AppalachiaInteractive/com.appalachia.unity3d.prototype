using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.UI.Controls.Sets.MultiPart.MultiPartSubsetWithCanvas;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Components.Subsets;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Complex
{
    /// <summary>
    ///     Defines the metadata necessary for configuring a
    ///     <see cref="BaseComplexCursorComponentSet{TSet, TSetData, TISetData}" />.
    /// </summary>
    /// <typeparam name="TSet">The component set.</typeparam>
    /// <typeparam name="TSetData">Styling metadata for the set.</typeparam>
    /// <typeparam name="TISetData">The interface that the style implements.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseComplexCursorComponentSetData<TSet, TSetData, TISetData> :
        BaseMultiPartComponentSubsetWithCanvasSetData<TSet, TSetData, TISetData, FadeableImageSubset,
            FadeableImageSubset.List, FadeableImageSubsetData, FadeableImageSubsetData.List>,
        IComplexCursorComponentSetData
        where TSet : BaseComplexCursorComponentSet<TSet, TSetData, TISetData>, new()
        where TSetData : BaseComplexCursorComponentSetData<TSet, TSetData, TISetData>, TISetData
        where TISetData : IComplexCursorComponentSetData
    {
        #region Fields and Autoproperties

        [SerializeField] private AnimatorData _animatorData;

        #endregion

        /// <inheritdoc />
        public override void ApplyToComponentSet(TSet componentSet)
        {
            using (_PRF_ApplyToComponentSet.Auto())
            {
                base.ApplyToComponentSet(componentSet);

                AnimatorData.RefreshAndUpdateComponent(ref _animatorData, this, componentSet.Animator);
            }
        }

        #region IComplexCursorComponentSetData Members

        public AnimatorData AnimatorData => _animatorData;

        #endregion
    }
}
