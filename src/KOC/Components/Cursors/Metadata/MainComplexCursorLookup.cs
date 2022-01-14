using System;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Components.Cursors.Collections;

namespace Appalachia.Prototype.KOC.Components.Cursors.Metadata
{
    [Serializable]
    public class MainComplexCursorLookup : SingletonAppalachiaObjectLookupCollection<ComplexCursors,
        ComplexCursorMetadata, ComplexCursorsList, ComplexCursorMetadataList, ComplexCursorMetadataLookup,
        ComplexCursorLookup, MainComplexCursorLookup>
    {
        #region Menu Items

#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(MainComplexCursorLookup),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew<MainComplexCursorLookup>();
        }
#endif

        #endregion
    }
}
