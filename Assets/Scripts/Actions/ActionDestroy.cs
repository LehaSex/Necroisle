using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    /// <summary>
    /// Just destroy the destructible
    /// </summary>

    [CreateAssetMenu(fileName = "Action", menuName = "Necroisle/Actions/Destroy", order = 50)]
    public class ActionDestroy : AAction
    {
        public string animation;

        public override void DoAction(PlayerController character, Selectable select)
        {
            select.GetDestructible().KillIn(0.5f);
            character.TriggerAnim(animation, select.transform.position);
            character.TriggerAction(0.5f);
        }

        public override bool CanDoAction(PlayerController character, Selectable select)
        {
            return select.GetDestructible() && !select.GetDestructible().IsDead();
        }
    }

}