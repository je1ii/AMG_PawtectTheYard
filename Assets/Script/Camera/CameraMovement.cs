using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour
{
    private Func<Vector3> getCameraPositionFunc;

    public void SetUp(Func<Vector3> cameraPosFunc)
    {
        getCameraPositionFunc = cameraPosFunc;
    }

    // public void GetCameraPositionFunc(Func<Vector3> cameraPosFunc)
    // {
    //     getCameraPositionFunc = cameraPosFunc;
    // }

    void Update()
    {
        var cameraPosition = getCameraPositionFunc();
        cameraPosition.z = transform.position.z;

        var cameraMoveDir = (cameraPosition - transform.position).normalized;
        var distance = Vector3.Distance(cameraPosition, transform.position);
        var cameraMoveSpeed = 1f;

        if (distance > 0)
        {
            var newCameraPosition = transform.position + cameraMoveDir * (distance * cameraMoveSpeed * Time.deltaTime);
            
            var distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraPosition);

            if (distanceAfterMoving > distance)
            {
                // overshot
                newCameraPosition = cameraPosition;
            }
            
            transform.position = newCameraPosition;
        }
    }
}
