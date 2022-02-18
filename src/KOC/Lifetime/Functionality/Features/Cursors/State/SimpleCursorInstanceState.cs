using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Collections;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State
{
    [Serializable]
    [CallStaticConstructorInEditor]
    public sealed class SimpleCursorInstanceState :
        CursorInstanceStateData<SimpleCursorInstanceState, SimpleCursorMetadata>,
        IReadOnlySimpleCursorInstanceStateData
    {
        static SimpleCursorInstanceState()
        {
            RegisterDependency<MainSimpleCursorLookup>(i => _mainSimpleCursorLookup = i);
        }

        public SimpleCursorInstanceState()
        {
        }

        public SimpleCursorInstanceState(Object owner) : base(owner)
        {
        }

        #region Static Fields and Autoproperties

        private static MainSimpleCursorLookup _mainSimpleCursorLookup;

        #endregion

        #region Fields and Autoproperties

        [SerializeField]
        [ReadOnly]
        private SimpleCursors _cursor;

        private SimpleCursorMetadata _metadata;

        #endregion

        /// <inheritdoc />
        public override SimpleCursorMetadata Metadata
        {
            get
            {
                using (_PRF_Metadata.Auto())
                {
                    if ((_metadata == null) || (_metadata.value != Cursor))
                    {
                        _metadata = _mainSimpleCursorLookup.Lookup.Find(Cursor);
                    }

                    return _metadata;
                }
            }
        }

        /// <inheritdoc />
        protected override bool ShouldExecuteInternal()
        {
            using (_PRF_ShouldExecuteInternal.Auto())
            {
                if (_metadata == null)
                {
                    return false;
                }

                if (!_metadata.IsSimple)
                {
                    return false;
                }

                if (Texture == null)
                {
                    return false;
                }

                return true;
            }
        }

        #region IReadOnlySimpleCursorInstanceStateData Members

        public SimpleCursors Cursor => _cursor;

        #endregion
    }
}
