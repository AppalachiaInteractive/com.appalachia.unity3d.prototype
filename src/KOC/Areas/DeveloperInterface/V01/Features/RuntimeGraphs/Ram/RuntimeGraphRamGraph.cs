using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Shader;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Ram
{
    public class RuntimeGraphRamGraph : RuntimeGraphInstance<RuntimeGraphRamGraph, RuntimeGraphRamManager,
        RuntimeGraphRamMonitor, RuntimeGraphRamText, RuntimeGraphRamSettings>
    {
        #region Fields and Autoproperties

        [SerializeField] private Image m_imageAllocated;
        [SerializeField] private Image m_imageReserved;
        [SerializeField] private Image m_imageMono;

        [SerializeField] private UnityEngine.Shader ShaderFull;
        [SerializeField] private UnityEngine.Shader ShaderLight;

        private GraphShader m_shaderGraphAllocated;
        private GraphShader m_shaderGraphReserved;
        private GraphShader m_shaderGraphMono;

        private float[] m_allocatedArray;
        private float[] m_reservedArray;
        private float[] m_monoArray;

        private float m_highestMemory;

        #endregion

        /// <inheritdoc />
        public override GameObject graphParent => m_imageAllocated.gameObject;

        /// <inheritdoc />
        protected override bool ShouldUpdate => true;

        /// <inheritdoc />
        protected override RuntimeGraphRamSettings settings => allSettings.ram;

        /// <inheritdoc />
        public override void InitializeParameters()
        {
            using (_PRF_SyncParameters.Auto())
            {
                base.InitializeParameters();

                if ((m_shaderGraphAllocated == null) ||
                    (m_shaderGraphReserved == null) ||
                    (m_shaderGraphMono == null))
                {
                    return;
                }

                switch (allSettings.general.runtimeGraphMode)
                {
                    case Mode.FULL:
                        m_shaderGraphAllocated.arrayMaxSize = GraphShader.ARRAY_MAX_SIZE_FULL;
                        m_shaderGraphReserved.arrayMaxSize = GraphShader.ARRAY_MAX_SIZE_FULL;
                        m_shaderGraphMono.arrayMaxSize = GraphShader.ARRAY_MAX_SIZE_FULL;

                        if (m_shaderGraphAllocated.image.material == null)
                        {
                            m_shaderGraphAllocated.image.material = new Material(ShaderFull);
                        }

                        if (m_shaderGraphReserved.image.material == null)
                        {
                            m_shaderGraphReserved.image.material = new Material(ShaderFull);
                        }

                        if (m_shaderGraphMono.image.material == null)
                        {
                            m_shaderGraphMono.image.material = new Material(ShaderFull);
                        }

                        break;

                    case Mode.LIGHT:
                        m_shaderGraphAllocated.arrayMaxSize = GraphShader.ARRAY_MAX_SIZE_LIGHT;
                        m_shaderGraphReserved.arrayMaxSize = GraphShader.ARRAY_MAX_SIZE_LIGHT;
                        m_shaderGraphMono.arrayMaxSize = GraphShader.ARRAY_MAX_SIZE_LIGHT;

                        if (m_shaderGraphAllocated.image.material == null)
                        {
                            m_shaderGraphAllocated.image.material = new Material(ShaderLight);
                        }

                        if (m_shaderGraphReserved.image.material == null)
                        {
                            m_shaderGraphReserved.image.material = new Material(ShaderLight);
                        }

                        if (m_shaderGraphMono.image.material == null)
                        {
                            m_shaderGraphMono.image.material = new Material(ShaderLight);
                        }

                        break;
                }

                m_shaderGraphAllocated.InitializeShader();
                m_shaderGraphReserved.InitializeShader();
                m_shaderGraphMono.InitializeShader();
            }
        }

        /// <inheritdoc />
        protected override void AfterInitialization()
        {
            using (_PRF_AfterInitialization.Auto())
            {
                if (m_shaderGraphAllocated == null)
                {
                    m_shaderGraphAllocated = new GraphShader();
                }

                if (m_shaderGraphReserved == null)
                {
                    m_shaderGraphReserved = new GraphShader();
                }

                if (m_shaderGraphMono == null)
                {
                    m_shaderGraphMono = new GraphShader();
                }

                if (m_shaderGraphAllocated.image == null)
                {
                    m_shaderGraphAllocated.image = m_imageAllocated;
                }

                if (m_shaderGraphReserved.image == null)
                {
                    m_shaderGraphReserved.image = m_imageReserved;
                }

                if (m_shaderGraphMono.image == null)
                {
                    m_shaderGraphMono.image = m_imageMono;
                }

                base.AfterInitialization();
            }
        }

        /// <inheritdoc />
        protected override void CreatePoints()
        {
            using (_PRF_CreatePoints.Auto())
            {
                if ((m_shaderGraphAllocated.shaderArrayValues == null) ||
                    (m_shaderGraphAllocated.shaderArrayValues.Length != settings.GraphResolution))
                {
                    m_allocatedArray = new float[settings.GraphResolution];
                    m_reservedArray = new float[settings.GraphResolution];
                    m_monoArray = new float[settings.GraphResolution];

                    m_shaderGraphAllocated.shaderArrayValues = new float[settings.GraphResolution];
                    m_shaderGraphReserved.shaderArrayValues = new float[settings.GraphResolution];
                    m_shaderGraphMono.shaderArrayValues = new float[settings.GraphResolution];
                }

                for (var i = 0; i < settings.GraphResolution; i++)
                {
                    m_shaderGraphAllocated.shaderArrayValues[i] = 0;
                    m_shaderGraphReserved.shaderArrayValues[i] = 0;
                    m_shaderGraphMono.shaderArrayValues[i] = 0;
                }

                // Initialize the material values

                // Colors

                UpdateColors();

                // Thresholds

                m_shaderGraphAllocated.goodThreshold = 0;
                m_shaderGraphAllocated.cautionThreshold = 0;
                m_shaderGraphAllocated.UpdateThresholds();

                m_shaderGraphReserved.goodThreshold = 0;
                m_shaderGraphReserved.cautionThreshold = 0;
                m_shaderGraphReserved.UpdateThresholds();

                m_shaderGraphMono.goodThreshold = 0;
                m_shaderGraphMono.cautionThreshold = 0;
                m_shaderGraphMono.UpdateThresholds();

                m_shaderGraphAllocated.UpdateArray();
                m_shaderGraphReserved.UpdateArray();
                m_shaderGraphMono.UpdateArray();

                // Average

                m_shaderGraphAllocated.average = 0;
                m_shaderGraphReserved.average = 0;
                m_shaderGraphMono.average = 0;

                m_shaderGraphAllocated.UpdateAverage();
                m_shaderGraphReserved.UpdateAverage();
                m_shaderGraphMono.UpdateAverage();
            }
        }

        /// <inheritdoc />
        protected override void UpdateGraph()
        {
            using (_PRF_UpdateGraph.Auto())
            {
                var allocatedMemory = monitor.AllocatedRam;
                var reservedMemory = monitor.ReservedRam;
                var monoMemory = monitor.MonoRam;

                m_highestMemory = 0;

                for (var i = 0; i <= (settings.GraphResolution - 1); i++)
                {
                    if (i >= (settings.GraphResolution - 1))
                    {
                        m_allocatedArray[i] = allocatedMemory;
                        m_reservedArray[i] = reservedMemory;
                        m_monoArray[i] = monoMemory;
                    }
                    else
                    {
                        m_allocatedArray[i] = m_allocatedArray[i + 1];
                        m_reservedArray[i] = m_reservedArray[i + 1];
                        m_monoArray[i] = m_monoArray[i + 1];
                    }

                    if (m_highestMemory < m_reservedArray[i])
                    {
                        m_highestMemory = m_reservedArray[i];
                    }
                }

                for (var i = 0; i <= (settings.GraphResolution - 1); i++)
                {
                    m_shaderGraphAllocated.shaderArrayValues[i] = m_allocatedArray[i] / m_highestMemory;

                    m_shaderGraphReserved.shaderArrayValues[i] = m_reservedArray[i] / m_highestMemory;

                    m_shaderGraphMono.shaderArrayValues[i] = m_monoArray[i] / m_highestMemory;
                }

                m_shaderGraphAllocated.UpdatePoints();
                m_shaderGraphReserved.UpdatePoints();
                m_shaderGraphMono.UpdatePoints();
                UpdateColors();
            }
        }

        private void UpdateColors()
        {
            m_shaderGraphAllocated.goodColor = settings.allocatedRamColor;
            m_shaderGraphAllocated.cautionColor = settings.allocatedRamColor;
            m_shaderGraphAllocated.criticalColor = settings.allocatedRamColor;

            m_shaderGraphAllocated.UpdateColors();

            m_shaderGraphReserved.goodColor = settings.reservedRamColor;
            m_shaderGraphReserved.cautionColor = settings.reservedRamColor;
            m_shaderGraphReserved.criticalColor = settings.reservedRamColor;

            m_shaderGraphReserved.UpdateColors();

            m_shaderGraphMono.goodColor = settings.monoRamColor;
            m_shaderGraphMono.cautionColor = settings.monoRamColor;
            m_shaderGraphMono.criticalColor = settings.monoRamColor;

            m_shaderGraphMono.UpdateColors();
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        private static readonly ProfilerMarker _PRF_CreatePoints =
            new ProfilerMarker(_PRF_PFX + nameof(CreatePoints));

        private static readonly ProfilerMarker _PRF_SyncParameters =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        private static readonly ProfilerMarker _PRF_UpdateGraph =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateGraph));

        #endregion
    }
}
