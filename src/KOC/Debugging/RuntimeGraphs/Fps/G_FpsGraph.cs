using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Shader;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Fps
{
    public class G_FpsGraph : RuntimeGraphInstance<G_FpsGraph, G_FpsManager, G_FpsMonitor, G_FpsText,
        RuntimeGraphFpsSettings>
    {
        #region Fields and Autoproperties

        [SerializeField] private Image m_imageGraph;

        [SerializeField] private UnityEngine.Shader ShaderFull;
        [SerializeField] private UnityEngine.Shader ShaderLight;

        private G_GraphShader m_shaderGraph;

        private int[] m_fpsArray;

        private int m_highestFps;

        #endregion

        public override GameObject graphParent => m_imageGraph.gameObject;

        protected override bool ShouldUpdate => true;

        protected override RuntimeGraphFpsSettings settings => allSettings.fps;

        public override void InitializeParameters()
        {
            using (_PRF_InitializeGraph.Auto())
            {
                if (m_shaderGraph == null)
                {
                    return;
                }

                switch (allSettings.general.graphyMode)
                {
                    case Mode.FULL:
                        m_shaderGraph.ArrayMaxSize = G_GraphShader.ArrayMaxSizeFull;

                        if (m_shaderGraph.Image.material == null)
                        {
                            m_shaderGraph.Image.material = new Material(ShaderFull);
                        }

                        break;

                    case Mode.LIGHT:
                        m_shaderGraph.ArrayMaxSize = G_GraphShader.ArrayMaxSizeLight;

                        if (m_shaderGraph.Image.material == null)
                        {
                            m_shaderGraph.Image.material = new Material(ShaderLight);
                        }

                        break;
                }

                m_shaderGraph.InitializeShader();
            }
        }

        protected override void AfterInitialize()
        {
            using (_PRF_AfterInitialize.Auto())
            {
                if (m_shaderGraph == null)
                {
                    m_shaderGraph = new G_GraphShader { Image = m_imageGraph };
                }
            }
        }

        protected override void CreatePoints()
        {
            using (_PRF_CreatePoints.Auto())
            {
                var updatedPoints = false;

                if ((m_shaderGraph.ShaderArrayValues == null) ||
                    (m_fpsArray.Length != settings.GraphResolution))
                {
                    updatedPoints = true;

                    m_fpsArray = new int[settings.GraphResolution];
                    m_shaderGraph.ShaderArrayValues = new float[settings.GraphResolution];

                    for (var i = 0; i < settings.GraphResolution; i++)
                    {
                        m_shaderGraph.ShaderArrayValues[i] = 0;
                    }
                }

                var updatedColors = false;

                if (m_shaderGraph.GoodColor != settings.goodFpsColor)
                {
                    m_shaderGraph.GoodColor = settings.goodFpsColor;
                    updatedColors = true;
                }

                if (m_shaderGraph.CautionColor != settings.cautionFpsColor)
                {
                    m_shaderGraph.CautionColor = settings.cautionFpsColor;
                    updatedColors = true;
                }

                if (m_shaderGraph.CriticalColor != settings.criticalFpsColor)
                {
                    m_shaderGraph.CriticalColor = settings.criticalFpsColor;
                    updatedColors = true;
                }

                if (updatedColors)
                {
                    m_shaderGraph.UpdateColors();
                }

                if (updatedPoints)
                {
                    m_shaderGraph.UpdateArray();
                }
            }
        }

        protected override void UpdateGraph()
        {
            using (_PRF_UpdateGraph.Auto())
            {
                var fps = (short)(1 / Time.unscaledDeltaTime);

                var currentMaxFps = 0;

                for (var i = 0; i <= (settings.GraphResolution - 1); i++)
                {
                    if (i >= (settings.GraphResolution - 1))
                    {
                        m_fpsArray[i] = fps;
                    }
                    else
                    {
                        m_fpsArray[i] = m_fpsArray[i + 1];
                    }

                    // Store the highest fps to use as the highest point in the graph

                    if (currentMaxFps < m_fpsArray[i])
                    {
                        currentMaxFps = m_fpsArray[i];
                    }
                }

                m_highestFps = (m_highestFps < 1) || (m_highestFps <= currentMaxFps)
                    ? currentMaxFps
                    : m_highestFps - 1;

                m_highestFps = m_highestFps > 0 ? m_highestFps : 1;

                if (m_shaderGraph.ShaderArrayValues == null)
                {
                    m_fpsArray = new int[settings.GraphResolution];
                    m_shaderGraph.ShaderArrayValues = new float[settings.GraphResolution];
                }

                for (var i = 0; i <= (settings.GraphResolution - 1); i++)
                {
                    m_shaderGraph.ShaderArrayValues[i] = m_fpsArray[i] / (float)m_highestFps;
                }

                // Update the material values

                m_shaderGraph.UpdatePoints();

                if (monitor is not null)
                {
                    m_shaderGraph.Average = monitor.AverageFPS / (float)m_highestFps;
                }

                m_shaderGraph.UpdateAverage();

                m_shaderGraph.GoodThreshold = (float)settings.goodFpsThreshold / m_highestFps;
                m_shaderGraph.CautionThreshold = (float)settings.cautionFpsThreshold / m_highestFps;
                m_shaderGraph.UpdateThresholds();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(G_FpsGraph) + ".";

        private static readonly ProfilerMarker _PRF_InitializeGraph =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        private static readonly ProfilerMarker _PRF_AfterInitialize =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialize));

        private static readonly ProfilerMarker _PRF_CreatePoints =
            new ProfilerMarker(_PRF_PFX + nameof(CreatePoints));

        private static readonly ProfilerMarker _PRF_UpdateGraph =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateGraph));

        #endregion
    }
}
