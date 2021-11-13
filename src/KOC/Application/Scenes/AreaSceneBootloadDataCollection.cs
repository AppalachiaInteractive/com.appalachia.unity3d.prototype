using System;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Collections;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Logging;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    [Serializable]
    [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.Boxed)]
    [HideLabel]
    [LabelWidth(0)]
    public class AreaSceneBootloadDataCollection : AppalachiaMetadataCollection<
        AreaSceneBootloadDataCollection, SceneBootloadData, AppaList_SceneBootloadData>
    {
        #region Profiling

        private const string _PRF_PFX = nameof(AreaSceneBootloadDataCollection) + ".";

        private static readonly ProfilerMarker _PRF_GetByArea =
            new ProfilerMarker(_PRF_PFX + nameof(GetByArea));

        #endregion

        #region Fields

        public AppaLookup_SceneBootloadData areas;

        #endregion

        public SceneBootloadData GetByArea(ApplicationArea area)
        {
            using (_PRF_GetByArea.Auto())
            {
                AppaLog.Context.Bootload.Info(nameof(GetByArea));

                return areas.Get(area);
            }
        }

#if UNITY_EDITOR
        private static readonly ProfilerMarker _PRF_RegisterNecessaryInstances =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterNecessaryInstances));

        protected override void RegisterNecessaryInstances()
        {
            using (_PRF_RegisterNecessaryInstances.Auto())
            {
                areas.PopulateEnumKeys(area => SceneBootloadData.LoadOrCreateNew(area.ToString()));
            }
        }
#endif
    }
}
