using UnityEngine;

public class Ball : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;
    [SerializeField] Paddle paddle1;
    [SerializeField] AudioClip[] ballSounds;
     
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
        float maxRotate = 10f;
        float minRotate = 2f;

        if (hasStarted)
        {

            float vectorAngle = Vector2.Angle(thisRigidBody2D.velocity, new Vector2(1f, 0f)); // Angle from horizontal
            float modVal = minRotate + Mathf.Abs(Mathf.Cos(2 * Mathf.Deg2Rad * vectorAngle)) * (maxRotate - minRotate);
            float randomAngle = Random.Range(0, modVal);
            float rotation = Vector2.SignedAngle(thisRigidBody2D.velocity, relVelocityEnterCollision); // rotation from before to after collision
            float sign = rotation >= 0 ? 1 : -1;
            thisRigidBody2D.velocity = Quaternion.Euler(0, 0, sign * randomAngle) * thisRigidBody2D.velocity;

            AudioClip clip = ballSounds[Random.Range(0, ballSounds.Length)];
            thisAudioSource.PlayOneShot(clip);
        }
    }

}
