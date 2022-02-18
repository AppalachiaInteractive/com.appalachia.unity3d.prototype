using Appalachia.CI.Constants;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Functionality
{
    public interface IApplicationFunctionalityManager
    {
        #region Constants and Static Readonly

        public const string PARENT_NAME_FEATURES = APPASTR.ObjectNames.Features;
        public const string PARENT_NAME_SERVICES = APPASTR.ObjectNames.Services;
        public const string PARENT_NAME_WIDGETS = APPASTR.ObjectNames.Widgets;

        #endregion

        GameObject GetFeatureParentObject();
        GameObject GetWidgetParentObject();
        AppaTask InitializeFeatures();
    }
}
