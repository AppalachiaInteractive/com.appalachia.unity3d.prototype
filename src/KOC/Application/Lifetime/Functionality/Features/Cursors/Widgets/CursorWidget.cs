using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Simple;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Instance.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Services;
using UnityEngine;
using UnityEngine.Serialization;

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

        [FormerlySerializedAs("_complexCursorSets")] [SerializeField] private ComplexCursorControl.List _complexCursors;
        [FormerlySerializedAs("_simpleCursorSets")] [SerializeField] private SimpleCursorControl.List _simpleCursors;

        #endregion

        public ComplexCursorControl.List ComplexCursors
        {
            get
            {
                if (_complexCursors == null)
                {
                    _complexCursors = new();
                }

                return _complexCursors;
            }
        }

        public SimpleCursorControl.List SimpleCursors
        {
            get
            {
                if (_simpleCursors == null)
                {
                    _simpleCursors = new();
                }

                return _simpleCursors;
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
