using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Models;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Services;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries.Core;
using Appalachia.UI.Controls.Sets.Buttons.Button;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries
{
    public class SceneInfoStatusBarEntry : StatusBarEntry<SceneInfoStatusBarEntry,
        SceneInfoStatusBarEntryMetadata, ButtonComponentSet, ButtonComponentSetData>

    {
        static SceneInfoStatusBarEntry()
        {
            RegisterDependency<DeveloperInfoProviderService>(i => _developerInfoProviderService = i);
        }

        #region Static Fields and Autoproperties

        private static DeveloperInfoProviderService _developerInfoProviderService;

        #endregion

        public override void OnClicked()
        {
            using (_PRF_OnClicked.Auto())
            {
                throw new NotImplementedException();
            }
        }

        public override void UpdateStatusBarEntry()
        {
            using (_PRF_UpdateStatusBarEntry.Auto())
            {
                base.UpdateStatusBarEntry();

                var activeSceneNameAndCount =
                    _developerInfoProviderService.GetUpdatedText(DeveloperInfoType.ActiveSceneNameAndCount);

                var text = button.buttonText.TextMeshProUGUI;

                text.text = activeSceneNameAndCount;

                text.autoSizeTextContainer = true;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnClicked =
            new ProfilerMarker(_PRF_PFX + nameof(OnClicked));

        private static readonly ProfilerMarker _PRF_UpdateStatusBarEntry =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateStatusBarEntry));

        #endregion
    }
}
