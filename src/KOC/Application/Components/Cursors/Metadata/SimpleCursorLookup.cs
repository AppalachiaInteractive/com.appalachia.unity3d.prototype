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
    public class SimpleCursorLookup : AppalachiaObjectLookupCollection<SimpleCursors, SimpleCursorMetadata,
        SimpleCursorsList, SimpleCursorMetadataList, SimpleCursorMetadataLookup, SimpleCursorLookup>
    {
        public override bool HasDefault => true;

        protected override SimpleCursors GetUniqueKeyFromValue(SimpleCursorMetadata value)
        {
            return value.simpleCursorValue;
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComplexCursorLookup) + ".";

        #endregion

#if UNITY_EDITOR

        private static readonly ProfilerMarker _PRF_CreateAll =
            new ProfilerMarker(_PRF_PFX + nameof(CreateAll));

        [Button]
        public void CreateAll()
        {
            using (_PRF_CreateAll.Auto())
            {
                foreach (var simpleCursor in EnumValueManager.GetAllValues<SimpleCursors>())
                {
                    if (_items.ContainsKey(simpleCursor))
                    {
                        continue;
                    }

                    var newMetadata = SimpleCursorMetadata.LoadOrCreateNew(simpleCursor.ToString());

                    if (newMetadata.texture == null)
                    {
                        newMetadata.texture = AssetDatabaseManager.FindFirstAssetMatch<Texture2D>(
                            ZString.Format("t: Texture2D {0}", simpleCursor.ToString())
                        );
                    }

                    newMetadata.simpleCursorValue = simpleCursor;

                    newMetadata.MarkAsModified();

                    _items.Add(simpleCursor, newMetadata);

                    MarkAsModified();
                }
            }
        }
#endif
    }
}
