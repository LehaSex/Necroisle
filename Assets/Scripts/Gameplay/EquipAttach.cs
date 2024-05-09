using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{

    /// <summary>
    /// A location on a character to attach equipment (like hand, head, feet, ...)
    /// </summary>

    public class EquipAttach : MonoBehaviour
    {
        public EquipSlot slot;
        public EquipSide side;
        public float scale = 1f;

        private PlayerController character;

        private void Awake()
        {
            character = GetComponentInParent<PlayerController>();
        }

        public PlayerController GetCharacter()
        {
            return character;
        }

    }

}