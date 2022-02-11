using System;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Collections
{
    [Serializable]
    public class MainComplexCursorLookup : SingletonAppalachiaObjectLookupCollection<ComplexCursors,
        ComplexCursorMetadata, ComplexCursorsList, ComplexCursorMetadata.List, ComplexCursorMetadataLookup,
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
