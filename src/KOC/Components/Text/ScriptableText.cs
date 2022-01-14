using Appalachia.Core.Objects.Root;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Components.Text
{
    public class ScriptableText : AppalachiaObject<ScriptableText>
    {
        public delegate void OnScriptableTextChanged(ScriptableText text);

        public event OnScriptableTextChanged TextChanged;

        #region Fields and Autoproperties

        [OnValueChanged(nameof(InvokeTextChanged))]
        public string label;

        [OnValueChanged(nameof(InvokeTextChanged))]
        public string text;

        #endregion

        private void InvokeTextChanged()
        {
            TextChanged?.Invoke(this);
        }
    }
}
