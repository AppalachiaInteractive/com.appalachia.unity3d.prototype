/*
using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.UI.Controls.Sets2.Canvases.Canvas;
using Appalachia.Utility.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets.Simple
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a SimpleCursor component set.
    /// </summary>
    /// <typeparam name="TComponentSet">The component set.</typeparam>
    /// <typeparam name="TComponentSetData">Styling metadata for the set.</typeparam>
    /// <typeparam name="TIComponentSetData">The interface that the style implements.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class
        BaseSimpleCursorComponentSet<TComponentSet, TComponentSetData, TIComponentSetData> :
            BaseCanvasComponentSet<TComponentSet, TComponentSetData, TIComponentSetData>,
            ISimpleCursorComponentSet
        where TComponentSet : BaseSimpleCursorComponentSet<TComponentSet, TComponentSetData, TIComponentSetData>, new()
        where TComponentSetData : BaseSimpleCursorComponentSetData<TComponentSet, TComponentSetData, TIComponentSetData>, TIComponentSetData
        where TIComponentSetData : ISimpleCursorComponentSetData
    {
        #region Fields and Autoproperties

        [SerializeField] private Image _image;

        #endregion

        /// <inheritdoc />
        public override void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                base.DestroySafely();

                if (_image)
                {
                    _image.DestroySafely();
                }
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                base.Disable();

                if (_image)
                {
                    _image.enabled = false;
                }
            }
        }

        /// <param name="data"></param>
        /// <inheritdoc />
        public override void Enable(TComponentSetData data)
        {
            using (_PRF_Enable.Auto())
            {
                base.Enable(data);

                if (_image)
                {
                    _image.enabled = true;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnGetOrAddComponents()
        {
            using (_PRF_OnGetOrAddComponents.Auto())
            {
                base.OnGetOrAddComponents();

                GameObject.GetOrAddComponent(ref _image);
            }
        }

        #region ISimpleCursorComponentSet Members

        public Image Image => _image;

        #endregion
    }
}
*/
