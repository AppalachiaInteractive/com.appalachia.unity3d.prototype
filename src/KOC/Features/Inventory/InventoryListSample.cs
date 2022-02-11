/*
using Appalachia.Core.Objects.Root;
using Appalachia.UI.Controls.Views.ListView;
using Appalachia.UI.Core.Icons;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Inventory
{
    [ExecuteAlways]
    public sealed class InventoryListSample : AppalachiaBehaviour<InventoryListSample>
    {
        #region Fields and Autoproperties

        public CharacterInventoryInstance characterInventoryInstance;
        public InventoryItemLibrary inventoryItemLibrary;
        public UIIconLibrary iconLibrary;
        private ListView _listView;

        #endregion

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            if (_listView == null)
            {
                _listView = GetComponent<ListView>();
            }

            if (characterInventoryInstance == null)
            {
                characterInventoryInstance = new CharacterInventoryInstance();
            }

            characterInventoryInstance.Validate();

            if (characterInventoryInstance.items.Count == 0)
            {
                foreach (var item in inventoryItemLibrary.items)
                {
                    var itemInstance = item.CreateInstance();

                    if (item.name.Contains("Ugly"))
                    {
                        itemInstance.SetFavorite();
                    }
                    else if (item.name.Contains("Cool"))
                    {
                        itemInstance.SetEquipped();
                    }

                    characterInventoryInstance.items.Add(itemInstance);
                }
            }

            _listView.Initialize(inventoryItemLibrary.items.Count, GetItemByIndex);
        }

        private ListViewItem GetItemByIndex(ListView view, int index)
        {
            var first = view.ItemPrefabDataList[0];
            var newInstance = view.NewListViewItem(first.prefabObject.name);

            var inventoryUIComponent = newInstance.gameObject.GetComponent<InventoryItemInstanceUI>();
            var instance = characterInventoryInstance.items[index];

            inventoryUIComponent.Display(index, instance, characterInventoryInstance, iconLibrary);

            return newInstance;
        }
    }
}
*/


