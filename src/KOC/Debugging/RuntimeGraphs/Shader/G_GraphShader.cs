using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Shader
{
    /// <summary>
    ///     This class communicates directly with the shader to draw the graphs. Performance here is very important
    ///     to reduce as much overhead as possible, as we are updating hundreds of values every frame.
    /// </summary>
    public class G_GraphShader
    {
        #region Constants and Static Readonly

        public const int ArrayMaxSizeFull = 512;
        public const int ArrayMaxSizeLight = 128;

        #endregion

        #region Fields and Autoproperties

        public Color CautionColor = Color.white;
        public Color CriticalColor = Color.white;

        public Color GoodColor = Color.white;

        public float Average = 0;
        public float CautionThreshold = 0;

        public float GoodThreshold = 0;

        public float[] ShaderArrayValues;

        public Image Image = null;

        public int ArrayMaxSize = ArrayMaxSizeFull;

        private readonly string Name = "GraphValues"; // The name of the array
        private readonly string Name_Length = "GraphValues_Length";
        private int m_averagePropertyId;
        private int m_cautionColorPropertyId;
        private int m_cautionThresholdPropertyId;
        private int m_criticalColorPropertyId;

        private int m_goodColorPropertyId;

        private int m_goodThresholdPropertyId;

        #endregion

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
            if (m_averagePropertyId == 0)
            {
                Image.material.SetFloatArray(Name, new float[ArrayMaxSize]);

                m_averagePropertyId = UnityEngine.Shader.PropertyToID("Average");

                m_goodThresholdPropertyId = UnityEngine.Shader.PropertyToID("_GoodThreshold");
                m_cautionThresholdPropertyId = UnityEngine.Shader.PropertyToID("_CautionThreshold");

                m_goodColorPropertyId = UnityEngine.Shader.PropertyToID("_GoodColor");
                m_cautionColorPropertyId = UnityEngine.Shader.PropertyToID("_CautionColor");
                m_criticalColorPropertyId = UnityEngine.Shader.PropertyToID("_CriticalColor");
            }
        }

        /// <summary>
        ///     Updates the material linked with this shader graph  with the values in the float[] array.
        /// </summary>
        public void UpdateArray()
        {
            Image.material.SetInt(Name_Length, ShaderArrayValues.Length);
        }

        /// <summary>
        ///     Updates the average parameter in the material.
        /// </summary>
        public void UpdateAverage()
        {
            Image.material.SetFloat(m_averagePropertyId, Average);
        }

        /// <summary>
        ///     Updates the colors in the material.
        /// </summary>
        public void UpdateColors()
        {
            Image.material.SetColor(m_goodColorPropertyId,     GoodColor);
            Image.material.SetColor(m_cautionColorPropertyId,  CautionColor);
            Image.material.SetColor(m_criticalColorPropertyId, CriticalColor);
        }

        /// <summary>
        ///     Updates the points in the graph with the set array of values.
        /// </summary>
        public void UpdatePoints()
        {
            // Requires an array called "name"
            // and another one called "name_Length"

            Image.material.SetFloatArray(Name, ShaderArrayValues);
        }

        /// <summary>
        ///     Updates the thresholds in the material.
        /// </summary>
        public void UpdateThresholds()
        {
            Image.material.SetFloat(m_goodThresholdPropertyId,    GoodThreshold);
            Image.material.SetFloat(m_cautionThresholdPropertyId, CautionThreshold);
        }
    }
}
