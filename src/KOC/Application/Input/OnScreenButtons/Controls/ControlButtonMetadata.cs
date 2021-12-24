using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Extensions;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons.Devices;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Input.OnScreenButtons.Controls
{
    [Serializable, SmartLabelChildren, SmartLabel]
    public sealed class ControlButtonMetadata : AppalachiaObject<ControlButtonMetadata>
    {
        #region Fields and Autoproperties

        [ReadOnly] public DeviceMetadata device;

        [SerializeField]
        [ReadOnly]
        public ControlInfoData data;

        [SerializeField] public ControlButtonSpriteSet images;

        public OverridableString imageNameOverride;

        [SerializeField] private string _lastRootFolder;

        #endregion

        public string GetDisplayText(InputAction action, OnScreenButtonTextStyle style)
        {
            return data.GetDisplayText(action, style);
        }

        public Sprite GetSprite(OnScreenButtonSpriteStyle spriteStyle)
        {
            return images.GetSprite(spriteStyle);
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ControlButtonMetadata) + ".";

        #endregion

#if UNITY_EDITOR
        [Button]
        private void FindSprites()
        {
            FindSprites(_lastRootFolder);
        }

        public void FindSprites(string rootFolder)
        {
            using (_PRF_FindSprites.Auto())
            {
                try
                {
                    this.MarkAsModified();
                    _lastRootFolder = rootFolder;

                    var spriteDirectory = GetDirectoryForSprites();
                    var namesToTry = GetNamesToTry();

                    var spriteOutline = FindSprite(
                        rootFolder,
                        spriteDirectory,
                        OnScreenButtonSpriteStyle.Outline,
                        namesToTry
                    );
                    var spriteReversedOutline = FindSprite(
                        rootFolder,
                        spriteDirectory,
                        OnScreenButtonSpriteStyle.ReversedOutline,
                        namesToTry
                    );
                    var spriteIllustrative = FindSprite(
                        rootFolder,
                        spriteDirectory,
                        OnScreenButtonSpriteStyle.Illustrative,
                        namesToTry
                    );

                    if (spriteOutline != null)
                    {
                        this.MarkAsModified();
                        images.outline = spriteOutline;
                    }

                    if (spriteReversedOutline != null)
                    {
                        this.MarkAsModified();
                        images.reversedOutline = spriteReversedOutline;
                    }

                    if (spriteIllustrative != null)
                    {
                        this.MarkAsModified();
                        images.illustrative = spriteIllustrative;
                    }

                    if ((images.outline == null) ||
                        (images.reversedOutline == null) ||
                        (images.illustrative == null))
                    {
                        Context.Log.Warn(
                            ZString.Format("Could not find all images for {0}", data.ToString())
                        );
                    }
                }
                catch (Exception ex)
                {
                    Context.Log.Error(ZString.Format("Error finding images for {0}", data), ex);
                }
            }
        }

        private Sprite FindSprite(
            string rootFolder,
            string spriteDirectory,
            OnScreenButtonSpriteStyle spriteStyle,
            params string[] namesToTry)
        {
            var subdirectory = GetSubdirectoryByStyle(spriteStyle);
            var fullDirectory = AppaPath.Combine(rootFolder, spriteDirectory, subdirectory);

            foreach (var nameToTry in namesToTry)
            {
                if (nameToTry == null)
                {
                    continue;
                }

                var matches = AppaDirectory.GetFiles(fullDirectory, nameToTry + "*");

                foreach (var match in matches)
                {
                    var relativeMatch = match.ToRelativePath();

                    var asset = AssetDatabaseManager.LoadAssetAtPath<Sprite>(relativeMatch);

                    if (asset == null)
                    {
                        continue;
                    }

                    return asset;
                }
            }

            return null;
        }

        private string GetDirectoryForSprites()
        {
            return device.deviceName;
        }

        private string[] GetNamesToTry()
        {
            var prefix = GetPrefix().Trim();

            var fields = new[] { data.name, data.shortDisplayName, data.displayName };

            var results = new List<string>();

            if (imageNameOverride == null)
            {
                imageNameOverride = new OverridableString();
                this.MarkAsModified();
            }

            if (imageNameOverride.overrideEnabled && imageNameOverride.value.IsNotNullOrWhiteSpace())
            {
                results.Add(ZString.Format("{0}{1}", prefix, imageNameOverride.value));
            }

            foreach (var field in fields)
            {
                if (field.IsNullOrWhiteSpace() || field is "." or "," or "/" or "\\")
                {
                    continue;
                }

                var attempt = ZString.Format("{0}{1}", prefix, field.Trim().ToLowerInvariant());

                results.Add(attempt);

                if (attempt.Contains("button"))
                {
                    results.Add(attempt.Replace("button", string.Empty));
                }
                else
                {
                    results.Add(attempt + "button");
                }

                if (attempt.Contains("click"))
                {
                    results.Add(attempt.Replace("click", string.Empty));
                }
                else
                {
                    results.Add(attempt + "click");
                }

                if (attempt.Contains("numpad"))
                {
                    results.Add(attempt.Replace("numpad", string.Empty) + "pad");
                    results.Add(attempt.Replace("numpad", string.Empty));
                }

                if (results.Contains("left"))
                {
                    results.Add(attempt.Replace("left", "open"));
                }

                if (results.Contains("right"))
                {
                    results.Add(attempt.Replace("right", "close"));
                }

                if (results.Contains("lock"))
                {
                    results.Add(attempt.Replace("lock", ""));
                }

                if (results.Contains("left"))
                {
                    results.Add(attempt.Replace("left", "") + "left");
                }

                if (results.Contains("right"))
                {
                    results.Add(attempt.Replace("right", "") + "right");
                }

                if (results.Contains("up"))
                {
                    results.Add(attempt.Replace("up", "") + "up");
                }

                if (results.Contains("down"))
                {
                    results.Add(attempt.Replace("down", "") + "down");
                }
            }

            return results.ToArray();
        }

        private string GetPrefix()
        {
            if (device.deviceName == "Keyboard")
            {
                return "KEY_";
            }

            if (device.deviceName == "Mouse")
            {
                return "Mouse_";
            }

            if (device.deviceName == "XB1")
            {
                return "XB1_";
            }

            if (device.deviceName == "PS4")
            {
                return "PS4_";
            }

            if (device.deviceName == "ST")
            {
                return "ST_";
            }

            throw new NotImplementedException(device.deviceName);
        }

        private string GetSubdirectoryByStyle(OnScreenButtonSpriteStyle spriteStyle)
        {
            switch (spriteStyle)
            {
                case OnScreenButtonSpriteStyle.Outline:
                    return "outlined";
                case OnScreenButtonSpriteStyle.ReversedOutline:
                    return "reversedOut";
                case OnScreenButtonSpriteStyle.Illustrative:
                    return "illustrative";
                default:
                    throw new ArgumentOutOfRangeException(nameof(spriteStyle), spriteStyle, null);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_FindSprites =
            new ProfilerMarker(_PRF_PFX + nameof(FindSprites));

        #endregion

#endif
    }
}
