/*
using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.UI.Controls.Sets2.MultiPart.MultiPartSubsetWithCanvas;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Components.Subsets;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets.Complex
{
    /// <summary>
    ///     Defines the metadata necessary for configuring a
    ///     <see cref="BaseComplexCursorComponentSet{TComponentSet, TComponentSetData, TIComponentSetData}" />.
    /// </summary>
    /// <typeparam name="TComponentSet">The component set.</typeparam>
    /// <typeparam name="TComponentSetData">Styling metadata for the set.</typeparam>
    /// <typeparam name="TIComponentSetData">The interface that the style implements.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseComplexCursorComponentSetData<TComponentSet, TComponentSetData, TIComponentSetData> :
        BaseMultiPartComponentSubsetWithCanvasSetData<TComponentSet, TComponentSetData, TIComponentSetData, FadeableImageSubset,
            FadeableImageSubset.List, FadeableImageSubsetData, FadeableImageSubsetData.List>,
        IComplexCursorComponentSetData
        where TComponentSet : BaseComplexCursorComponentSet<TComponentSet, TComponentSetData, TIComponentSetData>, new()
        where TComponentSetData : BaseComplexCursorComponentSetData<TComponentSet, TComponentSetData, TIComponentSetData>, TIComponentSetData
        where TIComponentSetData : IComplexCursorComponentSetData
    {
        #region Fields and Autoproperties

        [SerializeField] private AnimatorData _animatorData;

        #endregion

        /// <inheritdoc />
        public override void ApplyToComponentSet(TComponentSet componentSet)
        {
            using (_PRF_ApplyToComponentSet.Auto())
            {
                base.ApplyToComponentSet(componentSet);

                AnimatorData.RefreshAndUpdate(ref _animatorData, this, componentSet.Animator);
            }
        }

        #region IComplexCursorComponentSetData Members

        public AnimatorData AnimatorData => _animatorData;

        #endregion
    }
}
*/
