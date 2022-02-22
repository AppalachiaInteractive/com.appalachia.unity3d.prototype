using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries.Core;
using Appalachia.UI.Controls.Sets.Buttons.Button;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries.Contracts
{
    public interface IStatusBarEntryMetadata
    {
        StatusBarSection Section { get; }
        bool Enabled { get; }
        IButtonComponentSetData Button { get; }
        int Priority { get; }
    }
}
