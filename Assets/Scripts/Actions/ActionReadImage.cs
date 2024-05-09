using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    /// <summary>
    /// Read a note on an item
    /// </summary>
    

    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/ReadImage", order = 50)]
    public class ActionReadImage : SAction
    {
        public Sprite image;

        public override void DoAction(PlayerController character, ItemSlot slot)
        {
            ItemData item = slot.GetItem();
            if (item != null)
            {
                ReadPanel.Get(1).ShowPanel(item.title, image);
            }
        }

        public override bool CanDoAction(PlayerController character, ItemSlot slot)
        {
            return true;
        }
    }

}