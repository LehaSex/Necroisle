using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{

    /// <summary>
    /// Use to order a pet to follow (follow the player, and attack)
    /// </summary>

    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/PetFollow", order = 50)]
    public class ActionPetFollow : SAction
    {
        public override void DoAction(PlayerController character, Selectable select)
        {
            Pet pet = select.GetComponent<Pet>();
            pet.Follow();
        }

        public override bool CanDoAction(PlayerController character, Selectable select)
        {
            Pet pet = select.GetComponent<Pet>();
            return pet != null && pet.GetMaster() == character && !pet.IsFollow();
        }
    }

}
