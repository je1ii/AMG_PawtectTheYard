using UnityEngine;

public class EnemyPathMovement : MonoBehaviour
{
    [Header("Path Points")]
    public Transform path1, path2, path3, path4;

    private float speed;
    private float curveDepth = 6f;

    // offset given by WaveManager so enemies don’t overlap
    private Vector3 startOffset = Vector3.zero;

    private float totalPathLength;
    private float traveledDistance = 0f;

    public void SetSpeed(float newSpeed) => speed = newSpeed;
    public void SetStartOffset(Vector3 offset) => startOffset = offset;

    void Start()
    {
        // Approximate total length for timing consistency
        float vLength = Vector3.Distance(path2.position, (path2.position + path3.position) / 2f + Vector3.down * curveDepth)
                      + Vector3.Distance((path2.position + path3.position) / 2f + Vector3.down * curveDepth, path3.position);

        totalPathLength = Vector3.Distance(path1.position, path2.position) + vLength + Vector3.Distance(path3.position, path4.position);

        transform.position = path1.position + startOffset;
    }

    void Update()
    {
        if (speed <= 0f) return;

        traveledDistance += speed * Time.deltaTime;
        if (traveledDistance >= totalPathLength)
        {
            Destroy(gameObject);
            return;
        }

        float dist12 = Vector3.Distance(path1.position, path2.position);
        float dist23 = Vector3.Distance(path2.position, (path2.position + path3.position) / 2f + Vector3.down * curveDepth)
                      + Vector3.Distance((path2.position + path3.position) / 2f + Vector3.down * curveDepth, path3.position);
        float dist34 = Vector3.Distance(path3.position, path4.position);

        Vector3 pathPos;

        if (traveledDistance <= dist12)
        {
            // Straight: Path1 → Path2
            float t = traveledDistance / dist12;
            pathPos = Vector3.Lerp(path1.position, path2.position, t);
        }
        else if (traveledDistance <= dist12 + dist23)
        {
            // Curved: Path2 → Path3 (each enemy’s own offset)
            float t = (traveledDistance - dist12) / dist23;

            Vector3 start = path2.position + startOffset;
            Vector3 end = path3.position + startOffset;

            // control point also gets the offset so curve shifts properly
            Vector3 control = (path2.position + path3.position) / 2f + Vector3.down * curveDepth + startOffset;

            // quadratic Bezier curve formula
            pathPos = Mathf.Pow(1 - t, 2) * start + 2 * (1 - t) * t * control + Mathf.Pow(t, 2) * end;
        }
        else
        {
            // Straight: Path3 → Path4
            float t = (traveledDistance - dist12 - dist23) / dist34;
            pathPos = Vector3.Lerp(path3.position + startOffset, path4.position + startOffset, t);
        }

        transform.position = pathPos;
    }
}
