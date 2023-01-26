using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVelocity : MonoBehaviour
{

    [SerializeField] [Range(0f, 15f)] private float moveSpeed = 6;
    [SerializeField] [Range(0f, 15f)] private float climbSpeed = 6;
    [SerializeField] [Range(0f, 15f)] private float maxJumpHeight = 4;
    [SerializeField] [Range(0f, 5f)] private float minJumpHeight = 1;
    [SerializeField] [Range(0f, 1f)] private float timeToJumpApex = .4f;
    [SerializeField] [Range(0f, 1f)] private float accelerationTimeAirborne = .2f;
    [SerializeField] [Range(0f, 1f)] private float accelerationTimeGrounded = .1f;
    [SerializeField] [Range(0f, 1f)] private float accelerationTimeClimb = .1f;

    [HideInInspector] public float gravity;

    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public Vector3 oldVelocity;

    [HideInInspector] public Vector2 directionalInput;

    private float maxJumpVelocity;
    private float minJumpVelocity;
    private Movement movement;
    private float velocityXSmoothing;
    private float velocityYSmoothing;

    [HideInInspector] public float gravityIncrease = 1;
    [HideInInspector] public bool climbing;


    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);

        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateVelocity();

        // r = r0 + 1/2(v+v0)t, note Vector version used here
        // displacement = 1/2(v+v0)t since the playerMovementController uses Translate which moves from r0
        Vector3 displacement = (velocity + oldVelocity) * 0.5f * Time.deltaTime;
        // Move player using movement controller which checks for collisions then applies correct transform (displacement) translation
        movement.Move(displacement, directionalInput);

        bool verticalCollision = movement.collisionDirection.above || movement.collisionDirection.below;

        if (verticalCollision)
        {
            if (movement.slidingDownMaxSlope)
            {
                velocity.y += movement.collisionAngle.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
    }

    void CalculateVelocity()
    {
        // suvat; s = ut, note a=0
        oldVelocity = velocity;

        float targetVelocityX = directionalInput.x * moveSpeed;

        // ms when player is on the ground faster vs. in air
        float smoothTime = (movement.collisionDirection.below) ? accelerationTimeGrounded : accelerationTimeAirborne;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, smoothTime);

        if (climbing)
        {
            float targetVelocityY = directionalInput.y * climbSpeed;
            velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTimeClimb);
        }
        else
        {
            velocity.y += (gravity * gravityIncrease) * Time.deltaTime;
        }

    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }
}
