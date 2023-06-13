using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public float delayTime = 0.5f;
    public Rigidbody2D hookRigidBody;
    public GameObject chainSegmentPrefab;
    public int numberOfLinks = 5;
    void Start()
    {
        GenerateChain();
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(delayTime);
    }

    private void GenerateChain()
    {
        List<GameObject> chainSegments = new List<GameObject>();
        Rigidbody2D previousBody = hookRigidBody;
        for(int i = 0; i < numberOfLinks; i++)
        {
            GameObject newSegment = Instantiate(chainSegmentPrefab, transform.position, Quaternion.identity);
            newSegment.name = "Chain: " + i;
            newSegment.transform.parent = transform;
            HingeJoint2D hingeJoint = newSegment.GetComponent<HingeJoint2D>();
            hingeJoint.connectedBody = previousBody;

            previousBody = newSegment.GetComponent<Rigidbody2D>();
            chainSegments.Add(newSegment);
        }
        foreach(GameObject segment in chainSegments)
        {
            segment.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}
