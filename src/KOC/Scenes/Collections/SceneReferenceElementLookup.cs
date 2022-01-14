using System;
using System.Linq;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Areas;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Scenes.Collections
{
    [Serializable]
    public class SceneReferenceElementLookup : AppaLookup<AreaVersion, SceneReferenceElement,
        AreaSceneVersionList, SceneReferenceElementList>
    {
        public SceneReferenceElement latest
        {
            get
            {
                var maxKey = Keys.OrderByDescending(k => k).FirstOrDefault();
                return this[maxKey];
            }
        }

        protected override bool ShouldDisplayTitle => true;

        [Button]
        public void Add()
        {
            Add(AreaVersion.None, null);
        }

        protected override Color GetDisplayColor(AreaVersion key, SceneReferenceElement value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(AreaVersion key, SceneReferenceElement value)
        {
            return null;
        }

        protected override string GetDisplayTitle(AreaVersion key, SceneReferenceElement value)
        {
            return value.name;
        }
    }
}
