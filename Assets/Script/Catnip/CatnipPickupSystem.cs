using UnityEngine;

public class CatnipPickupSystem : MonoBehaviour
{
    public CatnipData catnipData;
    
    public float moveSpeed = 0.1f;
    private bool isMoving = false;
    private Vector3 targetWorldPos;
    
    public void InitializeDrop(CatnipData dropData)
    {
        catnipData = dropData;
    }

    void Update()
    {
        if (!isMoving) return;

        int fastForwardSteps = 10; 
        float lerpFactor = Time.deltaTime * moveSpeed;

        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.zero;

        for (int i = 0; i < fastForwardSteps; i++)
        {
            transform.position = Vector3.Lerp(transform.position, targetWorldPos, lerpFactor);
            transform.localScale = Vector3.Lerp(transform.localScale, endScale, lerpFactor);

            if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
            {
                isMoving = false;
                OnReachUI();
                break; 
            }
        }
    }

    void OnMouseDown()
    {
        if (!isMoving)
        {
            targetWorldPos = CatnipDropManager.Instance.GetUITargetWorldPosition();
            isMoving = true;

            Debug.Log("Catnip has been picked");
        }
    }

    private void OnReachUI()
    {
        CatnipManager.Instance.AddCatnip(catnipData.catnipValue);
        Destroy(gameObject);
    }
}
