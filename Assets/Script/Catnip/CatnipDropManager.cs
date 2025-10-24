using UnityEngine;

public class CatnipDropManager : MonoBehaviour
{
    public static CatnipDropManager Instance;

    public RectTransform uiTargetIcon;

    void Awake()
    {
        if (Instance == null) Instance = this;
        //else Destroy(gameObject);
    }

    public Vector3 GetUITargetWorldPosition()
    {
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, uiTargetIcon.position);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane + 1f));
        return worldPos;
    }
}
