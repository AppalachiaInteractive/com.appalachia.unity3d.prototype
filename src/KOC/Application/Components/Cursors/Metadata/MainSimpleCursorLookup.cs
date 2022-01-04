using System;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Application.Components.Cursors.Collections;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata
{
    [Serializable]
    public class MainSimpleCursorLookup : SingletonAppalachiaObjectLookupCollection<SimpleCursors,
        SimpleCursorMetadata, SimpleCursorsList, SimpleCursorMetadataList, SimpleCursorMetadataLookup,
        SimpleCursorLookup, MainSimpleCursorLookup>
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(MainSimpleCursorLookup),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew<MainSimpleCursorLookup>();
        }
#endif
    }
}
