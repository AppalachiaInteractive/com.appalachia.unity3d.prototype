using System;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Services.Screenshot.Model
{
    [Serializable]
    public class ScreenshotCompletedArgs
    {
        public ScreenshotCompletedArgs(
            Texture2D screenshot,
            string screenshotFilePath,
            Texture2D preview,
            string previewFilePath)
        {
            this.screenshot = screenshot;
            this.screenshotFilePath = screenshotFilePath;
            this.preview = preview;
            this.previewFilePath = previewFilePath;
        }

        #region Fields and Autoproperties

        public readonly string previewFilePath;
        public readonly string screenshotFilePath;
        public readonly Texture2D preview;

        public readonly Texture2D screenshot;

        #endregion
    }
}
