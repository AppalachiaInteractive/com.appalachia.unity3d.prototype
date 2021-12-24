using System;
using System.Reflection;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Styling.Base;
using Appalachia.Prototype.KOC.Application.Styling.Fonts;
using Appalachia.Prototype.KOC.Application.Styling.OnScreenButtons;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Styling
{
    [Serializable]
    public class ApplicationStyleElementDefaultLookup : SingletonAppalachiaObject<
            ApplicationStyleElementDefaultLookup>
    {
        #region Fields and Autoproperties

        public FontStyle fontStyle;
        public OnScreenButtonStyle onScreenButtonStyle;

        #endregion

        public TDefault Get<TDefault, TOverride, TInterface>()
            where TDefault : ApplicationStyleElementDefault<TDefault, TOverride, TInterface>, TInterface
            where TOverride : ApplicationStyleElementOverride<TDefault, TOverride, TInterface>, TInterface
            where TInterface : IApplicationStyle
        {
            using (_PRF_Get.Auto())
            {
                var defaultType = typeof(TDefault);
                var thisType = GetType();

                var fields = thisType.GetFields_CACHE(BindingFlags.Public | BindingFlags.Instance);

                foreach (var field in fields)
                {
                    if (field.FieldType == defaultType)
                    {
                        var value = field.GetValue(this);

                        return value as TDefault;
                    }
                }

                throw new NotImplementedException(defaultType.Name);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ApplicationStyleElementDefaultLookup) + ".";

        private static readonly ProfilerMarker _PRF_Get = new ProfilerMarker(_PRF_PFX + nameof(Get));

        #endregion
    }
}
