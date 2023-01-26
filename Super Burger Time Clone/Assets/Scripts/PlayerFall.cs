using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : State
{

    PlayerStateMachine playerStateMachine;
    bool didWeJump;

    public PlayerFall(PlayerStateMachine stateMachine, bool DidWeJump) : base(stateMachine)
    {
        this.playerStateMachine = stateMachine;
        this.didWeJump = DidWeJump;
    }

    // Start is called before the first frame update
    public override IEnumerator Start()
    {
        playerStateMachine.playerInput.canMove = true;
        playerStateMachine.anim.Play("Falling", 0, 0f);

        yield return null;

        if(didWeJump == false)
        {
            float lenght = playerStateMachine.coyoteTime;
            while (lenght > 0f)
            {
                lenght -= Time.deltaTime;
                if (playerStateMachine.playerInput.JumpInputDown)
                {
                    playerStateMachine.playerVelocity.coyoteTime = true;
                    playerStateMachine.playerVelocity.OnJumpInputDown();
                    playerStateMachine.SetState(new PlayerJumpSquat(playerStateMachine));
                    break;
                }
                yield return null;
            }
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if(playerStateMachine.playerMovement.grounded == true)
        {
            playerStateMachine.SetState(new PlayerIdle(playerStateMachine));
        }
        else if (playerStateMachine.CheckForLadder(false) && playerStateMachine.canClimb)
        {
            playerStateMachine.SetState(new PlayerClimb(playerStateMachine));
        }
        else if (playerStateMachine.playerVelocity.wallContact == true)
        {
            playerStateMachine.SetState(new PlayerWallSlide(playerStateMachine));
        }

        if (playerStateMachine.playerInput.JumpInputUp)
        {
            playerStateMachine.playerVelocity.OnJumpInputUp();
        }
    }
}
