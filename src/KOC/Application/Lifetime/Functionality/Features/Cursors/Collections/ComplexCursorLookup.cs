using System;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Utility.Enums;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Collections
{
    [Serializable]
    public class ComplexCursorLookup : AppalachiaObjectLookupCollection<ComplexCursors, ComplexCursorMetadata,
        ComplexCursorsList, ComplexCursorMetadata.List, ComplexCursorMetadataLookup, ComplexCursorLookup>
    {
        #region Fields and Autoproperties

#if UNITY_EDITOR
        public UnityEditor.Animations.AnimatorController templateController;
#endif

        #endregion

        /// <inheritdoc />
        public override bool HasDefault => true;

        /// <inheritdoc />
        protected override ComplexCursors GetUniqueKeyFromValue(ComplexCursorMetadata value)
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
                foreach (var complexCursor in EnumValueManager.GetAllValues<ComplexCursors>())
                {
                    if (_items.ContainsKey(complexCursor))
                    {
                        continue;
                    }

                    var newMetadata =
                        ComplexCursorMetadata.LoadOrCreateNew<ComplexCursorMetadata>(
                            complexCursor.ToString()
                        );

                    /*if (newMetadata.prefab == null)
                    {
                        newMetadata.prefab = AssetDatabaseManager.FindFirstAssetMatch<GameObject>(
                            ZString.Format("t:Prefab {0}", complexCursor.ToString())
                        );
                    }*/

                    newMetadata.value = complexCursor;

                    newMetadata.MarkAsModified();

                    _items.Add(complexCursor, newMetadata);

                    MarkAsModified();
                }
            }
        }
#endif
    }
}
