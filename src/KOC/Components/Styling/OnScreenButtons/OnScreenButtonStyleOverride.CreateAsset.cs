#if UNITY_EDITOR
namespace Appalachia.Prototype.KOC.Components.OnScreenButtons
{
    public sealed partial class OnScreenButtonMetadata
    {
        #region Menu Items

        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(OnScreenButtonMetadata),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew<OnScreenButtonMetadata>();
        }

        #endregion
    }
}

#endif
