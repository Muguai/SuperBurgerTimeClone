using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStateMachine : StateMachine
{

    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb2D;
    [HideInInspector] public Movement playerMovement;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerVelocity playerVelocity;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public bool canClimb = true;

    public GameObject SpriteObject;
    public LayerMask ladderMask;
    public float ladderDistance = 1f;
    [Range(0f, 1f)] public float ladderCdTimer = 0.4f;
    [Range(0f, 1f)] public float coyoteTime = 0.15f;
    [Range(1f, 5f)] public float SlideGravityIncrease = 1f;
    [Range(1f, 5f)] public float slideFinishBoost = 1f;



    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<Movement>();
        playerVelocity = GetComponent<PlayerVelocity>();
        spriteRenderer = SpriteObject.GetComponent<SpriteRenderer>();
        anim = SpriteObject.GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        this.canClimb = true;
        SetState(new PlayerIdle(this));
    }

    void Update()
    {
        state.Update();

    }

    void FixedUpdate()
    {
        state.FixedUpdate();

    }

    public IEnumerator LadderCooldown()
    {
        canClimb = false;
        float timer = ladderCdTimer;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        canClimb = true;

    }

    public bool CheckForLadder(bool currentlyClimbing)
    {

        if(Input.GetAxis("Vertical") != 0 || currentlyClimbing)
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                string state = GetCurrentState().GetType().ToString();
                if (state == "PlayerIdle" || state == "PlayerRun")
                {
                    return false;
                }
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, ladderDistance, ladderMask);
            return hit.collider != null;
        }
        else
        {
            return false;
        }
      
    }

    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawRay(transform.position, direction);
    }



}
