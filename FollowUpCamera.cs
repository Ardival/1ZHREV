using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUpCamera : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f); // Adjusted offset to include a negative Z value
    private float smoothTime = 1000f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    void FixedUpdate()
    {
        if (target == null)
        {
            Debug.LogError("FollowUpCamera: Target not assigned!");
            return;
        }

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Optionally, you can add code to make sure the camera always looks at the target
        transform.LookAt(target.position);
    }
}