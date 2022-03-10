using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Controls.Sets.MultiPart.MultiPartSubsetWithCanvas;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Components.Subsets;
using UnityEngine;
using Object = UnityEngine.Object;

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
        BaseMultiPartComponentSubsetWithCanvasSetData<TComponentSet, TComponentSetData, TIComponentSetData,
            FadeableImageSubset, FadeableImageSubset.List, FadeableImageSubsetData, FadeableImageSubsetData.List>,
        IComplexCursorComponentSetData
        where TComponentSet : BaseComplexCursorComponentSet<TComponentSet, TComponentSetData, TIComponentSetData>, new()
        where TComponentSetData :
        BaseComplexCursorComponentSetData<TComponentSet, TComponentSetData, TIComponentSetData>, TIComponentSetData, new
        ()
        where TIComponentSetData : IComplexCursorComponentSetData
    {
        #region Fields and Autoproperties

        [SerializeField] private AnimatorData _animatorData;

        #endregion

        /// <inheritdoc />
        protected override void OnApply(TComponentSet componentSet)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(componentSet);

                AnimatorData.RefreshAndApply(ref _animatorData, Owner, componentSet.Animator);
            }
        }

        protected override void OnInitializeFields(Initializer initializer, Object owner)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
                base.OnInitializeFields(initializer, owner);

                AnimatorData.CreateOrRefresh(ref _animatorData, owner);
            }
        }

        #region IComplexCursorComponentSetData Members

        public AnimatorData AnimatorData
        {
            get => _animatorData;
            protected set => _animatorData = value;
        }

        #endregion
    }
}
