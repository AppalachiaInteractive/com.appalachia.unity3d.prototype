using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Models;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Services;
using Appalachia.UI.Functionality.Text.Controls.Calculated;
using Appalachia.Utility.Constants;

// ReSharper disable FormatStringProblem

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Components
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInfoTextMeshControl : CalculatedTextMeshControl<DeveloperInfoTextMeshControl,
        DeveloperInfoTextMeshControlConfig, DeveloperInfoType>
    {
        static DeveloperInfoTextMeshControl()
        {
            RegisterDependency<DeveloperInfoProviderService>(i => _developerInfoProviderService = i);
        }

        #region Static Fields and Autoproperties

        private static DeveloperInfoProviderService _developerInfoProviderService;

        #endregion

        public int LineCount => textMeshValue.textMeshProUGUI.textInfo.lineCount;

        /// <inheritdoc />
        protected override string GetLabel()
        {
            using (_PRF_GetLabel.Auto())
            {
                return _developerInfoProviderService.GetLabel(currentCalculation);
            }
        }

        /// <inheritdoc />
        protected override string GetUpdatedText()
        {
            using (_PRF_GetUpdatedText.Auto())
            {
                return _developerInfoProviderService.GetUpdatedText(currentCalculation);
            }
        }

        /// <inheritdoc />
        protected override void HandleUpdateException(Exception ex)
        {
            using (_PRF_HandleUpdateException.Auto())
            {
                Context.Log.Error(nameof(UpdateText).GenericMethodException(this), this, ex);
            }
        }

        /// <inheritdoc />
        protected override bool IsImmutableValue(DeveloperInfoType x)
        {
            using (_PRF_IsImmutableValue.Auto())
            {
                return _developerInfoProviderService.IsImmutableValue(x);
            }
        }
    }
}
