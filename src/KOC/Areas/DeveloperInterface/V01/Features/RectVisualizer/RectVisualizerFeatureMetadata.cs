using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer
{
    public class RectVisualizerFeatureMetadata : DeveloperInterfaceMetadata_V01.FeatureMetadata<
        RectVisualizerFeature, RectVisualizerFeatureMetadata>
    {
        #region Fields and Autoproperties

        public float z;

        [BoxGroup("Modifiers")] public OverridableColor inactive;
        [BoxGroup("Canvas")] public OverridableColor canvas;
        [BoxGroup("Canvas")] public OverridableColor screenSpace;
        [BoxGroup("Canvas")] public OverridableColor worldSpace;
        [BoxGroup("Mask")] public OverridableColor mask;
        [BoxGroup("Selectable")] public OverridableColor selectable;
        [BoxGroup("Selectable")] public OverridableColor button;
        [BoxGroup("Selectable")] public OverridableColor dropdown;
        [BoxGroup("Selectable")] public OverridableColor inputField;
        [BoxGroup("Selectable")] public OverridableColor slider;
        [BoxGroup("Selectable")] public OverridableColor toggle;
        [BoxGroup("Selectable")] public OverridableColor scrollbar;
        [BoxGroup("Graphic")] public OverridableColor graphic;

        [BoxGroup("Graphic/Text")]
        public OverridableColor text;

        [BoxGroup("Graphic/Image")]
        public OverridableColor image;

        [BoxGroup("Graphic/Image")]
        public OverridableColor raycastTarget;

        [BoxGroup("Graphic/Image")]
        public OverridableColor maskable;

        [BoxGroup("Layout")] public OverridableColor canvasScaler;
        [BoxGroup("Layout")] public OverridableColor aspectRatioFitter;
        [BoxGroup("Layout")] public OverridableColor contentSizeFitter;
        [BoxGroup("Layout")] public OverridableColor layoutElement;
        [BoxGroup("Layout")] public OverridableColor layoutGroup;
        [BoxGroup("Layout")] public OverridableColor scrollRect;

        #endregion

        protected override void UpdateFunctionality(RectVisualizerFeature functionality)
        {
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(inactive),
                    inactive == default,
                    () => { inactive = new OverridableColor(true, Colors.White.ScaleA(.25f)); }
                );

                initializer.Do(
                    this,
                    nameof(canvas),
                    canvas == default,
                    () => { canvas = new OverridableColor(true, Colors.PaleTurquoise4); }
                );

                initializer.Do(
                    this,
                    nameof(screenSpace),
                    screenSpace == default,
                    () => { screenSpace = new OverridableColor(true, Colors.PaleGoldenrod); }
                );

                initializer.Do(
                    this,
                    nameof(worldSpace),
                    worldSpace == default,
                    () => { worldSpace = new OverridableColor(true, Colors.PaleGreen3); }
                );

                initializer.Do(
                    this,
                    nameof(mask),
                    mask == default,
                    () => { mask = new OverridableColor(true, Colors.VioletRed); }
                );

                initializer.Do(
                    this,
                    nameof(selectable),
                    selectable == default,
                    () => { selectable = new OverridableColor(true, Colors.PaleTurquoise1); }
                );

                initializer.Do(
                    this,
                    nameof(button),
                    button == default,
                    () => { button = new OverridableColor(true, Colors.TurquoiseBlue); }
                );

                initializer.Do(
                    this,
                    nameof(dropdown),
                    dropdown == default,
                    () => { dropdown = new OverridableColor(true, Colors.Turquoise3); }
                );

                initializer.Do(
                    this,
                    nameof(inputField),
                    inputField == default,
                    () => { inputField = new OverridableColor(true, Colors.PaleTurquoise2); }
                );

                initializer.Do(
                    this,
                    nameof(slider),
                    slider == default,
                    () => { slider = new OverridableColor(true, Colors.Turquoise2); }
                );

                initializer.Do(
                    this,
                    nameof(toggle),
                    toggle == default,
                    () => { toggle = new OverridableColor(true, Colors.MediumTurquoise); }
                );

                initializer.Do(
                    this,
                    nameof(scrollbar),
                    scrollbar == default,
                    () => { scrollbar = new OverridableColor(true, Colors.DarkTurquoise); }
                );

                initializer.Do(
                    this,
                    nameof(graphic),
                    graphic == default,
                    () => { graphic = new OverridableColor(true, Colors.CadmiumYellow); }
                );

                initializer.Do(
                    this,
                    nameof(text),
                    text == default,
                    () => { text = new OverridableColor(true, Colors.Gold1); }
                );

                initializer.Do(
                    this,
                    nameof(image),
                    image == default,
                    () => { image = new OverridableColor(true, Colors.IndianRed1); }
                );

                initializer.Do(
                    this,
                    nameof(raycastTarget),
                    raycastTarget == default,
                    () => { raycastTarget = new OverridableColor(true, Colors.Purple); }
                );

                initializer.Do(
                    this,
                    nameof(maskable),
                    maskable == default,
                    () => { maskable = new OverridableColor(true, Colors.MediumVioletRed); }
                );

                initializer.Do(
                    this,
                    nameof(canvasScaler),
                    canvasScaler == default,
                    () => { canvasScaler = new OverridableColor(true, Colors.Orange1); }
                );

                initializer.Do(
                    this,
                    nameof(aspectRatioFitter),
                    aspectRatioFitter == default,
                    () => { aspectRatioFitter = new OverridableColor(true, Colors.Orange1.ScaleV(.8f)); }
                );

                initializer.Do(
                    this,
                    nameof(contentSizeFitter),
                    contentSizeFitter == default,
                    () => { contentSizeFitter = new OverridableColor(true, Colors.Orange1.ScaleV(.8f)); }
                );

                initializer.Do(
                    this,
                    nameof(layoutElement),
                    layoutElement == default,
                    () =>
                    {
                        layoutElement = new OverridableColor(true, Colors.Orange1.ScaleV(.6f).ScaleS(.8f));
                    }
                );

                initializer.Do(
                    this,
                    nameof(layoutGroup),
                    layoutGroup == default,
                    () => { layoutGroup = new OverridableColor(true, Colors.Orange1.ScaleV(.3f)); }
                );

                initializer.Do(
                    this,
                    nameof(scrollRect),
                    scrollRect == default,
                    () => { scrollRect = new OverridableColor(true, Colors.Orange1.ScaleS(.5f).ScaleV(.3f)); }
                );
            }
        }

        protected override void SubscribeResponsiveComponents(RectVisualizerFeature target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                inactive.Changed.Event += OnChanged;
                canvas.Changed.Event += OnChanged;
                screenSpace.Changed.Event += OnChanged;
                worldSpace.Changed.Event += OnChanged;
                mask.Changed.Event += OnChanged;
                selectable.Changed.Event += OnChanged;
                button.Changed.Event += OnChanged;
                dropdown.Changed.Event += OnChanged;
                inputField.Changed.Event += OnChanged;
                slider.Changed.Event += OnChanged;
                toggle.Changed.Event += OnChanged;
                scrollbar.Changed.Event += OnChanged;
                graphic.Changed.Event += OnChanged;
                text.Changed.Event += OnChanged;
                image.Changed.Event += OnChanged;
                raycastTarget.Changed.Event += OnChanged;
                maskable.Changed.Event += OnChanged;
                canvasScaler.Changed.Event += OnChanged;
                aspectRatioFitter.Changed.Event += OnChanged;
                contentSizeFitter.Changed.Event += OnChanged;
                layoutElement.Changed.Event += OnChanged;
                layoutGroup.Changed.Event += OnChanged;
                scrollRect.Changed.Event += OnChanged;
            }
        }
    }
}
