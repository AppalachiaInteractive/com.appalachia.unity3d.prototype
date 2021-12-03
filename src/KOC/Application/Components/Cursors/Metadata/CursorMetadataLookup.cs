using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Components.Cursors.Collections;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata
{
    public class CursorMetadataLookup : AppalachiaObjectLookupCollection<Cursors, CursorMetadata,
        AppaList_Cursors, AppaList_CursorMetadata, AppaLookup_CursorMetadata, CursorMetadataLookup>
    {
        public override bool HasDefault => true;

        [Button]
        public void CreateAll()
        {
            using (_PRF_CreateAll.Auto())
            {
                foreach (var cursor in EnumValueManager.GetAllValues<Cursors>())
                {
                    if (_items.ContainsKey(cursor))
                    {
                        continue;
                    }

                    var newMetadata = LoadOrCreateNew<CursorMetadata>(cursor.ToString());

                    if (newMetadata.texture == null)
                    {
                        newMetadata.texture =
                            AssetDatabaseManager.FindFirstAssetMatch<Texture2D>(
                                $"t: Texture2D {cursor.ToString()}"
                            );
                    }

                    newMetadata.value = cursor;

                    newMetadata.MarkAsModified();

                    _items.Add(cursor, newMetadata);

                   this.MarkAsModified();
                }
            }
        }

        protected override Cursors GetUniqueKeyFromValue(CursorMetadata value)
        {
            return value.value;
        }

        #region Profiling

        private const string _PRF_PFX = nameof(CursorMetadataLookup) + ".";

        private static readonly ProfilerMarker _PRF_CreateAll =
            new ProfilerMarker(_PRF_PFX + nameof(CreateAll));

        #endregion
    }
}
