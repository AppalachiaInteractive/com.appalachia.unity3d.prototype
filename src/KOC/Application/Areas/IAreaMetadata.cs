using System.Collections.Generic;
using Appalachia.Prototype.KOC.Application.Components.Cursors;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Input;
using Doozy.Engine.Nody.Models;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    public interface IAreaMetadata
    {
        public AdditionalCanvasShaderChannels AdditionalShaderChannels { get; }
        ApplicationArea Area { get; }
        public AreaMetadataDefaultReferences DefaultReferences { get; }
        public AudioMixerGroup MixerGroup { get; }
        public GraphicRaycaster.BlockingObjects BlockingObjects { get; }
        public bool BlocksRaycasts { get; }
        public bool DoesDrawMenu { get; }
        public bool Fullscreen { get; }
        public bool HasCustomCursor { get; }

        public bool HideCursor { get; }
        public bool IgnoreParentGroups { get; }
        public bool IgnoreReversedGraphics { get; }
        public bool Interactable { get; }
        public bool PixelPerfect { get; }
        public bool ResetScaleToOne { get; }
        public bool SetEntrySceneActive { get; }
        public bool TemplateEnabled { get; }
        public bool UpdateCursorLockState { get; }
        public CursorLockMode CursorLockState { get; }
        public Cursors Cursor { get; }
        public float Match { get; }
        public float ReferencePixelsPerUnit { get; }
        public float TemplateAlpha { get; }
        public Graph Graph { get; }
        public int SortingOrder { get; }
        public LayerMask BlockingMask { get; }
        public List<Sprite> Templates { get; }
        public LoadBehaviour LoadBehaviour { get; }
        public KOCInputActions.MapEnableState OnDisableMapState { get; }
        public KOCInputActions.MapEnableState OnEnableMapState { get; }
        public RenderMode RenderMode { get; }
        public CanvasScaler.ScaleMode UiScaleMode { get; }
        public CanvasScaler.ScreenMatchMode ScreenMatchMode { get; }
        public Sprite SelectedTemplate { get; }
        public string CanvasName { get; }
        public string GraphControllerName { get; }
        public string ViewCategory { get; }
        public string ViewName { get; }
        public Vector2 ReferenceResolution { get; }
        void Apply(UITemplateComponentSet target);
        void Apply(UIViewComponentSet target);
        void Apply(UICanvasAreaComponentSet target);
    }
}
