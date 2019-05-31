using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class FloorColliders : MonoBehaviour
{
    private EdgeCollider2D col;
    private LineRenderer lineRenderer;
    public float widthModifier;
    // Start is called before the first frame update
    void Awake()
    {
        col = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();

        int posCount = lineRenderer.positionCount;
        Vector3[] rendererPositions = new Vector3[posCount];
        Vector2[] colPositions = new Vector2[posCount];

        float[] widths = new float[lineRenderer.widthCurve.keys.Length];

        int c = 0;
        foreach (Keyframe w in lineRenderer.widthCurve.keys) {
            widths[c] = w.value;
            c++;
        }

        lineRenderer.GetPositions(rendererPositions);
        int i = 0;
        foreach (Vector3 p in rendererPositions) {
            colPositions[i] = new Vector2(p.x, p.y + lineRenderer.startWidth * widthModifier);
            // if we add climb points, do them here.. off now cause player automatically detects the top of walls
            i++;

        }

        col.points = colPositions;

    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void AddClimbPoints() {
    //    if (
    //           (i == 0 || i == rendererPositions.Length - 1) || (
    //            /*valid place*/i > 0 && i < rendererPositions.Length - 2 &&
    //            (
    //                /*UP THEN RIGHT:*/
    //                (/*went up*/ p.y - rendererPositions[i - 1].y > 0 &&
    //                /*then right*/ p.x - rendererPositions[i + 1].x < 0
    //                )
    //            ||  /*OR*/
    //                /*UP THEN LEFT:*/
    //                (/*go down next*/ p.y - rendererPositions[i + 1].y > 0 &&
    //                /*went right last*/ p.x - rendererPositions[i - 1].x > 0
    //                )
    //            )
    //            )
    //       )
    //    {
    //        //climbPoints.Add(GameObject.Instantiate(climbPoint, p, Quaternion.identity)); // put a climb point
    //    }
    //}
}
