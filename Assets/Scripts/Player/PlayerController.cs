using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerController : MonoBehaviour
    {
        public int player_id = 0;
        [Header("Movement Settings")]
        public bool move_enabled = true; //Disable this if you want to use your own character controller
        public float move_speed = 4f;
        public float move_accel = 8; //Acceleration
        public LayerMask ground_layer = ~0;
        public float slope_angle_max = 45f; //Maximum angle, in degrees that the character can climb up
        public float ground_detect_dist = 0.1f; //Margin distance between the character and the ground, used to detect if character is grounded.
        public float moving_threshold = 0.15f; //Move threshold is how fast the character need to move before its considered movement (triggering animations, etc)
        public float fall_gravity = 40f; //Falling acceleration
        public float rotate_speed = 180f;
        public float fall_speed = 20f; //Falling speed

        private Rigidbody rb;
        private CapsuleCollider col;
        private bool is_grounded = false;
        private bool is_fronted = false;
        private bool is_action = false;
        private Vector3 move;
        private bool movement_enabled = true;
        private bool controls_enabled = true;
        private Vector3 facing;
        private Vector3 move_average;
        private Vector3 fall_vect;
        private Vector3 prev_pos;
        private Vector3 ground_normal = Vector3.up;
        private static PlayerController player_first = null;
        // Start is called before the first frame update
        
        void Awake()
        {
            if (player_first == null || player_id < player_first.player_id)
                player_first = this;

            rb = GetComponent<Rigidbody>();
            col = GetComponentInChildren<CapsuleCollider>();
            facing = transform.forward;
            prev_pos = transform.position;
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void FixedUpdate()
        {
            DetectGrounded();

            //Find the direction the character should move
            Vector3 tmove = FindMovementDirection();
            //Apply the move calculated previously
            move = Vector3.Lerp(move, tmove, move_accel * Time.fixedDeltaTime);
            rb.velocity = move;

            //Find facing direction
            Vector3 tfacing = FindFacingDirection();
            if (tfacing.magnitude > 0.5f)
                facing = tfacing;
            //Apply the facing
            Quaternion targ_rot = Quaternion.LookRotation(facing, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targ_rot, rotate_speed * Time.fixedDeltaTime));

            //Check the average traveled movement (allow to check if character is stuck)
            Vector3 last_frame_travel = transform.position - prev_pos;
            move_average = Vector3.MoveTowards(move_average, last_frame_travel, 1f * Time.fixedDeltaTime);
            prev_pos = transform.position;
        }

        //Detect if character is on the floor
        private void DetectGrounded()
        {
            float hradius = GetColliderHeightRadius();
            float radius = GetColliderRadius() * 0.9f;
            Vector3 center = GetColliderCenter();

            float gdist; Vector3 gnormal;
            is_grounded = PhysTool.DetectGround(transform, center, hradius, radius, ground_layer, out gdist, out gnormal);
            ground_normal = gnormal;

            float slope_angle = Vector3.Angle(ground_normal, Vector3.up);
            is_grounded = is_grounded && slope_angle <= slope_angle_max;
        }

        private void DetectFronted()
        {
            Vector3 scale = transform.lossyScale;
            float hradius = col.height * scale.y * 0.5f - 0.02f; //radius is half the height minus offset
            float radius = col.radius * (scale.x + scale.y) * 0.5f + 0.5f;

            Vector3 center = GetColliderCenter();
            Vector3 p1 = center;
            Vector3 p2 = center + Vector3.up * hradius;
            Vector3 p3 = center + Vector3.down * hradius;

            RaycastHit h1, h2, h3;
            bool f1 = PhysTool.RaycastCollision(p1, facing * radius, out h1);
            bool f2 = PhysTool.RaycastCollision(p2, facing * radius, out h2);
            bool f3 = PhysTool.RaycastCollision(p3, facing * radius, out h3);

            is_fronted = f1 || f2 || f3;

            //Debug.DrawRay(p1, facing * radius);
            //Debug.DrawRay(p2, facing * radius);
            //Debug.DrawRay(p3, facing * radius);
        }


        public float GetColliderHeightRadius()
        {
            Vector3 scale = transform.lossyScale;
            return col.height * scale.y * 0.5f + ground_detect_dist; //radius is half the height minus offset
        }

        public float GetColliderRadius()
        {
            Vector3 scale = transform.lossyScale;
            return col.radius * (scale.x + scale.y) * 0.5f;
        }

        public Vector3 GetColliderCenter()
        {
            Vector3 scale = transform.lossyScale;
            return col.transform.position + Vector3.Scale(col.center, scale);
        }

        private Vector3 FindMovementDirection()
        {
            Vector3 tmove = Vector3.zero;

            PlayerControls controls = PlayerControls.Get(player_id);
            Vector3 cam_move = CameraController.Get().GetRotation() * controls.GetMove();
            tmove = cam_move * GetMoveSpeed();

            //Stop moving if doing action
            if (is_action)
                tmove = Vector3.zero;

            if (!is_grounded)
            {
                fall_vect = Vector3.MoveTowards(fall_vect, Vector3.down * fall_speed, fall_gravity * Time.fixedDeltaTime);
                tmove += fall_vect;
            }
            //Add slope angle
            else if (is_grounded)
            {
                tmove = Vector3.ProjectOnPlane(tmove.normalized, ground_normal).normalized * tmove.magnitude;
            }

            return tmove;
        }

        public float GetMoveSpeed()
        {
/*             float boost = 1f + character_attr.GetBonusEffectTotal(BonusType.SpeedBoost);
            float base_speed = IsSwimming() ? character_swim.swim_speed : move_speed;
            return base_speed * boost * character_attr.GetSpeedMult(); */
            return move_speed;
        }

        public static PlayerController GetFirst()
        {
            return player_first;
        }

        private Vector3 FindFacingDirection()
        {
            PlayerControls controls = PlayerControls.Get(player_id);
            Vector3 tfacing = Vector3.zero;

            if (!IsMovementEnabled())
                return tfacing;

            //Calculate Facing
            if (IsMoving())
            {
                tfacing = new Vector3(move.x, 0f, move.z).normalized;
            }

            //Rotate character with right joystick when not in free rotate mode
            bool freerotate = CameraController.Get().IsFreeRotation();
            if (!freerotate && controls.IsGamePad())
            {
                Vector2 look = controls.GetFreelook();
                Vector3 look3 = CameraController.Get().GetRotation() * new Vector3(look.x, 0f, look.y);
                if (look3.magnitude > 0.5f)
                    tfacing = look3.normalized;
            }

            return tfacing;
        }

        public bool IsControlsEnabled()
        {
            return move_enabled && controls_enabled /* && !IsDead() && !TheUI.Get().IsFullPanelOpened() */;
        }

        //Can the character move? Or is it performing an action that prevents him from moving?
        public bool IsMovementEnabled()
        {
            return move_enabled && movement_enabled && !is_action /* && !IsDead() && !IsRiding() && !IsClimbing() */;
        }

        public bool IsMoving()
        {
/*             if (IsRiding() && character_ride.GetAnimal() != null)
                return character_ride.GetAnimal().IsMoving();
            if (Climbing && Climbing.IsClimbing())
                return Climbing.IsMoving(); */

            Vector3 moveXZ = new Vector3(move.x, 0f, move.z);
            return moveXZ.magnitude > GetMoveSpeed() * moving_threshold;
        }

        public void StopMove()
        {
            /* StopAutoMove(); */
            move = Vector3.zero;
            rb.velocity = Vector3.zero;
        }

        public Vector3 GetFacing()
        {
            return facing;
        }

        public Vector3 GetMoveNormalized()
        {
            return move.normalized * Mathf.Clamp01(move.magnitude / GetMoveSpeed());
        }

    }
}