using Appalachia.Core.Objects.Initialization;
using Appalachia.Globals.Shading;
using Appalachia.Globals.Shading.Properties;
using Appalachia.Utility.Events;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Components.Shaders
{
    /// <summary>
    ///     This class communicates directly with the shader to draw the graphs. Performance here is very important
    ///     to reduce as much overhead as possible, as we are updating hundreds of values every frame.
    /// </summary>
    public abstract class GraphShaderController<T> : AppalachiaShaderController<T>
        where T : GraphShaderController<T>
    {
        #region Constants and Static Readonly

        public const int ARRAY_MAX_SIZE = 512;
        protected const string SHADER_BASE_NAME = "appalachia/areas/developerinterface/v01/shaders/graphs/";

        #endregion

        #region Fields and Autoproperties

        public ShaderFloatProperty Average;
        public ShaderFloatProperty PointSize;
        public ShaderColorProperty _Color;
        public ShaderFloatArrayProperty GraphValues;
        public ShaderIntProperty GraphValues_Length;
        public ShaderTexture2DProperty _MainTex;
        public ShaderKeywordProperty PixelSnap;
        public ShaderKeywordProperty Interpolate;
        public ShaderColorProperty AverageColor;
        public ShaderFloatProperty AverageThickness;
        public ShaderFloatProperty EdgeFadeWidth;

        #endregion

        protected abstract Color TestAverageColor { get; }

        protected abstract Color TestColor { get; }

        protected override bool CanTest => true;

        public override void ReadFromMaterial()
        {
            using (_PRF_ReadFromMaterial.Auto())
            {
                base.ReadFromMaterial();

                Average.InitializeFromMaterial(ControlledMaterial);
                PointSize.InitializeFromMaterial(ControlledMaterial);
                GraphValues.InitializeFromMaterial(ControlledMaterial);
                GraphValues_Length.InitializeFromMaterial(ControlledMaterial);
                _MainTex.InitializeFromMaterial(ControlledMaterial);
                PixelSnap.InitializeFromMaterial(ControlledMaterial);
                Interpolate.InitializeFromMaterial(ControlledMaterial);
                AverageColor.InitializeFromMaterial(ControlledMaterial);
                AverageThickness.InitializeFromMaterial(ControlledMaterial);
                EdgeFadeWidth.InitializeFromMaterial(ControlledMaterial);
            }
        }

        /// <inheritdoc />
        public override void SubscribeToPropertyChanges()
        {
            using (_PRF_SubscribeToPropertyChanges.Auto())
            {
                ValidateProperty(Average,            nameof(Average));
                ValidateProperty(PointSize,          nameof(PointSize));
                ValidateProperty(_Color,             nameof(_Color));
                ValidateProperty(GraphValues,        nameof(GraphValues));
                ValidateProperty(GraphValues_Length, nameof(GraphValues_Length));
                ValidateProperty(_MainTex,           nameof(_MainTex));
                ValidateProperty(PixelSnap,          nameof(PixelSnap));
                ValidateProperty(Interpolate,        nameof(Interpolate));
                ValidateProperty(AverageColor,       nameof(AverageColor));
                ValidateProperty(AverageThickness,   nameof(AverageThickness));
                ValidateProperty(EdgeFadeWidth,      nameof(EdgeFadeWidth));

                Average.ValueChanged.Event += UpdateShaderProperty;
                PointSize.ValueChanged.Event += UpdateShaderProperty;
                _Color.ValueChanged.Event += UpdateShaderProperty;
                GraphValues.ValueChanged.Event += UpdateShaderProperty;
                GraphValues_Length.ValueChanged.Event += UpdateShaderProperty;
                _MainTex.ValueChanged.Event += UpdateShaderProperty;
                PixelSnap.ValueChanged.Event += UpdateShaderProperty;
                Interpolate.ValueChanged.Event += UpdateShaderProperty;
                AverageColor.ValueChanged.Event += UpdateShaderProperty;
                AverageThickness.ValueChanged.Event += UpdateShaderProperty;
                EdgeFadeWidth.ValueChanged.Event += UpdateShaderProperty;

                GraphValues.ValueChanged.Event += GraphValuesChanged;
            }
        }

        protected override void InitializeFields(Initializer initializer)
        {
            using (_PRF_InitializeFields.Auto())
            {
                Average ??= new ShaderFloatProperty(nameof(Average),     this);
                PointSize ??= new ShaderFloatProperty(nameof(PointSize), this);
                _Color ??= new ShaderColorProperty(nameof(_Color), Color.white, this);
                GraphValues ??= new ShaderFloatArrayProperty(nameof(GraphValues), new float[ARRAY_MAX_SIZE], this);
                GraphValues_Length ??= new ShaderIntProperty(nameof(GraphValues_Length), ARRAY_MAX_SIZE, this);
                _MainTex ??= new ShaderTexture2DProperty(nameof(_MainTex), this);
                PixelSnap ??= new ShaderKeywordProperty(nameof(PixelSnap),     true, this);
                Interpolate ??= new ShaderKeywordProperty(nameof(Interpolate), true, this);
                AverageColor ??= new ShaderColorProperty(nameof(AverageColor), Color.white, this);
                AverageThickness ??= new ShaderFloatProperty(nameof(AverageThickness), .01f, this);
                EdgeFadeWidth ??= new ShaderFloatProperty(nameof(EdgeFadeWidth),       .05f, this);
            }
        }

        protected override void Test()
        {
            using (_PRF_Test.Auto())
            {
                base.Test();

                var testValues = new float[ARRAY_MAX_SIZE];

                var offsetMultiplier = 2f;
                var offsets = new[] { .0025f, .005f, .006f, .0075f, .005f, .0025f, .001f };

                var signSwitch = 24;

                for (var i = 0; i < testValues.Length; i++)
                {
                    var newValue = Average.Value;

                    var sign = ((i / signSwitch) % 2) == 0 ? 1 : -1;
                    var offsetIndex = i % offsets.Length;

                    var offset = offsets[offsetIndex] * sign * offsetMultiplier;

                    if ((i % 49) == 0)
                    {
                        offset += -.25f;
                    }

                    if ((i % 51) == 0)
                    {
                        offset += .25f;
                    }

                    testValues[i] = newValue + offset;
                }

                PointSize.Value = .04f;
                _Color.Value = TestColor;
                GraphValues.Value = testValues;
                GraphValues_Length.Value = testValues.Length;
                PixelSnap.Value = true;
                Interpolate.Value = true;
                AverageColor.Value = TestAverageColor;
                AverageThickness.Value = .02f;
                EdgeFadeWidth.Value = .03f;
            }
        }

        private void GraphValuesChanged(AppaEvent<IShaderProperty>.Args args)
        {
            using (_PRF_GraphValuesChanged.Auto())
            {
                if (args.value is ShaderFloatArrayProperty graphValue)
                {
                    GraphValues_Length.Value = graphValue.Value.Length;
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GraphValuesChanged =
            new ProfilerMarker(_PRF_PFX + nameof(GraphValuesChanged));

        #endregion
    }
}
