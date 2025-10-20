using UnityEngine;

public class CatnipPickupSystem : MonoBehaviour
{
    public CatnipData catnipData;
    public float moveSpeed = 3f;
    private bool isMoving = false;
    private Vector3 targetWorldPos;
    
    public void InitializeDrop(CatnipData dropData)
    {
        catnipData = dropData;
    }

    void Update()
    {
        if(!isMoving) return;

        transform.position = Vector3.Lerp(transform.position, targetWorldPos, Time.deltaTime * moveSpeed);

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.1f)
        {
            isMoving =false;
            OnReachUI();
        }
    }

    void OnMouseDown()
    {
        if (!isMoving)
        {
            targetWorldPos = CatnipDropManager.Instance.GetUITargetWorldPosition();
            isMoving = true;

            Debug.Log("Clicked drop!");
        }
    }

    private void OnReachUI()
    {
        CatnipManager.Instance.AddCatnip(catnipData.catnipValue);
        Destroy(gameObject);
    }
}
