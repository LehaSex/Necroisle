using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Necroisle
{
    /// <summary>
    /// Use to allow storing item inside a construction (chest)
    /// Careful! This action will not work without a UniqueID set on the selectable
    /// </summary>

    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/Storage", order = 50)]
    public class ActionStorage : AAction
    {
        public override void DoAction(PlayerController character, Selectable select)
        {
            Storage storage = select.GetComponent<Storage>();
            if (storage != null)
            {
                storage.OpenStorage(character);
            }
        }

        public override bool CanDoAction(PlayerController character, Selectable select)
        {
            return select.GetComponent<Storage>() != null;
        }
    }

}
