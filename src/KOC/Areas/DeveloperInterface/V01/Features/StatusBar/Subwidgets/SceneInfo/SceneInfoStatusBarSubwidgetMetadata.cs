using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.SceneInfo
{
    [Serializable]
    public class SceneInfoStatusBarSubwidgetMetadata : StatusBarSubwidgetMetadata<SceneInfoStatusBarSubwidget,
        SceneInfoStatusBarSubwidgetMetadata>
    {
        public override StatusBarSection DefaultSection => StatusBarSection.Left;
        protected override int DefaultPriority => 0;
    }
}
