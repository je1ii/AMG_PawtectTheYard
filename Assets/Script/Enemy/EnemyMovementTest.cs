// No need to use this script :>



using UnityEngine;

public class EnemyMovementTest : MonoBehaviour
{
    public float speed = 5f;
    private CameraDamageEffect cameraEffect;

    void Start()
    {
        cameraEffect = Object.FindFirstObjectByType<CameraDamageEffect>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector3(moveX, moveY, 0) * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EndPath"))
        {
            if (cameraEffect != null)
                cameraEffect.TriggerDamageEffect();

            Destroy(gameObject);
        }
    }
}
