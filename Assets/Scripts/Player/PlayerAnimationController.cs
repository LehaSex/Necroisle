using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public string move_anim = "Move";
        public string move_side_x = "MoveX";
        public string move_side_z = "MoveZ";
        public string idle_anim = "Idle";

        // Start is called before the first frame update
        private Animator animator;
        // refactor
        private PlayerController character;

        void Awake()
        {
            character = GetComponent<PlayerController>();
            animator = GetComponentInChildren<Animator>();

            if (animator == null)
                enabled = false;
        }

        void Start()
        {
            Application.targetFrameRate = 0;
        }

        // Update is called once per frame
        void Update()
        {
/*             bool player_paused = GameManager.Get().IsPausedByPlayer();
            bool gameplay_paused = GameManager.Get().IsPausedByScript();
            animator.enabled = !player_paused; */

            if (animator.enabled)
            {
                SetAnimBool(move_anim, character.IsMoving());
                /* SetAnimBool(move_anim, !gameplay_paused && character.IsMoving()); */
                Vector3 move_vect = character.GetMoveNormalized();
                float mangle = Vector3.SignedAngle(character.GetFacing(), move_vect, Vector3.up);
                Vector3 move_side = new Vector3(Mathf.Sin(mangle * Mathf.Deg2Rad), 0f, Mathf.Cos(mangle * Mathf.Deg2Rad));
                move_side = move_side * move_vect.magnitude;
                SetAnimFloat(move_side_x, move_side.x);
                SetAnimFloat(move_side_z, move_side.z);
            }
        }

        public void SetAnimBool(string id, bool value)
        {
            if (!string.IsNullOrEmpty(id))
                animator.SetBool(id, value);
        }

        public void SetAnimFloat(string id, float value)
        {
            if (!string.IsNullOrEmpty(id))
                animator.SetFloat(id, value);
        }


    }
}
