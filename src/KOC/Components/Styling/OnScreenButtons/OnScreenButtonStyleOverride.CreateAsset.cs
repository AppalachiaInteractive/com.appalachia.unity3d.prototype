#if UNITY_EDITOR
namespace Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons
{
    public sealed partial class OnScreenButtonStyleOverride
    {
        #region Menu Items

        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(OnScreenButtonStyleOverride),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew<OnScreenButtonStyleOverride>();
        }

        #endregion
    }
}

#endif
