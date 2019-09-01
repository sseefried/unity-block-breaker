using UnityEngine;

public class Ball : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;
    [SerializeField] Paddle paddle1;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float randomFactor = 0.2f;
     
    // State
    Vector2 paddleToBallVector;
    bool hasStarted = false;

    // Cached component references
    AudioSource thisAudioSource;
    Rigidbody2D thisRigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        paddleToBallVector = transform.position - paddle1.transform.position;
        thisAudioSource = GetComponent<AudioSource>();
        thisRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
    }

    public void MoveBallBackToPaddle()
    {
        transform.position = paddle1.transform.position;
        hasStarted = false;
        thisRigidBody2D.velocity = new Vector2(0f, 0f);
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            thisRigidBody2D.velocity = new Vector2(xPush, yPush);
            hasStarted = true;
        }
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = paddle1.transform.position;
        transform.position = paddlePos + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        float randomAngle = Random.Range(-randomFactor, randomFactor);

        if (hasStarted)
        {
            AudioClip clip = ballSounds[Random.Range(0, ballSounds.Length)];
            // PlayOneShot will not be interrupted
            thisAudioSource.PlayOneShot(clip);
            thisRigidBody2D.velocity = Quaternion.Euler(0, 0, randomAngle) * thisRigidBody2D.velocity;
        }
    }


}
