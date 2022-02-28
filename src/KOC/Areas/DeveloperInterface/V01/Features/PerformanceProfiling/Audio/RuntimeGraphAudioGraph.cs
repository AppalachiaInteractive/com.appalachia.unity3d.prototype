using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Settings;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Shader;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Audio
{
    public class RuntimeGraphAudioGraph : RuntimeGraphInstance<RuntimeGraphAudioGraph,
        RuntimeGraphAudioManager, RuntimeGraphAudioMonitor, RuntimeGraphAudioText, RuntimeGraphAudioSettings>
    {
        #region Fields and Autoproperties

        [SerializeField] private Image m_imageGraph;
        [SerializeField] private Image m_imageGraphHighestValues;

        [SerializeField] private UnityEngine.Shader ShaderFull;
        [SerializeField] private UnityEngine.Shader ShaderLight;

        private GraphShader _shaderGraph;
        private GraphShader _shaderGraphHighestValues;

        private float[] _graphArray;
        private float[] _graphArrayHighestValue;

        #endregion

        /// <inheritdoc />
        public override GameObject graphParent => m_imageGraph.gameObject;

        /// <inheritdoc />
        protected override bool ShouldUpdate => monitor.SpectrumDataAvailable;

        /// <inheritdoc />
        protected override RuntimeGraphAudioSettings settings => allSettings.audio;

        /// <summary>
        ///     Normalizes a value in decibels between 0-1.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static float dBNormalized(float db)
        {
            using (_PRF_dBNormalized.Auto())
            {
                return (db + 160f) / 160f;
            }
        }

        /// <summary>
        ///     Converts spectrum values to decibels using logarithms.
        /// </summary>
        /// <param name="linear"></param>
        /// <returns></returns>
        public static float lin2dB(float linear)
        {
            using (_PRF_lin2dB.Auto())
            {
                return Mathf.Clamp(Mathf.Log10(linear) * 20.0f, -160.0f, 0.0f);
            }
        }

        /// <inheritdoc />
        public override void InitializeParameters()
        {
            using (_PRF_UpdateParameters.Auto())
            {
                if (_shaderGraph == null)
                {
                    return;
                }

                _shaderGraph.arrayMaxSize = GraphShader.ARRAY_MAX_SIZE;
                if (_shaderGraph.image.material == null)
                {
                    _shaderGraph.image.material = new Material(ShaderFull);
                }

                _shaderGraphHighestValues.arrayMaxSize = GraphShader.ARRAY_MAX_SIZE;
                if (_shaderGraphHighestValues.image.material == null)
                {
                    _shaderGraphHighestValues.image.material = new Material(ShaderFull);
                }

                _shaderGraph.InitializeShader();
                _shaderGraphHighestValues.InitializeShader();
            }
        }

        /// <inheritdoc />
        protected override void AfterInitialization()
        {
            using (_PRF_AfterInitialization.Auto())
            {
                _shaderGraph = new GraphShader { image = m_imageGraph };

                _shaderGraphHighestValues = new GraphShader { image = m_imageGraphHighestValues };

                base.AfterInitialization();
            }
        }

        /// <inheritdoc />
        protected override void CreatePoints()
        {
            using (_PRF_CreatePoints.Auto())
            {
                // Init Arrays
                if ((_shaderGraph.shaderArrayValues == null) ||
                    (_shaderGraph.shaderArrayValues.Length != settings.GraphResolution))
                {
                    _graphArray = new float[settings.GraphResolution];
                    _graphArrayHighestValue = new float[settings.GraphResolution];
                    _shaderGraph.shaderArrayValues = new float[settings.GraphResolution];
                    _shaderGraphHighestValues.shaderArrayValues = new float[settings.GraphResolution];
                }

                for (var i = 0; i < settings.GraphResolution; i++)
                {
                    _shaderGraph.shaderArrayValues[i] = 0;
                    _shaderGraphHighestValues.shaderArrayValues[i] = 0;
                }

                // Color
                _shaderGraph.goodColor = settings.audioGraphColor;
                _shaderGraph.cautionColor = settings.audioGraphColor;
                _shaderGraph.criticalColor = settings.audioGraphColor;
                _shaderGraph.UpdateColors();

                _shaderGraphHighestValues.goodColor = settings.audioGraphColor;
                _shaderGraphHighestValues.cautionColor = settings.audioGraphColor;
                _shaderGraphHighestValues.criticalColor = settings.audioGraphColor;
                _shaderGraphHighestValues.UpdateColors();

                // Threshold
                _shaderGraph.goodThreshold = 0;
                _shaderGraph.cautionThreshold = 0;
                _shaderGraph.UpdateThresholds();

                _shaderGraphHighestValues.goodThreshold = 0;
                _shaderGraphHighestValues.cautionThreshold = 0;
                _shaderGraphHighestValues.UpdateThresholds();

                // Update Array
                _shaderGraph.UpdateArray();
                _shaderGraphHighestValues.UpdateArray();

                // Average
                _shaderGraph.average = 0;
                _shaderGraph.UpdateAverage();

                _shaderGraphHighestValues.average = 0;
                _shaderGraphHighestValues.UpdateAverage();
            }
        }

        /// <inheritdoc />
        protected override void UpdateGraph()
        {
            using (_PRF_UpdateGraph.Auto())
            {
                var incrementPerIteration =
                    Mathf.FloorToInt(monitor.Spectrum.Length / (float)settings.GraphResolution);

                // Current values -------------------------

                for (var i = 0; i <= (settings.GraphResolution - 1); i++)
                {
                    float currentValue = 0;

                    for (var j = 0; j < incrementPerIteration; j++)
                    {
                        currentValue += monitor.Spectrum[(i * incrementPerIteration) + j];
                    }

                    // Uses 3 values for each bar to accomplish that look

                    if ((((i + 1) % 3) == 0) && (i > 1))
                    {
                        var value = (dBNormalized(lin2dB(currentValue / incrementPerIteration)) +
                                     _graphArray[i - 1] +
                                     _graphArray[i - 2]) /
                                    3;

                        _graphArray[i] = value;
                        _graphArray[i - 1] = value;
                        _graphArray[i - 2] =
                            -1; // Always set the third one to -1 to leave gaps in the graph and improve readability
                    }
                    else
                    {
                        _graphArray[i] = dBNormalized(lin2dB(currentValue / incrementPerIteration));
                    }
                }

                for (var i = 0; i <= (settings.GraphResolution - 1); i++)
                {
                    _shaderGraph.shaderArrayValues[i] = _graphArray[i];
                }

                _shaderGraph.UpdatePoints();

                // Highest values -------------------------

                for (var i = 0; i <= (settings.GraphResolution - 1); i++)
                {
                    float currentValue = 0;

                    for (var j = 0; j < incrementPerIteration; j++)
                    {
                        currentValue += monitor.SpectrumHighestValues[(i * incrementPerIteration) + j];
                    }

                    // Uses 3 values for each bar to accomplish that look

                    if ((((i + 1) % 3) == 0) && (i > 1))
                    {
                        var value = (dBNormalized(lin2dB(currentValue / incrementPerIteration)) +
                                     _graphArrayHighestValue[i - 1] +
                                     _graphArrayHighestValue[i - 2]) /
                                    3;

                        _graphArrayHighestValue[i] = value;
                        _graphArrayHighestValue[i - 1] = value;
                        _graphArrayHighestValue[i - 2] =
                            -1; // Always set the third one to -1 to leave gaps in the graph and improve readability
                    }
                    else
                    {
                        _graphArrayHighestValue[i] =
                            dBNormalized(lin2dB(currentValue / incrementPerIteration));
                    }
                }

                for (var i = 0; i <= (settings.GraphResolution - 1); i++)
                {
                    _shaderGraphHighestValues.shaderArrayValues[i] = _graphArrayHighestValue[i];
                }

                _shaderGraphHighestValues.UpdatePoints();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_dBNormalized =
            new ProfilerMarker(_PRF_PFX + nameof(dBNormalized));

        private static readonly ProfilerMarker _PRF_lin2dB = new ProfilerMarker(_PRF_PFX + nameof(lin2dB));

        private static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        private static readonly ProfilerMarker _PRF_CreatePoints =
            new ProfilerMarker(_PRF_PFX + nameof(CreatePoints));

        private static readonly ProfilerMarker _PRF_UpdateGraph =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateGraph));

        private static readonly ProfilerMarker _PRF_UpdateParameters =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        #endregion
    }
}
