/*
 * Script gets player's intended velocity & displacement (caused by enviroment variables + user input which is taken from PlayerInput)
 * See for equations/physics: https://en.wikipedia.org/wiki/Equations_of_motion
 * See: http://lolengine.net/blog/2011/12/14/understanding-motion-in-games for Verlet integration vs. Euler
 */

using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerVelocity : MonoBehaviour
{

	[SerializeField] [Range(0f, 15f)] private float moveSpeed = 6;
	[SerializeField] [Range(0f, 15f)] private float climbSpeed = 6;
	[SerializeField] [Range(0f, 15f)] private float maxJumpHeight = 4;
	[SerializeField] [Range(0f, 5f)] private float  minJumpHeight = 1;
	[SerializeField] [Range(0f, 1f)] private float timeToJumpApex = .4f;
	[SerializeField] [Range(0f, 1f)] private float accelerationTimeAirborne = .2f;
	[SerializeField] [Range(0f, 1f)] private float accelerationTimeGrounded = .1f;
	[SerializeField] [Range(0f, 1f)] private float accelerationTimeClimb = .1f;
	[SerializeField] private float forceFallSpeed = 20;
	[SerializeField] private bool playerCanGrabWall = false;

	[SerializeField] private Vector2 wallJump;
	[SerializeField] private Vector2 wallJumpClimb;
	[SerializeField] private Vector2 wallLeapOff;

	[SerializeField] private float wallSlideSpeedMax = 3;
	[SerializeField] private float wallStickTime = .25f;

	[HideInInspector] public float gravity;

	[HideInInspector] public Vector3 velocity;
	[HideInInspector] public Vector3 oldVelocity;

	[HideInInspector] public float gravityIncrease = 1;
	[HideInInspector] public bool climbing;
	[HideInInspector] public bool coyoteTime;
	[HideInInspector] public bool wallContact;
	[HideInInspector] public bool holdWallContact;
	[HideInInspector] public Vector2 directionalInput;


	private float timeToWallUnstick;
	private float maxJumpVelocity;
	private float minJumpVelocity;
	private float velocityXSmoothing;
	private float velocityYSmoothing;


	private Movement playerMovement;
	private PlayerInput playerInput;
	private SpriteRenderer spriteRenderer;

	private int wallDirX;


	void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		playerMovement = GetComponent<Movement>();
		spriteRenderer = GetComponent<PlayerStateMachine>().SpriteObject.GetComponent<SpriteRenderer>();

		// see suvat calculations; s = ut + 1/2at^2, v^2 = u^2 + 2at, where u=0, scalar looking at only y dir
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	void Update()
	{
		CalculateVelocity();
		HandleWallSliding();

		// r = r0 + 1/2(v+v0)t, note Vector version used here
		// displacement = 1/2(v+v0)t since the playerMovementController uses Translate which moves from r0
		Vector3 displacement = (velocity + oldVelocity) * 0.5f * Time.deltaTime;
		// Move player using movement controller which checks for collisions then applies correct transform (displacement) translation
		playerMovement.Move(displacement, directionalInput);


        if (!wallContact)
        {
			if (displacement.x < 0)
				spriteRenderer.flipX = true;
			else if (displacement.x > 0)
				spriteRenderer.flipX = false;
		}

		bool verticalCollision = playerMovement.collisionDirection.above || playerMovement.collisionDirection.below;

		if (verticalCollision)
		{
			if (playerMovement.slidingDownMaxSlope)
			{
				velocity.y += playerMovement.collisionAngle.slopeNormal.y * -gravity * Time.deltaTime;
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
		float smoothTime = (playerMovement.collisionDirection.below) ? accelerationTimeGrounded : accelerationTimeAirborne;
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

	void HandleWallSliding()
	{
		wallDirX = (playerMovement.collisionDirection.left) ? -1 : 1;
		spriteRenderer.flipX = (playerMovement.collisionDirection.left) ? false : true;

		bool horizontalCollision = playerMovement.collisionDirection.left || playerMovement.collisionDirection.right;

		if (horizontalCollision && !playerMovement.collisionDirection.below && !playerMovement.forceFall && playerMovement.collisionAngle.onWall)
		{
			wallContact = true;

			// Check if falling down - only wall slide then
			if (velocity.y < 0)
			{
				// Grab wall if input facing wall
				if (directionalInput.x == wallDirX && playerCanGrabWall)
				{
					holdWallContact = true;
					velocity.y = 0;
				}
				else
				{
					holdWallContact = false;

					// Only slow down if falling faster than slide speed
					if (velocity.y < -wallSlideSpeedMax)
					{
						velocity.y = -wallSlideSpeedMax;
					}

					// Stick to wall until timeToWallUnstick has counted down to 0 from wallStickTime
					if (timeToWallUnstick > 0)
					{
						velocityXSmoothing = 0;
						velocityYSmoothing = 0;
						velocity.x = 0;

						if (directionalInput.x != wallDirX && directionalInput.x != 0)
						{
							timeToWallUnstick -= Time.deltaTime;
						}
						else
						{
							timeToWallUnstick = wallStickTime;
						}
					}
					else
					{
						timeToWallUnstick = wallStickTime;
					}
				}
			}

		} else
		{
			wallContact = false;
		}

	}

	/* Public Functions used by PlayerInput script */

	/// <summary>
	/// Handle horizontal movement input
	/// </summary>
	public void SetDirectionalInput(Vector2 input)
	{
		directionalInput = input;
	}

	/// <summary>
	/// Handle jumps
	/// </summary>
	public void OnJumpInputDown()
	{
		playerInput.jumpPressedReminder = 0f;
		if (wallContact)
		{
			// Standard wall jump
			if (directionalInput.x == 0)
			{
				velocity.x = -wallDirX * wallJump.x;
				velocity.y = wallJump.y;
			}
			// Climb up if input is facing wall
			else if (wallDirX == directionalInput.x)
			{
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			// Leap wall if input facing away from wall
			else
			{
				velocity.x = -wallDirX * wallLeapOff.x;
				velocity.y = wallLeapOff.y;
			}
		}
		if (playerMovement.collisionDirection.below || climbing || coyoteTime)
		{
			coyoteTime = false;
			if (playerMovement.slidingDownMaxSlope)
			{
				// Jumping away from max slope dir
				if (directionalInput.x != -Mathf.Sign(playerMovement.collisionAngle.slopeNormal.x))
				{ 
					velocity.y = maxJumpVelocity * playerMovement.collisionAngle.slopeNormal.y;
					velocity.x = maxJumpVelocity * playerMovement.collisionAngle.slopeNormal.x;
				}
			}
			else
			{
				velocity.y = maxJumpVelocity;
			}
		}
	}

	/// <summary>
	/// Handle not fully commited jumps - allow for mini jumps
	/// </summary>
	public void OnJumpInputUp()
	{
		if (velocity.y > minJumpVelocity)
		{
			velocity.y = minJumpVelocity;
		}
	}

	/// <summary>
	/// Handle down direction - force fall
	/// </summary>
	public void OnFallInputDown()
    {
		if (!playerMovement.collisionDirection.below)
		{
			velocity.y = -forceFallSpeed;
			playerMovement.forceFall = true;
		}
	}
}
