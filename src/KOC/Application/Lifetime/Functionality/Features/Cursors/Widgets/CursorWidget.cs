using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Instance.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets.Complex;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets.Simple;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Widgets
{
    [CallStaticConstructorInEditor]
    public class CursorWidget : LifetimeWidget<CursorWidget, CursorWidgetMetadata, CursorFeature,
        CursorFeatureMetadata>
    {
        static CursorWidget()
        {
            When.Service<CursorService>().IsAvailableThen(i => _cursorService = i);
        }

        #region Static Fields and Autoproperties

        private static CursorService _cursorService;

        #endregion

        #region Fields and Autoproperties

        [SerializeField] private ComplexCursorComponentSet.List _complexCursorSets;
        [SerializeField] private SimpleCursorComponentSet.List _simpleCursorSets;

        #endregion

        public ComplexCursorComponentSet.List ComplexCursorSets
        {
            get
            {
                if (_complexCursorSets == null)
                {
                    _complexCursorSets = new();
                }

                return _complexCursorSets;
            }
        }

        public SimpleCursorComponentSet.List SimpleCursorSets
        {
            get
            {
                if (_simpleCursorSets == null)
                {
                    _simpleCursorSets = new();
                }

                return _simpleCursorSets;
            }
        }

        public IReadOnlyList<ICursorInstance> CursorInstances => _cursorService.CursorInstances;

        /// <inheritdoc />
        protected override void OnUpdate()
        {
            using (_PRF_OnUpdate.Auto())
            {
                base.OnUpdate();
            }
        }
    }
}
