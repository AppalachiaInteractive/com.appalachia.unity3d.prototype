using System;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Collections
{
    [Serializable]
    public class MainSimpleCursorLookup : SingletonAppalachiaObjectLookupCollection<SimpleCursors,
        SimpleCursorMetadata, SimpleCursorsList, SimpleCursorMetadata.List, SimpleCursorMetadataLookup,
        SimpleCursorLookup, MainSimpleCursorLookup>
    {
        #region Menu Items

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

        #endregion
    }
}
