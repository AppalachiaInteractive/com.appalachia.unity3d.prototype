using System;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Collections
{
    [Serializable]
    public class SimpleCursorLookup : AppalachiaObjectLookupCollection<SimpleCursors, SimpleCursorMetadata,
        SimpleCursorsList, SimpleCursorMetadata.List, SimpleCursorMetadataLookup, SimpleCursorLookup>
    {
        public override bool HasDefault => true;

        protected override SimpleCursors GetUniqueKeyFromValue(SimpleCursorMetadata value)
        {
            return value.value;
        }

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

                    var newMetadata =
                        SimpleCursorMetadata.LoadOrCreateNew<SimpleCursorMetadata>(simpleCursor.ToString());

                    if (newMetadata.texture == null)
                    {
                        newMetadata.texture = AssetDatabaseManager.FindFirstAssetMatch<Sprite>(
                            ZString.Format("t: Texture2D {0}", simpleCursor.ToString())
                        );
                    }

                    newMetadata.value = simpleCursor;

                    newMetadata.MarkAsModified();

                    _items.Add(simpleCursor, newMetadata);

                    MarkAsModified();
                }
            }
        }
#endif
    }
}
