using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpSquat : State
{
    PlayerStateMachine playerStateMachine;

    Animator m_Animator;
    string m_ClipName;
    AnimatorClipInfo[] m_CurrentClipInfo;

    float m_CurrentClipLength;

    public PlayerJumpSquat(PlayerStateMachine stateMachine) : base(stateMachine)
    {

        this.playerStateMachine = stateMachine;
    }

    public override IEnumerator Start()
    {
        playerStateMachine.anim.Play("JumpSquat", 0, 0f);
        playerStateMachine.playerInput.canMove = true;


        //   float length = playerStateMachine.anim.GetComponent<Animation>().clip.length;
        yield return new WaitForEndOfFrame();

        // float Length = playerStateMachine.anim.GetCurrentAnimatorStateInfo(0).length;
        float Length = 0.4f;

        float orglenght = Length;


        while (Length > 0f)
        {
            Length -= Time.deltaTime;
            if (playerStateMachine.CheckForLadder(false) && playerStateMachine.canClimb)
            {
                break;
            }
            if (Length < orglenght - 0.15f)
            {
                if (playerStateMachine.playerMovement.grounded == true)
                {
                    break;
                }

                if (playerStateMachine.playerVelocity.wallContact == true)
                {
                    break;
                }

            }
            yield return null;
        }

        if (playerStateMachine.CheckForLadder(false) && playerStateMachine.canClimb)
        {
            playerStateMachine.SetState(new PlayerClimb(playerStateMachine));
        }else if (playerStateMachine.playerVelocity.wallContact == true)
        {
            playerStateMachine.SetState(new PlayerWallSlide(playerStateMachine));
        }
        else
        {
            playerStateMachine.SetState(new PlayerFall(playerStateMachine, true));
        }


    }

    public override void Update()
    {

        if (playerStateMachine.playerInput.JumpInputUp)
        {
            playerStateMachine.playerVelocity.OnJumpInputUp();
        }
    }


}
