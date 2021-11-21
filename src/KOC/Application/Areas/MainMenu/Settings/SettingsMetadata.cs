using Doozy.Engine.Utils.ColorModels;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Areas.MainMenu.Settings
{
    public sealed class SettingsMetadata : AreaMetadata<SettingsManager, SettingsMetadata>
    {
        public override ApplicationArea Area => ApplicationArea.MainMenu_Settings;

        public Color backgroundColor;

        protected override void OnEnable()
        {
            initializationData.Initialize(this, nameof(backgroundColor), 
                () => {
                backgroundColor = new Color(0f, 0f, 0f, 0.6f);
            });
        }
    }
}
