using Appalachia.Data.Core.Documents;
using Appalachia.Data.Model.Fields.Quality;
using Appalachia.Data.Model.Fields.Quality.Settings;
using Appalachia.Prototype.KOC.Data.Collections.User;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Data.Documents.User
{
    public class QualitySettingLevel : AppaDocument<QualitySettingLevel, QualitySettingsCollection>
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     <para>Global anisotropic filtering mode.</para>
        /// </summary>
        public QualitySettingAnisotropicFiltering AnisotropicFiltering { get; set; }

        /// <summary>
        ///     <para>Set The AA Filtering option.</para>
        /// </summary>
        public QualitySettingAntiAliasing AntiAliasing { get; set; }

        /// <summary>
        ///     <para>
        ///         This flag controls if the async upload pipeline's ring buffer remains allocated when there are no active loading operations.
        ///         Set this to true, to make the ring buffer allocation persist after all upload operations have completed.
        ///         If you have issues with excessive memory usage, you can set this to false. This means you reduce the runtime memory footprint, but memory
        ///         fragmentation can occur.
        ///         The default value is true.
        ///     </para>
        /// </summary>
        public QualitySettingBool AsyncUploadPersistentBuffer { get; set; }

        /// <summary>
        ///     <para>Enables real-time reflection probes.</para>
        /// </summary>
        public QualitySettingBool RealtimeReflectionProbes { get; set; }

        /// <summary>
        ///     <para>Should soft blending be used for particles?</para>
        /// </summary>
        public QualitySettingBool SoftParticles { get; set; }

        /// <summary>
        ///     <para>Enable automatic streaming of texture mipmap levels based on their distance from all active cameras.</para>
        /// </summary>
        public QualitySettingBool TextureStreamingActive { get; set; }

        /// <summary>
        ///     <para>Process all enabled Cameras for texture streaming (rather than just those with StreamingController components).</para>
        /// </summary>
        public QualitySettingBool TextureStreamingAddAllCameras { get; set; }

        /// <summary>
        ///     <para>Global multiplier for the LOD's switching distance.</para>
        /// </summary>
        public QualitySettingFloat LodBias { get; set; }

        /// <summary>
        ///     <para>
        ///         In resolution scaling mode, this factor is used to multiply with the target Fixed DPI specified to get the actual Fixed DPI to use for this
        ///         quality setting.
        ///     </para>
        /// </summary>
        public QualitySettingFloat ResolutionScalingFixedDPIFactor { get; set; }

        /// <summary>
        ///     <para>The normalized cascade distribution for a 2 cascade setup. The value defines the position of the cascade with respect to Zero.</para>
        /// </summary>
        public QualitySettingFloat ShadowCascade2Split { get; set; }

        /// <summary>
        ///     <para>Shadow drawing distance.</para>
        /// </summary>
        public QualitySettingFloat ShadowDistance { get; set; }

        /// <summary>
        ///     <para>Offset shadow frustum near plane.</para>
        /// </summary>
        public QualitySettingFloat ShadowNearPlaneOffset { get; set; }

        /// <summary>
        ///     <para>Heightmap patches beyond basemap distance will use a precomputed low res basemap.</para>
        /// </summary>
        public QualitySettingFloat TerrainBasemapDistance { get; set; }

        /// <summary>
        ///     <para>Density of detail objects.</para>
        /// </summary>
        public QualitySettingFloat TerrainDetailObjectDensity { get; set; }

        /// <summary>
        ///     <para>Detail objects will be displayed up to this distance.</para>
        /// </summary>
        public QualitySettingFloat TerrainDetailObjectDistance { get; set; }

        /// <summary>
        ///     <para>An approximation of how many pixels the terrain will pop in the worst case when switching lod.</para>
        /// </summary>
        public QualitySettingFloat TerrainHeightmapPixelError { get; set; }

        /// <summary>
        ///     <para>The total amount of memory to be used by streaming and non-streaming textures.</para>
        /// </summary>
        public QualitySettingFloat TextureStreamingMemoryBudget { get; set; }

        /// <summary>
        ///     <para>Distance from the camera where trees will be rendered as billboards only.</para>
        /// </summary>
        public QualitySettingFloat TreeBillboardDistance { get; set; }

        /// <summary>
        ///     <para>Total distance delta that trees will use to transition from billboard orientation to mesh orientation.</para>
        /// </summary>
        public QualitySettingFloat TreeCrossFadeLength { get; set; }

        /// <summary>
        ///     <para>The maximum distance at which trees are rendered.</para>
        /// </summary>
        public QualitySettingFloat TreeDistance { get; set; }

        /// <summary>
        ///     <para>The multiplier to the current LOD bias used for rendering LOD trees (i.e. SpeedTree trees).</para>
        /// </summary>
        public QualitySettingFloat TreeLODBiasMultiplier { get; set; }

        /// <summary>
        ///     <para>
        ///         Asynchronous texture and mesh data upload provides timesliced async texture and mesh data upload on the render thread with tight control over
        ///         memory and timeslicing. There are no allocations except for the ones which driver has to do. To read data and upload texture and mesh data,
        ///         Unity re-uses a ringbuffer whose size can be controlled.
        ///         Use asyncUploadBufferSize to set the buffer size for asynchronous texture and mesh data uploads. The size is in megabytes. The minimum value
        ///         is 2 and the maximum value is 512. The buffer resizes automatically to fit the largest texture currently loading. To avoid re-sizing of the
        ///         buffer, which can incur performance cost, set the value approximately to the size of biggest texture used in the Scene.
        ///     </para>
        /// </summary>
        public QualitySettingInt AsyncUploadBufferSize { get; set; }

        /// <summary>
        ///     <para>
        ///         Async texture upload provides timesliced async texture upload on the render thread with tight control over memory and timeslicing. There are
        ///         no allocations except for the ones which driver has to do. To read data and upload texture data a ringbuffer whose size can be controlled is
        ///         re-used.
        ///         Use asyncUploadTimeSlice to set the time-slice in milliseconds for asynchronous texture uploads per
        ///         frame. Minimum value is 1 and maximum is 33.
        ///     </para>
        /// </summary>
        public QualitySettingInt AsyncUploadTimeSlice { get; set; }

        /// <summary>
        ///     <para>A maximum LOD level. All LOD groups.</para>
        /// </summary>
        public QualitySettingInt MaximumLODLevel { get; set; }

        /// <summary>
        ///     <para>Budget for how many ray casts can be performed per frame for approximate collision testing.</para>
        /// </summary>
        public QualitySettingInt ParticleRaycastBudget { get; set; }

        /// <summary>
        ///     <para>The maximum number of pixel lights that should affect any object.</para>
        /// </summary>
        public QualitySettingInt PixelLightCount { get; set; }

        /// <summary>
        ///     <para>Number of cascades to use for directional light shadows.</para>
        /// </summary>
        public QualitySettingInt ShadowCascades { get; set; }

        /// <summary>
        ///     <para>Lets you essentially lower the heightmap resolution used for rendering.</para>
        /// </summary>
        public QualitySettingInt TerrainHeightmapMaximumLOD { get; set; }

        /// <summary>
        ///     <para>The index of the baked lightmap applied to this terrain.</para>
        /// </summary>
        public QualitySettingInt TerrainLightmapIndex { get; set; }

        /// <summary>
        ///     <para>The maximum number of active texture file IO requests from the texture streaming system.</para>
        /// </summary>
        public QualitySettingInt TextureStreamingMaxFileIORequests { get; set; }

        /// <summary>
        ///     <para>The maximum number of mipmap levels to discard for each texture.</para>
        /// </summary>
        public QualitySettingInt TextureStreamingMaxLevelReduction { get; set; }

        /// <summary>
        ///     <para>The number of renderer instances that are processed each frame when calculating which texture mipmap levels should be streamed.</para>
        /// </summary>
        public QualitySettingInt TextureStreamingRenderersPerFrame { get; set; }

        /// <summary>
        ///     <para>Maximum number of trees rendered at full LOD.</para>
        /// </summary>
        public QualitySettingInt TreeMaximumFullLODCount { get; set; }

        /// <summary>
        ///     <para>How reflection probes are used for terrain. See Rendering.ReflectionProbeUsage.</para>
        /// </summary>
        public QualitySettingReflectionProbeUsage TerrainReflectionProbeUsage { get; set; }

        /// <summary>
        ///     <para>Allows you to set the shadow casting mode for the terrain.</para>
        /// </summary>
        public QualitySettingShadowCastingMode TerrainShadowCastingMode { get; set; }

        /// <summary>
        ///     <para>The rendering mode of Shadowmask.</para>
        /// </summary>
        public QualitySettingShadowmaskMode ShadowmaskMode { get; set; }

        /// <summary>
        ///     <para>Directional light shadow projection.</para>
        /// </summary>
        public QualitySettingShadowProjection ShadowProjection { get; set; }

        /// <summary>
        ///     <para>Real-time Shadows type to be used.</para>
        /// </summary>
        public QualitySettingShadowQuality Shadows { get; set; }

        /// <summary>
        ///     <para>The default resolution of the shadow maps.</para>
        /// </summary>
        public QualitySettingShadowResolution ShadowResolution { get; set; }

        public QualitySettingsPresetType Preset { get; set; }

        /// <summary>
        ///     <para>A texture size limit applied to most textures.</para>
        /// </summary>
        public QualitySettingTextureQuality TextureQuality { get; set; }

        /// <summary>
        ///     <para>
        ///         The normalized cascade start position for a 4 cascade setup. Each member of the vector defines the normalized position of the coresponding
        ///         cascade with respect to Zero.
        ///     </para>
        /// </summary>
        public QualitySettingVector3 ShadowCascade4Split { get; set; }

        /// <summary>
        ///     <para>The number of vertical syncs that should pass between each frame.</para>
        /// </summary>
        public QualitySettingVSync VSyncCount { get; set; }

        #endregion

        protected override void SetDefaults()
        {
        }
    }
}
