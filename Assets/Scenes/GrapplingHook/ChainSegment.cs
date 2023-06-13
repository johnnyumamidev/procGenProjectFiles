using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSegment : MonoBehaviour
{
    public GameObject connectedAbove, connectedBelow;
    void Start()
    {
        HingeJoint2D hingeJoint = GetComponent<HingeJoint2D>();
        connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        ChainSegment aboveSegment = connectedAbove.GetComponent<ChainSegment>();

        if(aboveSegment != null)
        {
            aboveSegment.connectedBelow = gameObject;
            float bottomConnectionPoint = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;
            hingeJoint.connectedAnchor = new Vector2(0, bottomConnectionPoint * -1);
        }
        else
        {
            hingeJoint.connectedAnchor = Vector2.zero;
        }
    }
}
