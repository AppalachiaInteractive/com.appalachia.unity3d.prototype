using Appalachia.Core.Attributes;
using Appalachia.Core.Events;
using Appalachia.Core.Events.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using TMPro;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.CommandPalette.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class CommandEntryWidget : DeveloperInterfaceManager_V01.Widget<CommandEntryWidget,
        CommandEntryWidgetMetadata, CommandPaletteFeature, CommandPaletteFeatureMetadata>
    {
        static CommandEntryWidget()
        {
            When.Widget(_menuBarWidget).IsAvailableThen(menuBarWidget => { _menuBarWidget = menuBarWidget; });
        }

        #region Static Fields and Autoproperties

        private static MenuBarWidget _menuBarWidget;

        #endregion

        #region Fields and Autoproperties

        public ValueEvent<string>.Data CommandPaletteInputModified;

        public ValueEvent<string>.Data CommandPaletteInputSubmitted;

        [ShowInInspector] private TMP_InputField _inputField;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => _menuBarWidget != null);
        }

        /// <inheritdoc />
        protected override void EnsureWidgetIsCorrectSize()
        {
            using (_PRF_EnsureWidgetIsCorrectSize.Auto())
            {
                base.EnsureWidgetIsCorrectSize();

                var menuBar = _menuBarWidget;

                var anchorMin = RectTransform.anchorMin;
                var anchorMax = RectTransform.anchorMax;

                var halfWidth = metadata.width * 0.5f;

                anchorMin.x = metadata.horizontalCenter - halfWidth;
                anchorMax.x = metadata.horizontalCenter + halfWidth;

                anchorMax.y = 1.0f - menuBar.EffectiveAnchorHeight;
                anchorMin.y = anchorMax.y - metadata.height;

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                base.UnsubscribeFromAllFunctionalities();

                _menuBarWidget.VisuallyChanged.Event -= OnDependencyChanged;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _menuBarWidget.VisuallyChanged.Event += OnDependencyChanged;
            }
        }

        private void OnCommandPaletteInputModified(string input)
        {
            CommandPaletteInputModified.RaiseEvent(input);
        }

        private void OnCommandPaletteInputSubmitted(string input)
        {
            CommandPaletteInputSubmitted.RaiseEvent(input);
        }
    }
}
