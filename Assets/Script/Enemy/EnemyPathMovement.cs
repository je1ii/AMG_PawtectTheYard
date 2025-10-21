using UnityEngine;

public class EnemyPathMovement : MonoBehaviour
{
    [Header("Path Points")]
    public Transform path1;
    public Transform path2;
    public Transform path3;
    public Transform path4;

    [Header("Movement")]
    public float speed = 1f;

    [Header("V Curve")]
    public float curveDepth = 6f; // how deep the V cruve

    private float totalPathLength;
    private float traveledDistance = 0f;

    void Start()
    {
        // Calculate total path length (approximate V curve)
        float vLength = Vector3.Distance(path2.position, (path2.position + path3.position) / 2f + Vector3.down * curveDepth)
                      + Vector3.Distance((path2.position + path3.position) / 2f + Vector3.down * curveDepth, path3.position);

        totalPathLength = Vector3.Distance(path1.position, path2.position) + vLength + Vector3.Distance(path3.position, path4.position);

 
        transform.position = path1.position;
    }

    void Update()
    {
        float delta = speed * Time.deltaTime;
        traveledDistance += delta;
        if (traveledDistance >= totalPathLength)
        {
            transform.position = path4.position;
            Destroy(gameObject);
            return;
        }

        // Distances of segments
        float dist12 = Vector3.Distance(path1.position, path2.position);
        float dist23 = Vector3.Distance(path2.position, (path2.position + path3.position) / 2f + Vector3.down * curveDepth)
                      + Vector3.Distance((path2.position + path3.position) / 2f + Vector3.down * curveDepth, path3.position);
        float dist34 = Vector3.Distance(path3.position, path4.position);

        if (traveledDistance <= dist12)
        {
            float t = traveledDistance / dist12;
            transform.position = Vector3.Lerp(path1.position, path2.position, t);
        }
        else if (traveledDistance <= dist12 + dist23)
        {
            float t = (traveledDistance - dist12) / dist23;
            Vector3 start = path2.position;
            Vector3 end = path3.position;
            Vector3 control = (start + end) / 2f + Vector3.down * curveDepth;
            transform.position = Mathf.Pow(1 - t, 2) * start + 2 * (1 - t) * t * control + Mathf.Pow(t, 2) * end;
        }
        else
        {
            float t = (traveledDistance - dist12 - dist23) / dist34;
            transform.position = Vector3.Lerp(path3.position, path4.position, t);
        }
    }
}
