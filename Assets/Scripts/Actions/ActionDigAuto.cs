using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    /// <summary>
    /// Dig using the shovel, put this on a Selectable to auto dig on left click
    /// </summary>

    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/DigAuto", order = 50)]
    public class ActionDigAuto : AAction
    {
        public GroupData required_item;

        public override void DoAction(PlayerController character, Selectable select)
        {
            DigSpot spot = select.GetComponent<DigSpot>();
            if (spot != null)
            {
                string animation = character.Animation ? character.Animation.dig_anim : "";
                character.TriggerAnim(animation, spot.transform.position);
                character.TriggerProgressAction(1.5f, () =>
                {
                    spot.Dig();

                    InventoryItemData ivdata = character.EquipData.GetFirstItemInGroup(required_item);
                    if (ivdata != null)
                        ivdata.durability -= 1;
                });
            }
        }

        public override bool CanDoAction(PlayerController character, Selectable select)
        {
            return character.EquipData.HasItemInGroup(required_item);
        }
    }

}