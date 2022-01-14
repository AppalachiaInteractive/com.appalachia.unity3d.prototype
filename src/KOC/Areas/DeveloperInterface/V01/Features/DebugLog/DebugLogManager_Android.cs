#if !UNITY_EDITOR && UNITY_ANDROID
using Appalachia.Prototype.KOC.Debugging.DebugLog.Plugins.Android;

namespace Appalachia.Prototype.KOC.Debugging.DebugLog
{
    public partial class DebugLogManager
    {
        private DebugLogLogcatListener logcatListener;
    }
}
#endif
