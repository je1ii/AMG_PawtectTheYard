using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private float moveAmount = 100f;
    [SerializeField] private float edgeSize = 30f;

    private Vector3 cameraPosition;
    
    void Start()
    {
        cameraPosition = transform.position;
        cameraMovement.SetUp(() => cameraPosition);
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if(cameraPosition.x < 10f)
                cameraPosition.x += moveAmount * Time.deltaTime;
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            if(cameraPosition.x > -10f)
                cameraPosition.x -= moveAmount * Time.deltaTime;
            
        }
        if (Input.mousePosition.x > Screen.width - edgeSize)
        {
            if(cameraPosition.x < 10f)
                cameraPosition.x += moveAmount * Time.deltaTime;
            
        }
        if (Input.mousePosition.x < edgeSize)
        {
            if(cameraPosition.x > -10f)
                cameraPosition.x -= moveAmount * Time.deltaTime;
            
        }
    }
}
