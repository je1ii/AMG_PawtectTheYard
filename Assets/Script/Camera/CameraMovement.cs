using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] PauseMenu pauseMenu;
    
    private Func<Vector3> getCameraPositionFunc;

    public void SetUp(Func<Vector3> cameraPosFunc)
    {
        getCameraPositionFunc = cameraPosFunc;
    }

    public void GetCameraPositionFunc(Func<Vector3> cameraPosFunc)
    {
        getCameraPositionFunc = cameraPosFunc;
    }

    void Update()
    {
        if(!pauseMenu.GetIsPaused())
            CameraMove();
    }

    private void CameraMove()
    {
        if (getCameraPositionFunc == null) return;
        
        var cameraPosition = getCameraPositionFunc();
        cameraPosition.z = transform.position.z;
        
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, -10f, 10f);
        
        var cameraMoveDir = (cameraPosition - transform.position).normalized;
        var distance = Vector3.Distance(cameraPosition, transform.position);
        var cameraMoveSpeed = 1f;
        
        if (distance > 0)
        {
            var newCameraPosition = transform.position + cameraMoveDir * (distance * cameraMoveSpeed * Time.unscaledDeltaTime);
            
            newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, -10f, 10f);
            
            var distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraPosition);
        
            if (distanceAfterMoving > distance)
            {
                // overshot
                newCameraPosition = cameraPosition;
            }
            
            transform.position = new Vector3(newCameraPosition.x, 0, -10);
        }
    }
}
