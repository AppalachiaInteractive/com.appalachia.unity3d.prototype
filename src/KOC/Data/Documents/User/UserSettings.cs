
// ReSharper disable UnusedAutoPropertyAccessor.Global

using Appalachia.Data.Core.Attributes;
using Appalachia.Data.Core.Documents;
using Appalachia.Data.Model.Fields.Quality;
using Appalachia.Prototype.KOC.Data.Collections.User;

namespace Appalachia.Prototype.KOC.Data.Documents.User
{
    public class UserSettings : AppaDocument<UserSettings, UserSettingsCollection>
    {
        public QualitySettingsPresetType QualityPreset { get; set; }

        [DataRef(nameof(QualitySettingLevel))]
        public QualitySettingLevel QualitySettings { get; set; }

        protected override void SetDefaults()
        {
            QualityPreset = QualitySettingsPresetType.High;
        }
    }
}
