using Appalachia.Core.Objects.Initialization;
using Appalachia.Globals.Shading.Properties;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Components.Shaders
{
    /// <summary>
    ///     This class communicates directly with the shader to draw the graphs. Performance here is very important
    ///     to reduce as much overhead as possible, as we are updating hundreds of values every frame.
    /// </summary>
    public abstract class FilledGraphShaderController<T> : GraphShaderController<T>
        where T : FilledGraphShaderController<T>
    {
        #region Fields and Autoproperties

        public ShaderFloatProperty HeightOpacityMultiplier;

        #endregion

        protected override bool CanTest => true;

        public override void ReadFromMaterial()
        {
            using (_PRF_ReadFromMaterial.Auto())
            {
                base.ReadFromMaterial();

                HeightOpacityMultiplier.InitializeFromMaterial(ControlledMaterial);
            }
        }

        /// <inheritdoc />
        public override void SubscribeToPropertyChanges()
        {
            using (_PRF_SubscribeToPropertyChanges.Auto())
            {
                base.SubscribeToPropertyChanges();

                ValidateProperty(HeightOpacityMultiplier, nameof(HeightOpacityMultiplier));

                HeightOpacityMultiplier.ValueChanged.Event += UpdateShaderProperty;
            }
        }

        protected override void InitializeFields(Initializer initializer)
        {
            using (_PRF_InitializeFields.Auto())
            {
                base.InitializeFields(initializer);

                HeightOpacityMultiplier ??= new ShaderFloatProperty(nameof(HeightOpacityMultiplier), this);
            }
        }

        protected override void Test()
        {
            using (_PRF_Test.Auto())
            {
                base.Test();

                HeightOpacityMultiplier.Value = .3f;
            }
        }
    }
}
