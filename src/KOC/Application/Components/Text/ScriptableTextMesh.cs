using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Components.Text
{
    public class ScriptableTextMesh : DynamicTextMesh<ScriptableTextMesh>
    {
        #region Fields and Autoproperties

        [InlineEditor]
        [OnValueChanged(nameof(UpdateText))]
        public ScriptableText text;

        #endregion

        protected override string GetLabel()
        {
            return text.label;
        }

        protected override string GetName()
        {
            return text.name;
        }

        protected override string GetUpdatedText()
        {
            using (_PRF_UpdateText.Auto())
            {
                if (text == null)
                {
                    return string.Empty;
                }

                text.TextChanged -= OnTextChanged;
                text.TextChanged += OnTextChanged;

                return text.text;
            }
        }

        protected void OnTextChanged(ScriptableText changedText)
        {
            using (_PRF_OnTextChanged.Auto())
            {
                textMeshValue.text = changedText.text;
                textMeshLabel.text = changedText.label;
            }
        }

        [ButtonGroup]
        private void CreateNew()
        {
#if UNITY_EDITOR
            using (_PRF_CreateNew.Auto())
            {
                text = ScriptableText.LoadOrCreateNew(name);
            }
#endif
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ScriptableTextMesh) + ".";

        private static readonly ProfilerMarker _PRF_UpdateText =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateText));

        private static readonly ProfilerMarker _PRF_CreateNew =
            new ProfilerMarker(_PRF_PFX + nameof(CreateNew));

        private static readonly ProfilerMarker _PRF_OnTextChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnTextChanged));

        #endregion
    }
}
