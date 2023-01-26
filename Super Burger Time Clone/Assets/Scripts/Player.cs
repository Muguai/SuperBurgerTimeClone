using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public LayerMask burgerLayer;
    public float checkGroundDistance = 0.1f;
    public float walkSpeed = 1f;
    public float jumpHeight = 10f;

    private RaycastHit2D hit;

    private Rigidbody2D rb;
    bool Grounded = false;
    bool moving;

    private Vector3 originalScale;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Horizontal") * walkSpeed * Time.deltaTime;

        moving = translation != 0f ? true : false;

       // transform.Translate(translation, 0f, 0f);

            if (translation > 0f )
            {
                spriteRenderer.flipX = false;
              //  transform.localScale = new Vector2(originalScale.x, originalScale.y);
            }
            if (translation < 0f )
            {
                 spriteRenderer.flipX = true;

            //  transform.localScale = new Vector2(-originalScale.x, originalScale.y);
            }




        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {

           // animator.ResetTrigger("Run");
           // animator.SetTrigger("JumpSquat");

           // rb.velocity = Vector2.up * jumpHeight;

        }


        if (moving)
        {
           // animator.SetTrigger("Run");

        }
        else
        {
          //  animator.ResetTrigger("Run");
        }

        if (Input.GetKeyDown(KeyCode.B) && Input.GetKey(KeyCode.DownArrow) && CheckForBurgerPart())
        {
             Debug.Log("DOWNB " + hit.collider.gameObject.name);
             hit.collider.gameObject.GetComponent<BurgerPart>().MoveDown();
        }
        

    }

    void OnCollisionStay2D(Collision2D collider)
    {
        CheckIfGrounded();
    }

    void OnCollisionExit2D(Collision2D collider)
    {
        Grounded = false;
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D[] hits;

        //We raycast down 1 pixel from this position to check for a collider
        Vector2 positionToCheck = transform.position;
        hits = Physics2D.RaycastAll(positionToCheck, Vector2.down, 1f);

        //if a collider was hit, we are grounded
        if (hits.Length > 0)
        {

            animator.ResetTrigger("JumpSquat");
            animator.ResetTrigger("Run");
          //  animator.ResetTrigger("Fall");

       //     animator.SetTrigger("Landed");
            Grounded = true;
        }
        else
        {
            Grounded = false;
            animator.ResetTrigger("JumpSquat");
            animator.ResetTrigger("Run");


            animator.SetTrigger("Fall");
        }
    }

    bool CheckForBurgerPart()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, checkGroundDistance, burgerLayer);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /*
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "BurgerPart")
        {
            Debug.Log("Add " + col.gameObject.name);
            touchingBurgerPart = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "BurgerPart")
        {
            Debug.Log("Remove " + col.gameObject.name);
            touchingBurgerPart = false;

            // burgerParts.Remove(col.gameObject);
        }
    }
    */
}
