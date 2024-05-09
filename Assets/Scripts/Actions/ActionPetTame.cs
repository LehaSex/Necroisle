using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{

    /// <summary>
    /// Use to tame a pet, tamed pet will follow the player that tamed them
    /// </summary>

    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/PetTame", order = 50)]
    public class ActionPetTame : SAction
    {
        public override void DoAction(PlayerController character, Selectable select)
        {
            Pet pet = select.GetComponent<Pet>();
            pet.TamePet(character);
        }

        public override bool CanDoAction(PlayerController character, Selectable select)
        {
            Pet pet = select.GetComponent<Pet>();
            return pet != null && !pet.HasMaster();
        }
    }

}
