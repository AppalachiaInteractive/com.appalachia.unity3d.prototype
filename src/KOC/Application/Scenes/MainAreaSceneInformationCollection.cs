using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Prototype.KOC.Application.Collections;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    public class MainAreaSceneInformationCollection : SingletonAppalachiaObjectLookupCollection<
        ApplicationArea, AreaSceneInformation, ApplicationAreaList, AreaSceneInformationList,
        AreaSceneInformationLookup, AreaSceneInformationCollection, MainAreaSceneInformationCollection>
    {
    }
}
