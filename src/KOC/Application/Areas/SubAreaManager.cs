using System;
using Appalachia.Prototype.KOC.Application.Areas.Base;
using Appalachia.Utility.Logging;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T">The sub-area manager type.</typeparam>
    /// <typeparam name="TM">The metadata scriptable object associated with the sub-area manager.</typeparam>
    /// <typeparam name="TE">The sub-area enum which includes this sub-area.</typeparam>
    /// <typeparam name="TA">The area manager (parent)</typeparam>
    /// <typeparam name="TAM">The area manager metadata.</typeparam>
    [ExecuteAlways]
    public abstract class SubAreaManager<T, TM, TE, TA, TAM> : AreaManagerBase<T, TM>
        where T : SubAreaManager<T, TM, TE, TA, TAM>
        where TM : SubAreaMetadata<TM>
        where TE : Enum
        where TA : AreaManager<TA, TAM>
        where TAM : AreaMetadata<TAM>
    {
        #region Profiling

        private const string _PRF_PFX = nameof(SubAreaManager<T, TM, TE, TA, TAM>) + ".";

        private static readonly ProfilerMarker _PRF_OnceAwake =
            new ProfilerMarker(_PRF_PFX + nameof(OnceAwake));

        #endregion

        public abstract TE SubArea { get; }

        protected override string AreaName => SubArea.ToString();

        protected override void OnceAwake()
        {
            using (_PRF_OnceAwake.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnceAwake));
            }
        }
    }
}
