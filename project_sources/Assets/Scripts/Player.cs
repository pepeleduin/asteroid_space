using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class Player : MonoBehaviour
{
    private float speed = 15.0f;
    private float rotationSpeed = 120.0f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    private Rigidbody rb;
    private Camera mainCamera;
    public static int lives = 3; // Static so it persists across scene reloads
    private AudioSource[] audioSources;
    private AudioSource bulletAudioSource;
    private AudioSource meteorHitAudioSource;
    private AudioSource gameOverAudioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ; // Ensure Z position is fixed
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        mainCamera = Camera.main;
        audioSources = GetComponents<AudioSource>();
        bulletAudioSource = audioSources[0]; 
        meteorHitAudioSource = audioSources[1]; 
        gameOverAudioSource = audioSources[2]; 
    }

    void Update()
    {
        float moveVertical = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        // Rotate around the Z-axis
        transform.Rotate(0, 0, -rotation);
        // Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 0, -rotation) * Time.fixedDeltaTime);
        // rb.MoveRotation(rb.rotation * deltaRotation);

        // Calculate forward direction based on current rotation
        Vector3 forwardDirection = transform.right;

        // Move in the forward direction
        rb.velocity = forwardDirection * moveVertical;
        //rb.AddForce(forwardDirection * moveVertical);

        // Shooting logic
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
            viewPos.x = Mathf.Clamp01(viewPos.x);
            viewPos.y = Mathf.Clamp01(viewPos.y);
            transform.position = mainCamera.ViewportToWorldPoint(viewPos);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            // Reduce lives and reset score because one life ended
            lives--;
            ScoreManager.Instance.ResetScore();

            if (lives <= 0)
            {
                Debug.Log("Game Over!");
                // Reset lives for the next game start
                lives = 3;
                gameOverAudioSource.Play();
                Destroy(collision.gameObject);
                
                StartCoroutine(ReloadSceneWithDelay(gameOverAudioSource.clip.length, "GameOverScene"));
            }
            else
            {
                // Reload the scene to start with a fresh life
                meteorHitAudioSource.Play();
                Destroy(collision.gameObject);
                StartCoroutine(ReloadSceneWithDelay(meteorHitAudioSource.clip.length, SceneManager.GetActiveScene().name));
            }
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Initialize(transform.right); // Set bullet direction
        bulletAudioSource.Play();
    }

    // Coroutine to reload the scene after a delay
    IEnumerator ReloadSceneWithDelay(float delay, string sceneName)
    {
        // Wait for the length of the audio clip
        yield return new WaitForSeconds(delay);

        
        SceneManager.LoadScene(sceneName);
    }
}