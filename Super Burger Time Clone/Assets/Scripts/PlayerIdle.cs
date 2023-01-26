using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : State
{
    PlayerStateMachine playerStateMachine;

    public PlayerIdle(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.playerStateMachine = stateMachine;
    }

    public override IEnumerator Start()
    {
        playerStateMachine.playerInput.canMove = true;

        playerStateMachine.anim.Play("Idle", 0, 0f);
        yield return null;
    }

    public override void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            playerStateMachine.SetState(new PlayerRun(playerStateMachine));
        }else if (playerStateMachine.playerInput.JumpInputDown)
        {
            playerStateMachine.playerVelocity.OnJumpInputDown();
            playerStateMachine.SetState(new PlayerJumpSquat(playerStateMachine));

        }
        else if (playerStateMachine.playerMovement.grounded == false)
        {
            playerStateMachine.SetState(new PlayerFall(playerStateMachine, false));

        }else if (playerStateMachine.CheckForLadder(false) && playerStateMachine.canClimb)
        {
            playerStateMachine.SetState(new PlayerClimb(playerStateMachine));
            Debug.Log("Check");
        }else if(playerStateMachine.playerMovement.slidingDownMaxSlope == true)
        {
            if(Mathf.Abs(playerStateMachine.playerVelocity.velocity.y) > Mathf.Abs(playerStateMachine.playerVelocity.velocity.x))
            {
                playerStateMachine.SetState(new PlayerSlopeSlide(playerStateMachine));
            }

        }

        if (playerStateMachine.playerInput.InputActionB)
        {
            RaycastHit2D[] allHits;
            allHits = Physics2D.RaycastAll(playerStateMachine.transform.position, -Vector2.up, 1f);
            foreach( RaycastHit2D hit in allHits)
            {
                if (hit.collider.tag == "BurgerPart")
                {
                    hit.collider.gameObject.GetComponent<BurgerPart>().MoveDown();
                    break;
                }

            }
            Debug.Log(allHits.Length);

        }


    }
}
