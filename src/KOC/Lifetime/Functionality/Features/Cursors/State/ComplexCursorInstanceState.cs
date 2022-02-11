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
    public sealed class ComplexCursorInstanceState :
        CursorInstanceStateData<ComplexCursorInstanceState, ComplexCursorMetadata>,
        IReadOnlyComplexCursorInstanceStateData
    {
        static ComplexCursorInstanceState()
        {
            RegisterDependency<MainComplexCursorLookup>(i => _mainComplexCursorLookup = i);
        }

        public ComplexCursorInstanceState()
        {
        }

        public ComplexCursorInstanceState(Object owner) : base(owner)
        {
        }

        #region Static Fields and Autoproperties

        private static MainComplexCursorLookup _mainComplexCursorLookup;

        #endregion

        #region Fields and Autoproperties

        private ComplexCursorMetadata _metadata;

        [SerializeField]
        [ReadOnly]
        private ComplexCursors _cursor;

        #endregion

        public override ComplexCursorMetadata Metadata
        {
            get
            {
                using (_PRF_Metadata.Auto())
                {
                    if ((_metadata == null) || (_metadata.value != Cursor))
                    {
                        _metadata = _mainComplexCursorLookup.Lookup.Find(Cursor);
                    }

                    return _metadata;
                }
            }
        }

        protected override bool ShouldExecuteInternal()
        {
            using (_PRF_ShouldExecuteInternal.Auto())
            {
                if (_metadata == null)
                {
                    return false;
                }

                if (_metadata.IsSimple)
                {
                    return false;
                }

                return true;
            }
        }

        #region IReadOnlyComplexCursorInstanceStateData Members

        public ComplexCursors Cursor => _cursor;

        #endregion
    }
}
