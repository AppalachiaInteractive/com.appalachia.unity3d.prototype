#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.References
{
    public static class AddressableExtensions
    {
        public static void SetAddressableGroup(this Object obj, string groupName = "Default Local Group")
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            if (settings)
            {
                var group = settings.FindGroup(groupName);
                if (!group)
                {
                    group = settings.CreateGroup(
                        groupName,
                        false,
                        false,
                        true,
                        null,
                        typeof(ContentUpdateGroupSchema),
                        typeof(BundledAssetGroupSchema)
                    );
                }

                var assetpath = UnityEditor.AssetDatabase.GetAssetPath(obj);
                var guid = UnityEditor.AssetDatabase.AssetPathToGUID(assetpath);

                var e = settings.CreateOrMoveEntry(guid, group, false, false);
                var entriesAdded = new List<AddressableAssetEntry> {e};

                group.SetDirty(
                    AddressableAssetSettings.ModificationEvent.EntryMoved,
                    entriesAdded,
                    false,
                    true
                );
                settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true);
            }
        }
    }
}

#endif
