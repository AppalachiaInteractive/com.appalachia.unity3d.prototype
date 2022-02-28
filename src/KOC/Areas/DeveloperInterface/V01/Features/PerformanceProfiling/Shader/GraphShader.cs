using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Shader
{
    /// <summary>
    ///     This class communicates directly with the shader to draw the graphs. Performance here is very important
    ///     to reduce as much overhead as possible, as we are updating hundreds of values every frame.
    /// </summary>
    public class GraphShader
    {
        #region Constants and Static Readonly

        public const int ARRAY_MAX_SIZE = 512;
        private const string ARRAY_LENGTH_NAME = "GraphValues_Length";

        private const string ARRAY_NAME = "GraphValues"; // The name of the array

        #endregion

        #region Static Fields and Autoproperties

        private static int? _graphValues;
        private static int? _graphValuesLength;

        #endregion

        #region Fields and Autoproperties

        public Color cautionColor = Color.white;
        public Color criticalColor = Color.white;

        public Color goodColor = Color.white;

        public float average = 0;
        public float cautionThreshold = 0;

        public float goodThreshold = 0;

        public float[] shaderArrayValues;

        public Image image = null;

        public int arrayMaxSize = ARRAY_MAX_SIZE;
        private int _averagePropertyId;
        private int _cautionColorPropertyId;
        private int _cautionThresholdPropertyId;
        private int _criticalColorPropertyId;

        private int _goodColorPropertyId;

        private int _goodThresholdPropertyId;

        #endregion

        private static int ArrayLengthID
        {
            get
            {
                if (!_graphValuesLength.HasValue)
                {
                    _graphValuesLength = UnityEngine.Shader.PropertyToID(ARRAY_LENGTH_NAME);
                }

                return _graphValuesLength.Value;
            }
        }

        private static int ArrayNameID
        {
            get
            {
                if (!_graphValues.HasValue)
                {
                    _graphValues = UnityEngine.Shader.PropertyToID(ARRAY_NAME);
                }

                return _graphValues.Value;
            }
        }

        /// <summary>
        ///     This is done to avoid a design problem that arrays in shaders have,
        ///     and should be called before initializing any shader graph.
        ///     The first time that you use initialize an array, the size of the array in the shader is fixed.
        ///     This is why sometimes you will get a warning saying that the array size will be capped.
        ///     It shouldn't generate any issues, but in the worst case scenario just reset the Unity Editor
        ///     (if for some reason the shaders reload).
        ///     I also cache the Property IDs, that make access faster to modify shader parameters.
        /// </summary>
        public void InitializeShader()
        {
            if (_averagePropertyId == 0)
            {
                image.material.SetFloatArray(ArrayNameID, new float[arrayMaxSize]);

                _averagePropertyId = UnityEngine.Shader.PropertyToID("Average");

                _goodThresholdPropertyId = UnityEngine.Shader.PropertyToID("_GoodThreshold");
                _cautionThresholdPropertyId = UnityEngine.Shader.PropertyToID("_CautionThreshold");

                _goodColorPropertyId = UnityEngine.Shader.PropertyToID("_GoodColor");
                _cautionColorPropertyId = UnityEngine.Shader.PropertyToID("_CautionColor");
                _criticalColorPropertyId = UnityEngine.Shader.PropertyToID("_CriticalColor");
            }
        }

        /// <summary>
        ///     Updates the material linked with this shader graph  with the values in the float[] array.
        /// </summary>
        public void UpdateArray()
        {
            image.material.SetInt(ArrayLengthID, shaderArrayValues.Length);
        }

        /// <summary>
        ///     Updates the average parameter in the material.
        /// </summary>
        public void UpdateAverage()
        {
            image.material.SetFloat(_averagePropertyId, average);
        }

        /// <summary>
        ///     Updates the colors in the material.
        /// </summary>
        public void UpdateColors()
        {
            image.material.SetColor(_goodColorPropertyId,     goodColor);
            image.material.SetColor(_cautionColorPropertyId,  cautionColor);
            image.material.SetColor(_criticalColorPropertyId, criticalColor);
        }

        /// <summary>
        ///     Updates the points in the graph with the set array of values.
        /// </summary>
        public void UpdatePoints()
        {
            // Requires an array called "name"
            // and another one called "name_Length"

            image.material.SetFloatArray(ARRAY_NAME, shaderArrayValues);
        }

        /// <summary>
        ///     Updates the thresholds in the material.
        /// </summary>
        public void UpdateThresholds()
        {
            image.material.SetFloat(_goodThresholdPropertyId,    goodThreshold);
            image.material.SetFloat(_cautionThresholdPropertyId, cautionThreshold);
        }
    }
}
