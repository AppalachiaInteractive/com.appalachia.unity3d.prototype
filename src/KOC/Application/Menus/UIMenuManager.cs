using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Menus.Components;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Groups;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Wrappers;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Menus
{
    [ExecuteAlways]
    public class UIMenuManager : AppalachiaApplicationBehaviour
    {
        #region Fields and Autoproperties

        [SerializeField, InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        private UIMenuBackgroundMetadataGroup _backgroundMetadataGroup;

        [SerializeField, InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        private UIMenuButtonMetadataGroup _buttonMetadataGroup;

        [SerializeField, HideLabel, InlineProperty]
        private List<UIMenuButtonWrapper> _buttons;

        [SerializeField, HideLabel, InlineProperty]
        private List<UIMenuBackgroundWrapper> _backgrounds;

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

        protected override async AppaTask WhenEnabled()
        {
            using (_PRF_OnEnable.Auto())
            {
                await base.WhenEnabled();

                Initialize();

                UpdateMenus();
            }
        }

        #endregion

#if UNITY_EDITOR
        public void CreateMetadata(string metadataName)
        {
            using (_PRF_CreateMetadata.Auto())
            {
                LoadOrCreateNewIfNull(ref _buttonMetadataGroup,     metadataName + "_ButtonMetadata");
                LoadOrCreateNewIfNull(ref _backgroundMetadataGroup, metadataName + "_BackgroundMetadata");
            }
        }
#endif

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

        private UIMenuBackgroundWrapper CreateUIMenuBackground(UIMenuBackgroundMetadata metadata, int index)
        {
            using (_PRF_CreateUIMenuBackground.Auto())
            {
                var backgroundName = "<Error Retrieving Name>";

                try
                {
                    backgroundName = GetBackgroundBaseName(metadata, index);

                    var background = new UIMenuBackgroundWrapper { metadata = metadata };

                    background.components.Configure(gameObject, backgroundName);

                    return background;
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        ZString.Format(
                            "Error building {0} {1}: {2}",
                            nameof(UIMenuBackgroundWrapper),
                            backgroundName,
                            ex.Message
                        )
                    );
                    Context.Log.Error(ex);
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
                    var buttonName = GetButtonBaseName(metadata, index);
                    var button = new UIMenuButtonWrapper { metadata = metadata };
                    button.components.Configure(gameObject, buttonName);

                    return button;
                }
                catch (Exception ex)
                {
                    var buttonName = GetButtonBaseName(metadata, index);

                    Context.Log.Error(
                        ZString.Format(
                            "Error building {0} {1}: {2}",
                            nameof(UIMenuButtonWrapper),
                            buttonName,
                            ex.Message
                        )
                    );
                    Context.Log.Error(ex);
                    return null;
                }
            }
        }

        private string GetBackgroundBaseName(UIMenuBackgroundMetadata metadata, int index)
        {
            using (_PRF_GetBackgroundBaseName.Auto())
            {
                var hasSprite = metadata.sprite != null;

                var backgroundName =
                    hasSprite ? metadata.sprite.name : ZString.Format("Background Image {0}", index);

                return backgroundName;
            }
        }

        private string GetButtonBaseName(UIMenuButtonMetadata metadata, int index)
        {
            using (_PRF_GetButtonBaseName.Auto())
            {
                var buttonName = metadata == null ? "[MISSING]" : metadata.text;

                return buttonName;
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
                    for (var index = 0; index < _backgrounds.Count; index++)
                    {
                        var uiMenuBackground = _backgrounds[index];
                        var metadata = uiMenuBackground.metadata;
                        var components = uiMenuBackground.components;

                        var backgroundName = GetBackgroundBaseName(metadata, index);
                        metadata.Apply(gameObject, backgroundName, ref components);
                    }
                }

                if (_buttons != null)
                {
                    for (var index = 0; index < _buttons.Count; index++)
                    {
                        var uiMenuButton = _buttons[index];
                        var metadata = uiMenuButton.metadata;
                        var components = uiMenuButton.components;

                        var buttonName = GetButtonBaseName(metadata, index);
                        metadata.Apply(gameObject, buttonName, ref components);
                    }
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIMenuManager) + ".";

        private static readonly ProfilerMarker _PRF_GetBackgroundBaseName =
            new ProfilerMarker(_PRF_PFX + nameof(GetBackgroundBaseName));

        private static readonly ProfilerMarker _PRF_GetButtonBaseName =
            new ProfilerMarker(_PRF_PFX + nameof(GetButtonBaseName));

        private static readonly ProfilerMarker _PRF_CreateUIMenuBackground =
            new ProfilerMarker(_PRF_PFX + nameof(CreateUIMenuBackground));

        private static readonly ProfilerMarker _PRF_CreateUIMenuButton =
            new ProfilerMarker(_PRF_PFX + nameof(CreateUIMenuButton));

#if UNITY_EDITOR
        private static readonly ProfilerMarker _PRF_CreateMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(CreateMetadata));
#endif

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
