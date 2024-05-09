using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    /// <summary>
    /// Read a note on an item
    /// </summary>
    

    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/Read", order = 50)]
    public class ActionRead : SAction
    {

        public override void DoAction(PlayerController character, ItemSlot slot)
        {
            ItemData item = slot.GetItem();
            if (item != null)
            {
                ReadPanel.Get().ShowPanel(item.title, item.desc);
            }

        }

        public override bool CanDoAction(PlayerController character, ItemSlot slot)
        {
            return true;
        }
    }

}