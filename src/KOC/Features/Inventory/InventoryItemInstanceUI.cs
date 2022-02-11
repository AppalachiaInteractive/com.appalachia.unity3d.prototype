/*
using System;
using Appalachia.Core.Objects.Root;
using Appalachia.UI.Controls.Common;
using Appalachia.UI.Controls.Views.ListView;
using Appalachia.UI.Core.Icons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Features.Inventory
{
    public sealed class InventoryItemInstanceUI : AppalachiaBehaviour<InventoryItemInstanceUI>
    {
        #region Constants and Static Readonly

        private static readonly Vector2 _spritePivot = new Vector2(0.5f, 0.5f);

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] public CharacterInventoryInstance characterInventoryInstance;
        [NonSerialized] public ClickEventListener clickEventListener;

        [Header("Selected Item")]
        public GameObject selected;

        [Header("Unselected Item")]
        public GameObject unselected;

        public Image highlight;
        public Image highlight_unselected;
        public Image preview;
        [NonSerialized] public int index;
        public InventoryItemInstance inventoryItemInstance;
        [NonSerialized] public ListViewItem listViewItem;

        public TMP_Text primary;

        public TMP_Text primary_unselected;
        public TMP_Text secondary;
        public TMP_Text tertiary;
        [NonSerialized] public UIIconLibrary uiIconLibrary;

        #endregion

        public void Display(
            int index,
            InventoryItemInstance itemInstance,
            CharacterInventoryInstance inventoryInstance,
            UIIconLibrary iconLibrary)
        {
            this.index = index;
            inventoryItemInstance = itemInstance;
            characterInventoryInstance = inventoryInstance;
            uiIconLibrary = iconLibrary;
            clickEventListener = GetComponent<ClickEventListener>();
            listViewItem = GetComponent<ListViewItem>();

            clickEventListener.SetPointerDownHandler(go => Select());

            if (this.index == 0)
            {
                Select();
            }
            else
            {
                Unselect();
            }

            FormatUIItem(itemInstance);
        }

        public void FormatUIItem(InventoryItemInstance itemInstance)
        {
            primary.text = itemInstance.metadata.name;

            secondary.text = itemInstance.metadata.specialCategory == InventorySpecialCategory.None
                ? string.Empty
                : itemInstance.metadata.specialCategory.ToString();

            tertiary.text = itemInstance.metadata.subcategory.ToString();

            if (preview.sprite.texture != itemInstance.metadata.preview)
            {
                var spriteRect = new Rect(
                    Vector2.zero,
                    new Vector2(itemInstance.metadata.preview.width, itemInstance.metadata.preview.height)
                );

                preview.overrideSprite = Sprite.Create(
                    itemInstance.metadata.preview,
                    spriteRect,
                    _spritePivot
                );
            }

            primary_unselected.text = itemInstance.metadata.name;

            if (itemInstance.IsFavorited)
            {
                var favoriteIcon = uiIconLibrary.GetByValue(InventoryState.Favorited);

                var spriteRect = new Rect(
                    Vector2.zero,
                    new Vector2(favoriteIcon.icon.width, favoriteIcon.icon.height)
                );

                highlight.gameObject.SetActive(true);
                highlight.overrideSprite = Sprite.Create(favoriteIcon.icon, spriteRect, _spritePivot);
                highlight.color = favoriteIcon.tint;

                highlight_unselected.gameObject.SetActive(true);
                highlight_unselected.overrideSprite = Sprite.Create(
                    favoriteIcon.icon,
                    spriteRect,
                    _spritePivot
                );
                highlight_unselected.color = favoriteIcon.tint;
            }
            else
            {
                highlight.gameObject.SetActive(false);
                highlight_unselected.gameObject.SetActive(false);
            }
        }

        public void Select()
        {
            if (characterInventoryInstance.selectedItem)
            {
                characterInventoryInstance.selectedItem.Unselect();
            }

            characterInventoryInstance.selectedItem = this;

            selected.SetActive(true);
            unselected.SetActive(false);
        }

        public void Unselect()
        {
            selected.SetActive(false);
            unselected.SetActive(true);
        }
    }
}
*/


