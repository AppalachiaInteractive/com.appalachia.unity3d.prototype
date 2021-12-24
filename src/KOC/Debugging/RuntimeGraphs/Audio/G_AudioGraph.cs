using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Shader;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Audio
{
    public class G_AudioGraph : RuntimeGraphInstance<G_AudioGraph, G_AudioManager, G_AudioMonitor, G_AudioText
        , RuntimeGraphAudioSettings>
    {
        #region Fields and Autoproperties

        [SerializeField] private Image m_imageGraph;
        [SerializeField] private Image m_imageGraphHighestValues;

        [SerializeField] private UnityEngine.Shader ShaderFull;
        [SerializeField] private UnityEngine.Shader ShaderLight;

        private G_GraphShader _shaderGraph;
        private G_GraphShader _shaderGraphHighestValues;

        private float[] _graphArray;
        private float[] _graphArrayHighestValue;

        #endregion

        public override GameObject graphParent => m_imageGraph.gameObject;

        protected override bool ShouldUpdate => monitor.SpectrumDataAvailable;

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

        public override void InitializeParameters()
        {
            using (_PRF_UpdateParameters.Auto())
            {
                if (_shaderGraph == null)
                {
                    return;
                }

                switch (allSettings.general.graphyMode)
                {
                    case Mode.FULL:
                        _shaderGraph.ArrayMaxSize = G_GraphShader.ArrayMaxSizeFull;
                        if (_shaderGraph.Image.material == null)
                        {
                            _shaderGraph.Image.material = new Material(ShaderFull);
                        }

                        _shaderGraphHighestValues.ArrayMaxSize = G_GraphShader.ArrayMaxSizeFull;
                        if (_shaderGraphHighestValues.Image.material == null)
                        {
                            _shaderGraphHighestValues.Image.material = new Material(ShaderFull);
                        }

                        break;

                    case Mode.LIGHT:
                        _shaderGraph.ArrayMaxSize = G_GraphShader.ArrayMaxSizeLight;
                        if (_shaderGraph.Image.material == null)
                        {
                            _shaderGraph.Image.material = new Material(ShaderLight);
                        }

                        _shaderGraphHighestValues.ArrayMaxSize = G_GraphShader.ArrayMaxSizeLight;
                        if (_shaderGraphHighestValues.Image.material == null)
                        {
                            _shaderGraphHighestValues.Image.material = new Material(ShaderLight);
                        }

                        break;
                }

                _shaderGraph.InitializeShader();
                _shaderGraphHighestValues.InitializeShader();
            }
        }

        protected override void AfterInitialize()
        {
            using (_PRF_AfterInitialize.Auto())
            {
                _shaderGraph = new G_GraphShader { Image = m_imageGraph };

                _shaderGraphHighestValues = new G_GraphShader { Image = m_imageGraphHighestValues };
            }
        }

        protected override void CreatePoints()
        {
            using (_PRF_CreatePoints.Auto())
            {
                // Init Arrays
                if ((_shaderGraph.ShaderArrayValues == null) ||
                    (_shaderGraph.ShaderArrayValues.Length != settings.GraphResolution))
                {
                    _graphArray = new float[settings.GraphResolution];
                    _graphArrayHighestValue = new float[settings.GraphResolution];
                    _shaderGraph.ShaderArrayValues = new float[settings.GraphResolution];
                    _shaderGraphHighestValues.ShaderArrayValues = new float[settings.GraphResolution];
                }

                for (var i = 0; i < settings.GraphResolution; i++)
                {
                    _shaderGraph.ShaderArrayValues[i] = 0;
                    _shaderGraphHighestValues.ShaderArrayValues[i] = 0;
                }

                // Color
                _shaderGraph.GoodColor = settings.audioGraphColor;
                _shaderGraph.CautionColor = settings.audioGraphColor;
                _shaderGraph.CriticalColor = settings.audioGraphColor;
                _shaderGraph.UpdateColors();

                _shaderGraphHighestValues.GoodColor = settings.audioGraphColor;
                _shaderGraphHighestValues.CautionColor = settings.audioGraphColor;
                _shaderGraphHighestValues.CriticalColor = settings.audioGraphColor;
                _shaderGraphHighestValues.UpdateColors();

                // Threshold
                _shaderGraph.GoodThreshold = 0;
                _shaderGraph.CautionThreshold = 0;
                _shaderGraph.UpdateThresholds();

                _shaderGraphHighestValues.GoodThreshold = 0;
                _shaderGraphHighestValues.CautionThreshold = 0;
                _shaderGraphHighestValues.UpdateThresholds();

                // Update Array
                _shaderGraph.UpdateArray();
                _shaderGraphHighestValues.UpdateArray();

                // Average
                _shaderGraph.Average = 0;
                _shaderGraph.UpdateAverage();

                _shaderGraphHighestValues.Average = 0;
                _shaderGraphHighestValues.UpdateAverage();
            }
        }

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
                    _shaderGraph.ShaderArrayValues[i] = _graphArray[i];
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
                    _shaderGraphHighestValues.ShaderArrayValues[i] = _graphArrayHighestValue[i];
                }

                _shaderGraphHighestValues.UpdatePoints();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(G_AudioGraph) + ".";

        private static readonly ProfilerMarker _PRF_dBNormalized =
            new ProfilerMarker(_PRF_PFX + nameof(dBNormalized));

        private static readonly ProfilerMarker _PRF_lin2dB = new ProfilerMarker(_PRF_PFX + nameof(lin2dB));

        private static readonly ProfilerMarker _PRF_UpdateParameters =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        private static readonly ProfilerMarker _PRF_UpdateGraph =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateGraph));

        private static readonly ProfilerMarker _PRF_AfterInitialize =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialize));

        private static readonly ProfilerMarker _PRF_CreatePoints =
            new ProfilerMarker(_PRF_PFX + nameof(CreatePoints));

        #endregion
    }
}
