using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.UI.Controls.Sets.Canvas;
using Appalachia.Utility.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Simple
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a SimpleCursor component set.
    /// </summary>
    /// <typeparam name="TSet">The component set.</typeparam>
    /// <typeparam name="TSetData">Styling metadata for the set.</typeparam>
    /// <typeparam name="TISetData">The interface that the style implements.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class
        BaseSimpleCursorComponentSet<TSet, TSetData, TISetData> :
            BaseCanvasComponentSet<TSet, TSetData, TISetData>,
            ISimpleCursorComponentSet
        where TSet : BaseSimpleCursorComponentSet<TSet, TSetData, TISetData>, new()
        where TSetData : BaseSimpleCursorComponentSetData<TSet, TSetData, TISetData>, TISetData
        where TISetData : ISimpleCursorComponentSetData
    {
        #region Fields and Autoproperties

        [SerializeField] private Image _image;

        #endregion

        public override void DestroySet()
        {
            using (_PRF_Destroy.Auto())
            {
                base.DestroySet();

                if (_image)
                {
                    _image.DestroySafely();
                }
            }
        }

        public override void DisableSet()
        {
            using (_PRF_Disable.Auto())
            {
                base.DisableSet();

                if (_image)
                {
                    _image.enabled = false;
                }
            }
        }

        public override void EnableSet()
        {
            using (_PRF_Enable.Auto())
            {
                base.EnableSet();

                if (_image)
                {
                    _image.enabled = true;
                }
            }
        }

        protected override void GetOrAddComponents(ref TSetData data, GameObject parent, string name)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                base.GetOrAddComponents(ref data, parent, name);

                GameObject.GetOrAddComponent(ref _image);
            }
        }

        #region ISimpleCursorComponentSet Members

        public Image Image => _image;

        #endregion
    }
}
