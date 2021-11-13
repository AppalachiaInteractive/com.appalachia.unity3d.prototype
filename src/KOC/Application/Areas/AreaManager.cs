using Appalachia.Prototype.KOC.Application.Areas.Base;
using Appalachia.Prototype.KOC.Application.Scenes;
using Appalachia.Utility.Logging;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    [ExecuteAlways]
    public abstract class AreaManager<TAM, TAMD> : AreaManagerBase<TAM, TAMD>
        where TAM : AreaManager<TAM, TAMD>
        where TAMD : AreaMetadata<TAMD>
    {
        #region Profiling

        private const string _PRF_PFX = nameof(AreaManager<TAM, TAMD>) + ".";

        private static readonly ProfilerMarker _PRF_OnceAwake =
            new ProfilerMarker(_PRF_PFX + nameof(OnceAwake));

        private static readonly ProfilerMarker _PRF_OnReset = new ProfilerMarker(_PRF_PFX + nameof(OnReset));

        #endregion

        #region Fields

        protected SceneBootloadData _bootloadData;

        #endregion

        public abstract ApplicationArea Area { get; }

        protected override string AreaName => Area.ToString();

        protected SceneBootloadData bootloadData => _bootloadData;

        protected abstract void ResetArea();

        protected override void OnceAwake()
        {
            using (_PRF_OnceAwake.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnceAwake));

                _bootloadData = AreaSceneBootloadDataCollection.instance.GetByArea(Area);
            }
        }

        protected override void OnReset(bool resetting)
        {
            using (_PRF_OnReset.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnReset));

                if (resetting)
                {
                    _bootloadData = null;
                }
            }
        }
    }
}
