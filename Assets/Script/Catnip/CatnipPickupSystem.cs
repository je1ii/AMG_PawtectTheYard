using UnityEngine;

public class CatnipPickupSystem : MonoBehaviour
{
    public CatnipData catnipData;
    public float moveDuration = 0.5f;

    private bool isMoving = false;
    private Vector3 startPos;
    private Vector3 targetWorldPos;
    private Vector3 startScale;
    private Vector3 endScale = Vector3.zero;
    private float elapsedTime = 0f;

    public void InitializeDrop(CatnipData dropData)
    {
        catnipData = dropData;
    }

    void Update()
    {
        if (!isMoving) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveDuration);
        t = Mathf.SmoothStep(0f, 1f, t);

        transform.position = Vector3.Lerp(startPos, targetWorldPos, t);
        transform.localScale = Vector3.Lerp(startScale, endScale, t);

        if (t >= 1f)
        {
            isMoving = false;
            OnReachUI();
        }
    }

    void OnMouseDown()
    {
        if (!isMoving)
        {
            startPos = transform.position;
            startScale = transform.localScale;
            elapsedTime = 0f;

            Vector3 uiScreenPos = CatnipDropManager.Instance.GetUITargetWorldPosition(); 
            uiScreenPos.z = Camera.main.nearClipPlane + 1f; 
            targetWorldPos = Camera.main.ScreenToWorldPoint(uiScreenPos);

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


