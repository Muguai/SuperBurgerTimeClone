using UnityEngine;

[RequireComponent(typeof(PlayerVelocity))]
public class PlayerInput : MonoBehaviour
{

	private PlayerVelocity playerVelocity;
	[HideInInspector] public bool JumpInputDown = false;
	[HideInInspector] public bool JumpInputUp = false;
	[HideInInspector] public bool InputActionB = false;
	[HideInInspector] public bool InputHoldDown = false;
	[HideInInspector] public bool canMove = true;
	[HideInInspector] public bool cantMoveX = false;
	[HideInInspector] public bool cantMoveY = false;

	[HideInInspector] public bool holdX = false;
	[HideInInspector] public float holdXValue;
	[SerializeField] private float jumpPressedReminderTime;
	[HideInInspector] public float jumpPressedReminder;




	void Start()
	{
		playerVelocity = GetComponent<PlayerVelocity>();
	}

	void Update()
	{
		Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (cantMoveX || !canMove)
			directionalInput.x = 0f;

		if (cantMoveY) 	
			directionalInput.y = 0f;

        if (holdX)
        {
			directionalInput.x = holdXValue;
		}
		

		playerVelocity.SetDirectionalInput(directionalInput);

		jumpPressedReminder -= Time.deltaTime;

		JumpInputDown = jumpPressedReminder > 0 ? true : false;

		JumpInputUp = Input.GetKeyUp(KeyCode.Space) ? true : false;

		InputHoldDown = Input.GetKey(KeyCode.S) ? true : false;

		InputActionB = Input.GetKeyDown(KeyCode.B) ? true : false;


		if (Input.GetKeyDown(KeyCode.Space))
		{
			jumpPressedReminder = jumpPressedReminderTime;
			//playerVelocity.OnJumpInputDown();
		}
		if (JumpInputUp)
		{
			//playerVelocity.OnJumpInputUp();
		}
		if (InputHoldDown)
		{
			// playerVelocity.OnFallInputDown();
		}
	}
}
