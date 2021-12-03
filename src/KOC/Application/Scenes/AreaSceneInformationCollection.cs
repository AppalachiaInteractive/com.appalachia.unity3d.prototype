using System;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.Collections;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    [Serializable]
    [InlineEditor(Expanded = true, ObjectFieldMode = InlineEditorObjectFieldModes.Boxed)]
    [HideLabel]
    [LabelWidth(0)]
    public class AreaSceneInformationCollection : AppalachiaMetadataCollection<AreaSceneInformationCollection,
        AreaSceneInformation, AppaList_AreaSceneInformation>
    {
        #region Fields and Autoproperties

        [SerializeField] public AppaLookup_AreaSceneInformation areas;

        #endregion

        public AreaSceneInformation GetByArea(ApplicationArea area)
        {
            using (_PRF_GetByArea.Auto())
            {
                AppaLog.Context.Bootload.Trace(nameof(GetByArea));

                return areas.Get(area);
            }
        }

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                if (areas == null)
                {
                    areas = new AppaLookup_AreaSceneInformation();
                    this.MarkAsModified();
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AreaSceneInformationCollection) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_GetByArea =
            new ProfilerMarker(_PRF_PFX + nameof(GetByArea));

        #endregion

#if UNITY_EDITOR
        private static readonly ProfilerMarker _PRF_RegisterNecessaryInstances =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterNecessaryInstances));

        protected override void RegisterNecessaryInstances()
        {
            using (_PRF_RegisterNecessaryInstances.Auto())
            {
                areas.PopulateEnumKeys(area => LoadOrCreateNew<AreaSceneInformation>(area.ToString()));
            }
        }
#endif
    }
}
