using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Settings;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Shader;
using Appalachia.Utility.Timing;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Fps
{
    public class RuntimeGraphFpsGraph : RuntimeGraphInstance<RuntimeGraphFpsGraph, RuntimeGraphFpsManager,
        RuntimeGraphFpsMonitor, RuntimeGraphFpsText, RuntimeGraphFpsSettings>
    {
        #region Fields and Autoproperties

        [SerializeField] private Image m_imageGraph;

        [SerializeField] private UnityEngine.Shader ShaderFull;
        [SerializeField] private UnityEngine.Shader ShaderLight;

        private GraphShader m_shaderGraph;

        private int[] m_fpsArray;

        private int m_highestFps;

        #endregion

        /// <inheritdoc />
        public override GameObject graphParent => m_imageGraph.gameObject;

        /// <inheritdoc />
        protected override bool ShouldUpdate => true;

        /// <inheritdoc />
        protected override RuntimeGraphFpsSettings settings => allSettings.fps;

        /// <inheritdoc />
        public override void InitializeParameters()
        {
            using (_PRF_InitializeGraph.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void AfterInitialization()
        {
            using (_PRF_AfterInitialization.Auto())
            {
                if (m_shaderGraph == null)
                {
                    m_shaderGraph = new GraphShader { image = m_imageGraph };
                }

                base.AfterInitialization();
            }
        }

        /// <inheritdoc />
        protected override void CreatePoints()
        {
            using (_PRF_CreatePoints.Auto())
            {
                InitializePointArrays();

                var updatedPoints = false;

                if ((m_shaderGraph.shaderArrayValues == null) ||
                    (m_fpsArray.Length != settings.GraphResolution))
                {
                    updatedPoints = true;

                    m_fpsArray = new int[settings.GraphResolution];
                    m_shaderGraph.shaderArrayValues = new float[settings.GraphResolution];

                    for (var i = 0; i < settings.GraphResolution; i++)
                    {
                        m_shaderGraph.shaderArrayValues[i] = 0;
                    }
                }

                var updatedColors = false;

                if (m_shaderGraph.goodColor != settings.goodFpsColor)
                {
                    m_shaderGraph.goodColor = settings.goodFpsColor;
                    updatedColors = true;
                }

                if (m_shaderGraph.cautionColor != settings.cautionFpsColor)
                {
                    m_shaderGraph.cautionColor = settings.cautionFpsColor;
                    updatedColors = true;
                }

                if (m_shaderGraph.criticalColor != settings.criticalFpsColor)
                {
                    m_shaderGraph.criticalColor = settings.criticalFpsColor;
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

        /// <inheritdoc />
        protected override void UpdateGraph()
        {
            using (_PRF_UpdateGraph.Auto())
            {
                var fps = (short)(1 / CoreClock.Instance.UnscaledDeltaTime);

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

                if (m_shaderGraph.shaderArrayValues == null)
                {
                    m_fpsArray = new int[settings.GraphResolution];
                    m_shaderGraph.shaderArrayValues = new float[settings.GraphResolution];
                }

                for (var i = 0; i <= (settings.GraphResolution - 1); i++)
                {
                    m_shaderGraph.shaderArrayValues[i] = m_fpsArray[i] / (float)m_highestFps;
                }

                // Update the material values

                m_shaderGraph.UpdatePoints();

                if (monitor is not null)
                {
                    m_shaderGraph.average = monitor.AverageFPS / (float)m_highestFps;
                }

                m_shaderGraph.UpdateAverage();

                m_shaderGraph.goodThreshold = (float)settings.goodFpsThreshold / m_highestFps;
                m_shaderGraph.cautionThreshold = (float)settings.cautionFpsThreshold / m_highestFps;
                m_shaderGraph.UpdateThresholds();
            }
        }

        protected void InitializePointArrays()
        {
            using (_PRF_InitializePointArrays.Auto())
            {
                m_shaderGraph.arrayMaxSize = GraphShader.ARRAY_MAX_SIZE;

                if (m_shaderGraph.image.material == null)
                {
                    m_shaderGraph.image.material = new Material(ShaderFull);
                }

                m_shaderGraph.InitializeShader();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        private static readonly ProfilerMarker _PRF_CreatePoints =
            new ProfilerMarker(_PRF_PFX + nameof(CreatePoints));

        private static readonly ProfilerMarker _PRF_InitializeGraph =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        private static readonly ProfilerMarker _PRF_InitializePointArrays =
            new ProfilerMarker(_PRF_PFX + nameof(InitializePointArrays));

        private static readonly ProfilerMarker _PRF_UpdateGraph =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateGraph));

        #endregion
    }
}
