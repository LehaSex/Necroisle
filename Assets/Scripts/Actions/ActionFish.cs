using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    /// <summary>
    /// Use your fishing rod to fish a fish!
    /// </summary>
    
    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/Fish", order = 50)]
    public class ActionFish : SAction
    {
        public GroupData fishing_rod;
        public float fish_time = 3f; //In seconds

        public override void DoAction(PlayerController character, Selectable select)
        {
            if (select != null)
            {
                character.FaceTorward(select.transform.position);

                ItemProvider pond = select.GetComponent<ItemProvider>();
                if (pond != null)
                {
                    if (pond.HasItem())
                    {
                        character.FishItem(pond, 1, fish_time);
                        character.Attributes.GainXP("fishing", 10); //Example of XP gain
                    }
                }
            }
        }

        public override bool CanDoAction(PlayerController character, Selectable select)
        {
            ItemProvider pond = select.GetComponent<ItemProvider>();
            return pond != null && pond.HasItem() && character.EquipData.HasItemInGroup(fishing_rod) && !character.IsSwimming();
        }
    }

}