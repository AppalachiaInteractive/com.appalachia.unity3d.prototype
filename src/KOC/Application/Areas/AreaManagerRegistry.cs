using System.Collections.Generic;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    public static class AreaManagerRegistry
    {
        #region Static Fields and Autoproperties

        private static Dictionary<ApplicationArea, IAreaManager> _managerLookup =
            new Dictionary<ApplicationArea, IAreaManager>();

        #endregion

        public static IAreaManager GetManager(ApplicationArea area)
        {
            Initialize();

            if (_managerLookup.ContainsKey(area))
            {
                return _managerLookup[area];
            }

            return null;
        }

        public static void Register<T, TM>(AreaManager<T, TM> manager)
            where T : AreaManager<T, TM>
            where TM : AreaMetadata<T, TM>
        {
            Initialize();

            if (_managerLookup.ContainsKey(manager.Area))
            {
                _managerLookup[manager.Area] = manager;
            }
            else
            {
                _managerLookup.Add(manager.Area, manager);
            }
        }

        private static void Initialize()
        {
            _managerLookup ??= new Dictionary<ApplicationArea, IAreaManager>();
        }
    }
}
