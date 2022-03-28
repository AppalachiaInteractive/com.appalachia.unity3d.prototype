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

        [PropertyRange(0f, 1f)]
        [BoxGroup("Modifiers")]
        public float globalAlpha;

        [BoxGroup("Modifiers")] public OverridableColor inactive;
        [BoxGroup("General")] public OverridableColor rectTransform;
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

        [BoxGroup("Appalachia")] public OverridableColor appaCanvasScaler;
        [BoxGroup("Appalachia")] public OverridableColor feature;
        [BoxGroup("Appalachia")] public OverridableColor widget;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(globalAlpha), () => { globalAlpha = 1.0f; });

                initializer.Do(
                    this,
                    nameof(inactive),
                    inactive == default,
                    () => { inactive = new OverridableColor(true, Colors.White.ScaleA(.25f)); }
                );

                initializer.Do(
                    this,
                    nameof(rectTransform),
                    rectTransform == default,
                    () => { rectTransform = new OverridableColor(true, Colors.PaleTurquoise4); }
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

                initializer.Do(
                    this,
                    nameof(appaCanvasScaler),
                    appaCanvasScaler == default,
                    () => { appaCanvasScaler = new OverridableColor(true, Colors.Orange1); }
                );

                initializer.Do(
                    this,
                    nameof(feature),
                    feature == default,
                    () => { feature = new OverridableColor(true, Colors.Orange1); }
                );

                initializer.Do(
                    this,
                    nameof(widget),
                    widget == default,
                    () => { widget = new OverridableColor(true, Colors.Orange1); }
                );
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(RectVisualizerFeature target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);
                
                inactive.SubscribeToChanges(OnChanged);
                rectTransform.SubscribeToChanges(OnChanged);
                canvas.SubscribeToChanges(OnChanged);
                screenSpace.SubscribeToChanges(OnChanged);
                worldSpace.SubscribeToChanges(OnChanged);
                mask.SubscribeToChanges(OnChanged);
                selectable.SubscribeToChanges(OnChanged);
                button.SubscribeToChanges(OnChanged);
                dropdown.SubscribeToChanges(OnChanged);
                inputField.SubscribeToChanges(OnChanged);
                slider.SubscribeToChanges(OnChanged);
                toggle.SubscribeToChanges(OnChanged);
                scrollbar.SubscribeToChanges(OnChanged);
                graphic.SubscribeToChanges(OnChanged);
                text.SubscribeToChanges(OnChanged);
                image.SubscribeToChanges(OnChanged);
                raycastTarget.SubscribeToChanges(OnChanged);
                maskable.SubscribeToChanges(OnChanged);
                canvasScaler.SubscribeToChanges(OnChanged);
                aspectRatioFitter.SubscribeToChanges(OnChanged);
                contentSizeFitter.SubscribeToChanges(OnChanged);
                layoutElement.SubscribeToChanges(OnChanged);
                layoutGroup.SubscribeToChanges(OnChanged);
                scrollRect.SubscribeToChanges(OnChanged);

                appaCanvasScaler.SubscribeToChanges(OnChanged);
                feature.SubscribeToChanges(OnChanged);
                widget.SubscribeToChanges(OnChanged);
            }
        }
        
        

        /// <inheritdoc />
        protected override void UnsuspendResponsiveComponents(RectVisualizerFeature target)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
                base.UnsuspendResponsiveComponents(target);
                
                inactive.UnsuspendChanges();
                rectTransform.UnsuspendChanges();
                canvas.UnsuspendChanges();
                screenSpace.UnsuspendChanges();
                worldSpace.UnsuspendChanges();
                mask.UnsuspendChanges();
                selectable.UnsuspendChanges();
                button.UnsuspendChanges();
                dropdown.UnsuspendChanges();
                inputField.UnsuspendChanges();
                slider.UnsuspendChanges();
                toggle.UnsuspendChanges();
                scrollbar.UnsuspendChanges();
                graphic.UnsuspendChanges();
                text.UnsuspendChanges();
                image.UnsuspendChanges();
                raycastTarget.UnsuspendChanges();
                maskable.UnsuspendChanges();
                canvasScaler.UnsuspendChanges();
                aspectRatioFitter.UnsuspendChanges();
                contentSizeFitter.UnsuspendChanges();
                layoutElement.UnsuspendChanges();
                layoutGroup.UnsuspendChanges();
                scrollRect.UnsuspendChanges();

                appaCanvasScaler.UnsuspendChanges();
                feature.UnsuspendChanges();
                widget.UnsuspendChanges();
            }
        }
        
        

        /// <inheritdoc />
        protected override void SuspendResponsiveComponents(RectVisualizerFeature target)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(target);
                
                inactive.SuspendChanges();
                rectTransform.SuspendChanges();
                canvas.SuspendChanges();
                screenSpace.SuspendChanges();
                worldSpace.SuspendChanges();
                mask.SuspendChanges();
                selectable.SuspendChanges();
                button.SuspendChanges();
                dropdown.SuspendChanges();
                inputField.SuspendChanges();
                slider.SuspendChanges();
                toggle.SuspendChanges();
                scrollbar.SuspendChanges();
                graphic.SuspendChanges();
                text.SuspendChanges();
                image.SuspendChanges();
                raycastTarget.SuspendChanges();
                maskable.SuspendChanges();
                canvasScaler.SuspendChanges();
                aspectRatioFitter.SuspendChanges();
                contentSizeFitter.SuspendChanges();
                layoutElement.SuspendChanges();
                layoutGroup.SuspendChanges();
                scrollRect.SuspendChanges();

                appaCanvasScaler.SuspendChanges();
                feature.SuspendChanges();
                widget.SuspendChanges();
            }
        }

        /// <inheritdoc />
        protected override void OnApply(RectVisualizerFeature functionality)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(functionality);
            }
        }
    }
}
