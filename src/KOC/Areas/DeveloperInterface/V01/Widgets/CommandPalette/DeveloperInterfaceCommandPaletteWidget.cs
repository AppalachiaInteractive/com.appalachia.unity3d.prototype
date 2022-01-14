using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandPalette.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.MenuBar;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using TMPro;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandPalette
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInterfaceCommandPaletteWidget : AreaWidget<
        DeveloperInterfaceCommandPaletteWidget, DeveloperInterfaceCommandPaletteWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        public event CommandPaletteInputHandler CommandPaletteInputModified;
        public event CommandPaletteInputHandler CommandPaletteInputSubmitted;

        static DeveloperInterfaceCommandPaletteWidget()
        {
            RegisterDependency<DeveloperInterfaceMenuBarWidget>(i => _developerInterfaceMenuBarWidget = i);
        }

        #region Static Fields and Autoproperties

        private static DeveloperInterfaceMenuBarWidget _developerInterfaceMenuBarWidget;

        #endregion

        #region Fields and Autoproperties

        [ShowInInspector] private TMP_InputField _inputField;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                _inputField = initializer.Get(gameObject, _inputField, GetComponentStrategy.Children);

                _inputField.onValueChanged.AddListener(OnCommandPaletteInputModified);
                _inputField.onSubmit.AddListener(OnCommandPaletteInputSubmitted);

                _inputField.text = ">";
            }
        }

        protected override void OnApplyMetadataInternal()
        {
        }

        protected override void SubscribeToAllFunctionalties()
        {
            using (_PRF_SubscribeToAllFunctionalties.Auto())
            {
                _developerInterfaceMenuBarWidget.VisuallyChanged += ApplyMetadata;
            }
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                _developerInterfaceMenuBarWidget.VisuallyChanged -= ApplyMetadata;
            }
        }

        protected override void UpdateSizeInternal()
        {
            using (_PRF_UpdateSizeInternal.Auto())
            {
                var menuBar = _developerInterfaceMenuBarWidget;

                var anchorMin = rectTransform.anchorMin;
                var anchorMax = rectTransform.anchorMax;

                var halfWidth = metadata.width * 0.5f;

                anchorMin.x = metadata.horizontalCenter - halfWidth;
                anchorMax.x = metadata.horizontalCenter + halfWidth;

                anchorMax.y = 1.0f - menuBar.EffectiveAnchorHeight;
                anchorMin.y = anchorMax.y - metadata.height;

                rectTransform.anchorMin = anchorMin;
                rectTransform.anchorMax = anchorMax;
            }
        }

        private void OnCommandPaletteInputModified(string input)
        {
            CommandPaletteInputModified?.Invoke(input);
        }

        private void OnCommandPaletteInputSubmitted(string input)
        {
            CommandPaletteInputSubmitted?.Invoke(input);
        }
    }
}
