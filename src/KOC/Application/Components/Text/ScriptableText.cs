using Appalachia.Core.Objects.Root;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Application.Components.Text
{
    public class ScriptableText : AppalachiaObject<ScriptableText>
    {
        public delegate void OnScriptableTextChanged(ScriptableText text);

        #region Fields and Autoproperties

        [OnValueChanged(nameof(InvokeTextChanged))]
        public string label;

        [OnValueChanged(nameof(InvokeTextChanged))]
        public string text;

        #endregion

        public event OnScriptableTextChanged TextChanged;

        private void InvokeTextChanged()
        {
            TextChanged?.Invoke(this);
        }
    }
}
