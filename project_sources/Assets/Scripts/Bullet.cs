using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 500f;
    public float maxLifeTime = 10f;
    private AudioSource audioSource;
    private Vector3 direction;
    private Rigidbody _rigidbody;

    public void Initialize(Vector3 dir)
    {
        direction = dir;
    }

    void Start()
    {
        // Destroy(gameObject, maxLifeTime);
        audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        // _rigidbody.velocity = direction * speed;
    }

    void Update()
    {

        _rigidbody.velocity = direction * speed;
        Destroy(gameObject, maxLifeTime);
        // transform.Translate(direction * speed * Time.deltaTime);
        // _rigidbody.AddForce()
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            audioSource.Play();
            ScoreManager.Instance.IncreaseScore(5);
            Destroy(gameObject, audioSource.clip.length); // Destroy after sound length
            Destroy(other.gameObject); 
        }
    }
}