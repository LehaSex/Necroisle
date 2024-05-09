using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    /// <summary>
    /// Action to attack a destructible (if the destructible cant be attack automatically)
    /// </summary>

    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/Attack", order = 50)]
    public class ActionAttack : SAction
    {
        public override void DoAction(PlayerController character, Selectable select)
        {
            if (select.GetDestructible())
            {
                character.Attack(select.GetDestructible());
            }
        }
    }

}