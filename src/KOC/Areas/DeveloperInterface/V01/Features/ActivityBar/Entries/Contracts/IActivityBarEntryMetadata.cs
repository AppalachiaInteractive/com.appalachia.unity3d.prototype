using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Entries.Core;
using Appalachia.UI.Controls.Sets.Buttons.Button;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Entries.Contracts
{
    public interface IActivityBarEntryMetadata
    {
        ActivityBarSection Section { get; }
        bool Enabled { get; }
        ButtonComponentSetData Button { get; }
        int Priority { get; }
    }
}
