using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Components.Shaders
{
    public sealed class LineGraphShaderController : GraphShaderController<LineGraphShaderController>
    {
        #region Constants and Static Readonly

        public const string SHADER_NAME = SHADER_BASE_NAME + "line";

        #endregion

        public override string ShaderName => SHADER_NAME;

        protected override Color TestAverageColor => Color.clear;

        protected override Color TestColor => Color.green;
    }
}
