using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float maxLifeTime = 6f;

    void Start()
    {
        Destroy(gameObject, maxLifeTime);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ; // Ensure Z position is fixed
        rb.velocity = Vector3.down * speed;
    }
}