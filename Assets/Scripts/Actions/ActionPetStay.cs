using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{

    /// <summary>
    /// Use to order a pet to stay (stop moving)
    /// </summary>

    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/PetStay", order = 50)]
    public class ActionPetStay : SAction
    {
        public override void DoAction(PlayerController character, Selectable select)
        {
            Pet pet = select.GetComponent<Pet>();
            pet.StopFollow();
        }

        public override bool CanDoAction(PlayerController character, Selectable select)
        {
            Pet pet = select.GetComponent<Pet>();
            return pet != null && pet.GetMaster() == character && pet.IsFollow();
        }
    }

}
