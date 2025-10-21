using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform panel;       // The UI panel to move
    public float slideDistance = 150f; // How far it slides down when hidden
    public float slideSpeed = 20f;      // How fast it slides

    private Vector2 shownPosition;
    private Vector2 hiddenPosition;
    private Vector2 targetPosition;
    private bool isHovered = false;

    private void Start()
    {
        if (panel == null)
            panel = GetComponent<RectTransform>();

        shownPosition = panel.anchoredPosition;
        hiddenPosition = shownPosition - new Vector2(0, slideDistance);

        panel.anchoredPosition = hiddenPosition;
        targetPosition = hiddenPosition;
    }

    private void Update()
    {
        var t = Time.deltaTime * slideSpeed;
        t = Mathf.SmoothStep(0, 1, t);
        panel.anchoredPosition = Vector2.Lerp(panel.anchoredPosition, targetPosition, t);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        targetPosition = shownPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        targetPosition = hiddenPosition;
    }
}
