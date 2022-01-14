using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Areas;
using Appalachia.Prototype.KOC.Collections;

namespace Appalachia.Prototype.KOC.Scenes
{
    public class MainAreaSceneInformationCollection : SingletonAppalachiaObjectLookupCollection<
        ApplicationArea, AreaSceneInformation, ApplicationAreaList, AreaSceneInformationList,
        AreaSceneInformationLookup, AreaSceneInformationCollection, MainAreaSceneInformationCollection>
    {
    }
}
