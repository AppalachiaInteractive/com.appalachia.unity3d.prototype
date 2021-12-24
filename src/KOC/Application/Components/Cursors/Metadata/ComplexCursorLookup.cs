using System;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Application.Components.Cursors.Collections;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata
{
    [Serializable]
    public class ComplexCursorLookup : AppalachiaObjectLookupCollection<ComplexCursors, ComplexCursorMetadata,
        ComplexCursorsList, ComplexCursorMetadataList, ComplexCursorMetadataLookup, ComplexCursorLookup>
    {
        public override bool HasDefault => true;

#if UNITY_EDITOR
        public UnityEditor.Animations.AnimatorController templateController;
#endif

#if UNITY_EDITOR
        private static readonly ProfilerMarker _PRF_CreateAll =
            new ProfilerMarker(_PRF_PFX + nameof(CreateAll));

        [Button]
        public void CreateAll()
        {
            using (_PRF_CreateAll.Auto())
            {
                foreach (var complexCursor in EnumValueManager.GetAllValues<ComplexCursors>())
                {
                    if (_items.ContainsKey(complexCursor))
                    {
                        continue;
                    }

                    var newMetadata = ComplexCursorMetadata.LoadOrCreateNew(complexCursor.ToString());

                    if (newMetadata.prefab == null)
                    {
                        newMetadata.prefab = AssetDatabaseManager.FindFirstAssetMatch<GameObject>(
                            ZString.Format("t:Prefab {0}", complexCursor.ToString())
                        );
                    }

                    newMetadata.complexCursorValue = complexCursor;

                    newMetadata.MarkAsModified();

                    _items.Add(complexCursor, newMetadata);

                    MarkAsModified();
                }
            }
        }
#endif

        protected override ComplexCursors GetUniqueKeyFromValue(ComplexCursorMetadata value)
        {
            return value.complexCursorValue;
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComplexCursorLookup) + ".";

        #endregion
    }
}
