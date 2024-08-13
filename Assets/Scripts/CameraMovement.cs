using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [FormerlySerializedAs("player")] public Transform objectToFollow;
    public float smoothSpeed;

    private void LateUpdate()
    {
        if (objectToFollow != null)
        {
            CameraFollow();
        }
    }

    void CameraFollow()
    {
        Vector3 desiredPos = new Vector3(objectToFollow.position.x, transform.position.y, transform.position.z);
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothedPos;
    }
}