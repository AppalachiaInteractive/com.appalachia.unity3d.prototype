using System;
using Appalachia.CI.Integration.Attributes;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugConditions.Model
{
    [Serializable, DoNotReorderFields]
    public struct DebugCondition
    {
        #region Fields and Autoproperties

        [PropertyOrder(0)]
        [PropertyTooltip("Variable to compare against")]
        public DebugEvaluationTarget evaluationTarget;

        [PropertyOrder(10)]
        [PropertyTooltip("Comparer operator to use")]
        public ComparisonType comparisonType;

        [PropertyOrder(20)]
        [PropertyTooltip("Value to compare against the chosen variable")]
        public float value;

        #endregion
    }
}
