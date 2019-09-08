using UnityEngine;

public class Ball : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;
    [SerializeField] Paddle paddle1;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float minRotate = 2f; // degress
    [SerializeField] float maxRotate = 10f; //degrees
     
    // State
    Vector2 paddleToBallVector;
    bool hasStarted = false;
    Vector2 relVelocityEnterCollision;

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
        relVelocityEnterCollision = thisRigidBody2D.velocity;
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
        if (hasStarted)
        {
            /* Both horizontal and vertical angles are boring.
             * We minimise boredom by rotating the balls reflection angle
             * slightly by a random amount. When the ball is close to vertical
             * or horizontal the range of the random number is larger. When
             * it is close to a 45 degress angle the range is smaller.
             * It can never be less than minRotate and never more than maxRotate
             */

            float vectorAngle = Vector2.Angle(thisRigidBody2D.velocity, new Vector2(1f, 0f)); // Angle from horizontal
            // We use the cos function on twice vectorAngle so that horizontal/vertical (i.e. 0,90,180,270 degrees) yields a value of 1 and
            // any 45 degree value yields a zero.
            float scaleAmount = Mathf.Abs(Mathf.Cos(2 * Mathf.Deg2Rad * vectorAngle));
            float range = minRotate +  scaleAmount * (maxRotate - minRotate); // between minRotate and maxRotate
            float randomAngle = Random.Range(0, range);
            float rotation = Vector2.SignedAngle(thisRigidBody2D.velocity, relVelocityEnterCollision); // rotation from before to after collision
            float sign = rotation >= 0 ? 1 : -1;
            thisRigidBody2D.velocity = Quaternion.Euler(0, 0, sign * randomAngle) * thisRigidBody2D.velocity;

            AudioClip clip = ballSounds[Random.Range(0, ballSounds.Length)];
            thisAudioSource.PlayOneShot(clip);
        }
    }

}
