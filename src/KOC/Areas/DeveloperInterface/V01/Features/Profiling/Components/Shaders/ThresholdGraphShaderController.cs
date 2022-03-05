using Appalachia.Core.Objects.Initialization;
using Appalachia.Globals.Shading.Properties;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Components.Shaders
{
    public sealed class ThresholdGraphShaderController : FilledGraphShaderController<ThresholdGraphShaderController>
    {
        #region Constants and Static Readonly

        public const string SHADER_NAME = SHADER_BASE_NAME + "threshold";

        #endregion

        #region Fields and Autoproperties

        public ShaderColorProperty _CautionColor;
        public ShaderFloatProperty _CautionThreshold;
        public ShaderColorProperty _CriticalColor;
        public ShaderColorProperty _GoodColor;
        public ShaderFloatProperty _GoodThreshold;
        public ShaderFloatProperty _ThresholdThickness;

        #endregion

        public override string ShaderName => SHADER_NAME;

        public override void ReadFromMaterial()
        {
            using (_PRF_ReadFromMaterial.Auto())
            {
                base.ReadFromMaterial();
                
                _CautionColor.InitializeFromMaterial(ControlledMaterial);
                _CautionThreshold.InitializeFromMaterial(ControlledMaterial);
                _CriticalColor.InitializeFromMaterial(ControlledMaterial);
                _GoodColor.InitializeFromMaterial(ControlledMaterial);
                _GoodThreshold.InitializeFromMaterial(ControlledMaterial);
                _ThresholdThickness.InitializeFromMaterial(ControlledMaterial);
            }
        }

        /// <inheritdoc />
        public override void SubscribeToPropertyChanges()
        {
            base.SubscribeToPropertyChanges();

            ValidateProperty(_CautionColor,       nameof(_CautionColor));
            ValidateProperty(_CautionThreshold,   nameof(_CautionThreshold));
            ValidateProperty(_CriticalColor,      nameof(_CriticalColor));
            ValidateProperty(_GoodColor,          nameof(_GoodColor));
            ValidateProperty(_GoodThreshold,      nameof(_GoodThreshold));
            ValidateProperty(_ThresholdThickness, nameof(_ThresholdThickness));

            _CautionColor.ValueChanged.Event += UpdateShaderProperty;
            _CautionThreshold.ValueChanged.Event += UpdateShaderProperty;
            _CriticalColor.ValueChanged.Event += UpdateShaderProperty;
            _GoodColor.ValueChanged.Event += UpdateShaderProperty;
            _GoodThreshold.ValueChanged.Event += UpdateShaderProperty;
            _ThresholdThickness.ValueChanged.Event += UpdateShaderProperty;
        }

        protected override void InitializeFields(Initializer initializer)
        {
            using (_PRF_InitializeFields.Auto())
            {
                base.InitializeFields(initializer);

                _CautionColor = new ShaderColorProperty(nameof(_CautionColor), Color.yellow,this);
                _CautionThreshold = new ShaderFloatProperty(nameof(_CautionThreshold), .5f,this);
                _CriticalColor = new ShaderColorProperty(nameof(_CriticalColor), Color.red, this);
                _GoodColor = new ShaderColorProperty(nameof(_GoodColor),         Color.green, this);
                _GoodThreshold = new ShaderFloatProperty(nameof(_GoodThreshold),           .75f, this);
                _ThresholdThickness = new ShaderFloatProperty(nameof(_ThresholdThickness), .01f, this);
            }
        }

        protected override Color TestColor => Color.white;

        protected override Color TestAverageColor => Color.cyan;
    }
}
