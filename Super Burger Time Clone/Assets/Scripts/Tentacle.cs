using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int length;
    public LineRenderer lineRend;
    public Vector3[] segmentPoses;
    private Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    public float trailSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;

    public Transform stuckTarget;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
    }

    // Update is called once per frame
    void Update()
    {

        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDir.position;

        for(int i = 1; i < segmentPoses.Length; i++)
        {
            if(i == segmentPoses.Length - 1)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    segmentPoses[i] = Vector2.MoveTowards(segmentPoses[i], cursorPos, moveSpeed * Time.deltaTime);

                }
                else
                {
                    segmentPoses[i] = stuckTarget.transform.position;

                }

            }
            else
            {
                segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDir.right * targetDist, ref segmentV[i], smoothSpeed + i / trailSpeed);

            }
        }
        lineRend.SetPositions(segmentPoses);
    }
}
