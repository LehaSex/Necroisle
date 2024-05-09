using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{

    public class CameraFX : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            transform.position = CameraController.Get().GetTargetPos();
            transform.rotation = Quaternion.LookRotation(CameraController.Get().GetFacingFront(), Vector3.up);
        }
    }

}
