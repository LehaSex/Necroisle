using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    /// <summary>
    /// Drop an item 
    /// </summary>
    
    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/Drop", order = 50)]
    public class ActionDrop : SAction
    {

        public override void DoAction(PlayerController character, ItemSlot slot)
        {
            InventoryData inventory = slot.GetInventory();
            character.Inventory.DropItem(inventory, slot.index);
        }
    }

}