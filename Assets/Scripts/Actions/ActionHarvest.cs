using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    /// <summary>
    /// Harvest the fruit of a plant
    /// </summary>
    
    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/Harvest", order = 50)]
    public class ActionHarvest : AAction
    {
        public override void DoAction(PlayerController character, Selectable select)
        {
            Plant plant = select.GetComponent<Plant>();
            if (plant != null)
            {
                string animation = character.Animation ? character.Animation.take_anim : "";
                character.TriggerAnim(animation, plant.transform.position);
                character.TriggerAction(0.5f, () =>
                {
                    plant.Harvest(character);
                });
            }
        }

        public override bool CanDoAction(PlayerController character, Selectable select)
        {
            Plant plant = select.GetComponent<Plant>();
            if (plant != null)
            {
                return plant.HasFruit();
            }
            return false;
        }
    }

}