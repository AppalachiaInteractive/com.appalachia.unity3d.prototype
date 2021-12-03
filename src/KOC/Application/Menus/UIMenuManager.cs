using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Menus.Components;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Groups;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Wrappers;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Menus
{
    [ExecuteAlways]
    public class UIMenuManager : AppalachiaApplicationBehaviour
    {
        #region Fields and Autoproperties

        [SerializeField, InlineEditor(InlineEditorObjectFieldModes.Boxed)] private UIMenuBackgroundMetadataGroup _backgroundMetadataGroup;
        [SerializeField, InlineEditor(InlineEditorObjectFieldModes.Boxed)] private UIMenuButtonMetadataGroup _buttonMetadataGroup;

        [SerializeField] private List<UIMenuButtonWrapper> _buttons;
        [SerializeField] private List<UIMenuBackgroundWrapper> _backgrounds;

        #endregion

        #region Event Functions

        protected override void Start()
        {
            using (_PRF_Start.Auto())
            {
                base.Start();

                Initialize();

                UpdateMenus();
            }
        }

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
                UpdateMenus();
            }
        }

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnable();

                Initialize();

                UpdateMenus();
            }
        }

        #endregion

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                InitializeElementMetadataGroup<UIMenuBackgroundMetadataGroup, UIMenuBackgroundMetadata,
                    UIMenuBackgroundComponentSet, UIMenuBackgroundWrapper>(
                    _backgroundMetadataGroup,
                    ref _backgrounds,
                    CreateUIMenuBackground
                );

                InitializeElementMetadataGroup<UIMenuButtonMetadataGroup, UIMenuButtonMetadata,
                    UIMenuButtonComponentSet, UIMenuButtonWrapper>(
                    _buttonMetadataGroup,
                    ref _buttons,
                    CreateUIMenuButton
                );
            }
        }

        public void CreateMetadata(string metadataName)
        {
            using (_PRF_CreateMetadata.Auto())
            {
                AppalachiaObject.LoadOrCreateNewIfNull(ref _buttonMetadataGroup,     metadataName + "_ButtonMetadata");
                AppalachiaObject.LoadOrCreateNewIfNull(ref _backgroundMetadataGroup, metadataName + "_BackgroundMetadata");
            }
        }

        private UIMenuBackgroundWrapper CreateUIMenuBackground(UIMenuBackgroundMetadata metadata, int index)
        {
            using (_PRF_CreateUIMenuBackground.Auto())
            {
                var backgroundName = "<Error Retrieving Name>";

                try
                {
                    var hasSprite = metadata.sprite != null;

                    backgroundName = hasSprite ? metadata.sprite.name : $"Background Image {index}";

                    var background = new UIMenuBackgroundWrapper { metadata = metadata };
                    background.components.Configure(gameObject, backgroundName);

                    return background;
                }
                catch (Exception ex)
                {
                    AppaLog.Error(
                        $"Error building {nameof(UIMenuBackgroundWrapper)} {backgroundName}: {ex.Message}"
                    );
                    AppaLog.Exception(ex);
                    return null;
                }
            }
        }

        private UIMenuButtonWrapper CreateUIMenuButton(UIMenuButtonMetadata metadata, int index)
        {
            using (_PRF_CreateUIMenuButton.Auto())
            {
                try
                {
                    var button = new UIMenuButtonWrapper { metadata = metadata };
                    button.components.Configure(gameObject, metadata.text);

                    return button;
                }
                catch (Exception ex)
                {
                    var buttonName = metadata == null ? "[MISSING]" : metadata.text;

                    AppaLog.Error($"Error building {nameof(UIMenuButtonWrapper)} {buttonName}: {ex.Message}");
                    AppaLog.Exception(ex);
                    return null;
                }
            }
        }

        private void InitializeElementMetadataGroup<TG, TE, TC, TW>(
            TG group,
            ref List<TW> wrappers,
            Func<TE, int, TW> elementWrapperConvertor)
            where TG : UIElementMetadataGroupBase<TE, TC>
            where TE : UIElementMetadataBase<TC>
            where TC : IComponentSet
            where TW : UIElementWrapper<TE, TC>
        {
            if (group == null)
            {
                return;
            }

            if (group.elements is { Length: > 0 })
            {
                if (wrappers == null)
                {
                    wrappers = group.elements.Select(elementWrapperConvertor).Where(m => m != null).ToList();
                }
                else
                {
                    while (wrappers.Count > group.elements.Length)
                    {
                        var lastButton = wrappers[^1];

                        lastButton.components.GameObject.DestroySafely();

                        wrappers.RemoveAt(wrappers.Count - 1);
                    }

                    while (wrappers.Count < group.elements.Length)
                    {
                        var index = wrappers.Count;

                        var targetMetadata = group.elements[wrappers.Count];

                        wrappers.Add(elementWrapperConvertor(targetMetadata, index));
                    }

                    for (var i = 0; i < wrappers.Count; i++)
                    {
                        wrappers[i].metadata = group.elements[i];
                    }
                }
            }
        }

        private void UpdateMenus()
        {
            using (_PRF_UpdateMenus.Auto())
            {
                if (_backgrounds != null)
                {
                    foreach (var uiMenuBackground in _backgrounds)
                    {
                        var metadata = uiMenuBackground.metadata;
                        var components = uiMenuBackground.components;

                        metadata.Apply(components);
                    }
                }

                if (_buttons != null)
                {
                    foreach (var uiMenuButton in _buttons)
                    {
                        var metadata = uiMenuButton.metadata;
                        var components = uiMenuButton.components;

                        metadata.Apply(components);
                    }
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIMenuManager) + ".";

        private static readonly ProfilerMarker _PRF_CreateUIMenuBackground =
            new ProfilerMarker(_PRF_PFX + nameof(CreateUIMenuBackground));

        private static readonly ProfilerMarker _PRF_CreateUIMenuButton =
            new ProfilerMarker(_PRF_PFX + nameof(CreateUIMenuButton));

        private static readonly ProfilerMarker _PRF_CreateMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(CreateMetadata));

        private static readonly ProfilerMarker _PRF_UpdateMenus =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateMenus));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_Start = new ProfilerMarker(_PRF_PFX + nameof(Start));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        #endregion
    }
}
