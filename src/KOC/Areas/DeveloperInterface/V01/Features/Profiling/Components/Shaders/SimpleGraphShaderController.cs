using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Components.Shaders
{
    public sealed class SimpleGraphShaderController : FilledGraphShaderController<SimpleGraphShaderController>
    {
        #region Constants and Static Readonly

        public const string SHADER_NAME = SHADER_BASE_NAME + "simple";

        #endregion

        public override string ShaderName => SHADER_NAME;

        protected override Color TestAverageColor => Color.magenta;

        protected override Color TestColor => Color.blue;
    }
}
