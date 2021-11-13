using Appalachia.UI.Controls.ListView;
using Appalachia.UI.Core.Icons;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Inventory
{
    [ExecuteAlways]
    public class InventoryListSample : MonoBehaviour
    {
        public CharacterInventoryInstance characterInventoryInstance;
        public InventoryItemLibrary inventoryItemLibrary;
        public UIIconLibrary iconLibrary;
        private UI.Controls.ListView.ListView _listView;

        private void OnEnable()
        {
            if (_listView == null)
            {
                _listView = GetComponent<UI.Controls.ListView.ListView>();
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

        private ListViewItem GetItemByIndex(UI.Controls.ListView.ListView view, int index)
        {
            var first = view.ItemPrefabDataList[0];
            var newInstance = view.NewListViewItem(first.prefabObject.name);

            var inventoryUIComponent =
                newInstance.gameObject.GetComponent<InventoryItemInstanceUI>();
            var instance = characterInventoryInstance.items[index];

            inventoryUIComponent.Display(index, instance, characterInventoryInstance, iconLibrary);

            return newInstance;
        }
    }
}
