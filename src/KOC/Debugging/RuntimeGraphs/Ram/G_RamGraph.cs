using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Shader;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Ram
{
    public class G_RamGraph : RuntimeGraphInstance<G_RamGraph, G_RamManager, G_RamMonitor, G_RamText,
        RuntimeGraphRamSettings>
    {
        #region Fields and Autoproperties

        [SerializeField] private Image m_imageAllocated;
        [SerializeField] private Image m_imageReserved;
        [SerializeField] private Image m_imageMono;

        [SerializeField] private UnityEngine.Shader ShaderFull;
        [SerializeField] private UnityEngine.Shader ShaderLight;

        private G_GraphShader m_shaderGraphAllocated;
        private G_GraphShader m_shaderGraphReserved;
        private G_GraphShader m_shaderGraphMono;

        private float[] m_allocatedArray;
        private float[] m_reservedArray;
        private float[] m_monoArray;

        private float m_highestMemory;

        #endregion

        public override GameObject graphParent => m_imageAllocated.gameObject;

        protected override bool ShouldUpdate => true;

        protected override RuntimeGraphRamSettings settings => allSettings.ram;

        public override void InitializeParameters()
        {
            using (_PRF_SyncParameters.Auto())
            {
                if ((m_shaderGraphAllocated == null) ||
                    (m_shaderGraphReserved == null) ||
                    (m_shaderGraphMono == null))
                {
                    return;
                }

                switch (allSettings.general.graphyMode)
                {
                    case Mode.FULL:
                        m_shaderGraphAllocated.ArrayMaxSize = G_GraphShader.ArrayMaxSizeFull;
                        m_shaderGraphReserved.ArrayMaxSize = G_GraphShader.ArrayMaxSizeFull;
                        m_shaderGraphMono.ArrayMaxSize = G_GraphShader.ArrayMaxSizeFull;

                        if (m_shaderGraphAllocated.Image.material == null)
                        {
                            m_shaderGraphAllocated.Image.material = new Material(ShaderFull);
                        }

                        if (m_shaderGraphReserved.Image.material == null)
                        {
                            m_shaderGraphReserved.Image.material = new Material(ShaderFull);
                        }

                        if (m_shaderGraphMono.Image.material == null)
                        {
                            m_shaderGraphMono.Image.material = new Material(ShaderFull);
                        }

                        break;

                    case Mode.LIGHT:
                        m_shaderGraphAllocated.ArrayMaxSize = G_GraphShader.ArrayMaxSizeLight;
                        m_shaderGraphReserved.ArrayMaxSize = G_GraphShader.ArrayMaxSizeLight;
                        m_shaderGraphMono.ArrayMaxSize = G_GraphShader.ArrayMaxSizeLight;

                        if (m_shaderGraphAllocated.Image.material == null)
                        {
                            m_shaderGraphAllocated.Image.material = new Material(ShaderLight);
                        }

                        if (m_shaderGraphReserved.Image.material == null)
                        {
                            m_shaderGraphReserved.Image.material = new Material(ShaderLight);
                        }

                        if (m_shaderGraphMono.Image.material == null)
                        {
                            m_shaderGraphMono.Image.material = new Material(ShaderLight);
                        }

                        break;
                }

                m_shaderGraphAllocated.InitializeShader();
                m_shaderGraphReserved.InitializeShader();
                m_shaderGraphMono.InitializeShader();
            }
        }

        protected override void AfterInitialize()
        {
            using (_PRF_AfterInitialize.Auto())
            {
                if (m_shaderGraphAllocated == null)
                {
                    m_shaderGraphAllocated = new G_GraphShader();
                }

                if (m_shaderGraphReserved == null)
                {
                    m_shaderGraphReserved = new G_GraphShader();
                }

                if (m_shaderGraphMono == null)
                {
                    m_shaderGraphMono = new G_GraphShader();
                }

                if (m_shaderGraphAllocated.Image == null)
                {
                    m_shaderGraphAllocated.Image = m_imageAllocated;
                }

                if (m_shaderGraphReserved.Image == null)
                {
                    m_shaderGraphReserved.Image = m_imageReserved;
                }

                if (m_shaderGraphMono.Image == null)
                {
                    m_shaderGraphMono.Image = m_imageMono;
                }
            }
        }

        protected override void CreatePoints()
        {
            using (_PRF_CreatePoints.Auto())
            {
                if ((m_shaderGraphAllocated.ShaderArrayValues == null) ||
                    (m_shaderGraphAllocated.ShaderArrayValues.Length != settings.GraphResolution))
                {
                    m_allocatedArray = new float[settings.GraphResolution];
                    m_reservedArray = new float[settings.GraphResolution];
                    m_monoArray = new float[settings.GraphResolution];

                    m_shaderGraphAllocated.ShaderArrayValues = new float[settings.GraphResolution];
                    m_shaderGraphReserved.ShaderArrayValues = new float[settings.GraphResolution];
                    m_shaderGraphMono.ShaderArrayValues = new float[settings.GraphResolution];
                }

                for (var i = 0; i < settings.GraphResolution; i++)
                {
                    m_shaderGraphAllocated.ShaderArrayValues[i] = 0;
                    m_shaderGraphReserved.ShaderArrayValues[i] = 0;
                    m_shaderGraphMono.ShaderArrayValues[i] = 0;
                }

                // Initialize the material values

                // Colors

                UpdateColors();

                // Thresholds

                m_shaderGraphAllocated.GoodThreshold = 0;
                m_shaderGraphAllocated.CautionThreshold = 0;
                m_shaderGraphAllocated.UpdateThresholds();

                m_shaderGraphReserved.GoodThreshold = 0;
                m_shaderGraphReserved.CautionThreshold = 0;
                m_shaderGraphReserved.UpdateThresholds();

                m_shaderGraphMono.GoodThreshold = 0;
                m_shaderGraphMono.CautionThreshold = 0;
                m_shaderGraphMono.UpdateThresholds();

                m_shaderGraphAllocated.UpdateArray();
                m_shaderGraphReserved.UpdateArray();
                m_shaderGraphMono.UpdateArray();

                // Average

                m_shaderGraphAllocated.Average = 0;
                m_shaderGraphReserved.Average = 0;
                m_shaderGraphMono.Average = 0;

                m_shaderGraphAllocated.UpdateAverage();
                m_shaderGraphReserved.UpdateAverage();
                m_shaderGraphMono.UpdateAverage();
            }
        }

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
                    m_shaderGraphAllocated.ShaderArrayValues[i] = m_allocatedArray[i] / m_highestMemory;

                    m_shaderGraphReserved.ShaderArrayValues[i] = m_reservedArray[i] / m_highestMemory;

                    m_shaderGraphMono.ShaderArrayValues[i] = m_monoArray[i] / m_highestMemory;
                }

                m_shaderGraphAllocated.UpdatePoints();
                m_shaderGraphReserved.UpdatePoints();
                m_shaderGraphMono.UpdatePoints();
                UpdateColors();
            }
        }

        private void UpdateColors()
        {
            m_shaderGraphAllocated.GoodColor = settings.allocatedRamColor;
            m_shaderGraphAllocated.CautionColor = settings.allocatedRamColor;
            m_shaderGraphAllocated.CriticalColor = settings.allocatedRamColor;

            m_shaderGraphAllocated.UpdateColors();

            m_shaderGraphReserved.GoodColor = settings.reservedRamColor;
            m_shaderGraphReserved.CautionColor = settings.reservedRamColor;
            m_shaderGraphReserved.CriticalColor = settings.reservedRamColor;

            m_shaderGraphReserved.UpdateColors();

            m_shaderGraphMono.GoodColor = settings.monoRamColor;
            m_shaderGraphMono.CautionColor = settings.monoRamColor;
            m_shaderGraphMono.CriticalColor = settings.monoRamColor;

            m_shaderGraphMono.UpdateColors();
        }

        #region Profiling

        private const string _PRF_PFX = nameof(G_RamGraph) + ".";

        private static readonly ProfilerMarker _PRF_SyncParameters =
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
