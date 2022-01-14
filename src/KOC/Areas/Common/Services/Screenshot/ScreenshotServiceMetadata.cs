using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Common.Services.Screenshot.Model;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Common.Services.Screenshot
{
    public abstract class ScreenshotServiceMetadata<TService, TServiceMetadata, TAreaManager, TAreaMetadata> :
        AreaServiceMetadata<TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TService : ScreenshotService<TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TServiceMetadata :
        ScreenshotServiceMetadata<TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup("Capture")] public ScreenshotCaptureMethod captureMethod;

        [BoxGroup("Capture")]
        [ValueDropdown(nameof(_textureWidths))]
        public int outputSize;

        [BoxGroup("Capture")] public bool linear;

        [BoxGroup("Capture")] public TextureFormat textureFormat = TextureFormat.ARGB32;

        [BoxGroup("Capture")] public RenderTextureFormat renderTextureFormat = RenderTextureFormat.ARGB32;

        [BoxGroup("Capture")]
        [ValueDropdown(nameof(_previewTextureWidths))]
        public int previewSize;

        [BoxGroup("Naming")] public bool useCustomFileNamePrefix;

        [BoxGroup("Naming"), ShowIf(nameof(useCustomFileNamePrefix))]
        public string customFileNamePrefix = "";

        [BoxGroup("Naming")] public bool includeProject = true;

        [BoxGroup("Naming")] public bool includeCamera;

        [BoxGroup("Naming")] public bool includeDate = true;

        [BoxGroup("Naming")] public bool includeResolution = true;

        [BoxGroup("Naming")] public bool includeCounter = true;

        [BoxGroup("Output")] public ScreenshotFileType fileType;

        [BoxGroup("Output")] public bool saveToApplicationPath;

        [BoxGroup("Output")] public bool useCustomSubDirectoryName;

        [BoxGroup("Output"), FolderPath, ShowIf(nameof(_showCustomDirectory))]
        public string customDirectoryName = "";

        #endregion

        private static int[] _previewTextureWidths => APPAINT.POWERS_OF.TWO.PreviewTextureSizes;

        private static int[] _textureWidths => APPAINT.POWERS_OF.TWO.TextureSizes;

        private bool _showCustomDirectory => useCustomSubDirectoryName;

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(outputSize),  outputSize == 0,  () => outputSize = 2048);
                initializer.Do(this, nameof(previewSize), previewSize == 0, () => previewSize = 256);
                initializer.Do(this, nameof(linear),      () => linear = true);
                initializer.Do(
                    this,
                    nameof(textureFormat),
                    textureFormat == default,
                    () => textureFormat = TextureFormat.ARGB32
                );
                initializer.Do(
                    this,
                    nameof(renderTextureFormat),
                    renderTextureFormat == default,
                    () => renderTextureFormat = RenderTextureFormat.ARGB32
                );
            }
        }
    }
}
