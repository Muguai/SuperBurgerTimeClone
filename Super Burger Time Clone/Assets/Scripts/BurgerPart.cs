using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerPart : MonoBehaviour
{
    public int timesToMoveDown = 3;
    public float speedToMoveDown = 1;
    public LayerMask collisionWhenFalling;
    
    private int timesMoved = 0;
    private bool notCurrentlyMoving = true;
    private BoxCollider2D boxCollider2D;
    private float amountToMoveDown;
    private Vector2 target;
    private bool notHit = true;
    private GameObject currentOccupiedPlattform;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        amountToMoveDown = (boxCollider2D.bounds.extents.y * 2) / timesToMoveDown;
        float distance = 1000;
        RaycastHit2D[] allHits;
        allHits = Physics2D.RaycastAll(transform.position, -Vector2.up, 10f);
        if(allHits.Length > 1)
        {
            currentOccupiedPlattform = allHits[1].transform.gameObject;
            transform.position = transform.position - new Vector3(0f, allHits[1].distance - boxCollider2D.bounds.extents.y, 0f);
        }
        

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
        }
    }

    public void MoveDown()
    {
        if (notCurrentlyMoving)
        {
            amountToMoveDown = (boxCollider2D.bounds.extents.y * 2) / timesToMoveDown;
            foreach(Transform c in this.transform)
            {
                if(c.gameObject.tag == "BurgerPart")
                {
                    amountToMoveDown += (c.GetComponent<BoxCollider2D>().bounds.extents.y * 2) / timesToMoveDown;
                }
            }

            float actualMoveDownAmount = amountToMoveDown * (transform.childCount + 1);
            target = transform.position - new Vector3(0f, amountToMoveDown, 0f);

            notCurrentlyMoving = false;
            StartCoroutine(dadada());
        }
    }

    IEnumerator dadada()
    {
        while (Vector2.Distance(transform.position, target) > 0.001f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speedToMoveDown * Time.deltaTime);
         //   Physics2D.MoveTowards()
            yield return new WaitForEndOfFrame();
        }
        notCurrentlyMoving = true;

        timesMoved++;
        if(timesMoved == timesToMoveDown)
        {
            notCurrentlyMoving = false;

            notHit = true;
            StartCoroutine(MoveDownTilNextBurger());
            

        }


    }
    IEnumerator MoveDownTilNextBurger()
    {
        while (notHit)
        {
            transform.position = transform.position - new Vector3(0f, 2f * speedToMoveDown * Time.deltaTime, 0f);
            RaycastHit2D[] allHits;
            if (transform.childCount == 0)
            {

                allHits = Physics2D.RaycastAll(transform.position, -Vector2.up, 0.1f, collisionWhenFalling);
               
            }
            else
            {
                allHits = Physics2D.RaycastAll(transform.GetChild(transform.childCount - 1).position, -Vector2.up, 0.1f, collisionWhenFalling);
            }
            if (allHits.Length > 1)
            {

                foreach (RaycastHit2D hit in allHits)
                {
                    if (hit.collider.gameObject != this.gameObject && hit.collider.gameObject != currentOccupiedPlattform)
                    {
                        currentOccupiedPlattform = hit.collider.gameObject;
                        Debug.Log(hit.collider.gameObject.name);
                        transform.position = transform.position - new Vector3(0f, hit.distance - boxCollider2D.bounds.extents.y, 0f);
                        notHit = false;
                        if (hit.collider.gameObject.tag == "BurgerPart")
                        {
                            hit.collider.transform.SetParent(this.transform);
                        }
                    }

                }
                    
                    
                
               

            }
            //   Physics2D.MoveTowards()
            yield return new WaitForEndOfFrame();

        }

        timesMoved = 0;

        notCurrentlyMoving = true;

    }

}
