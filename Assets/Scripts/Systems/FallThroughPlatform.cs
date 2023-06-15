using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThroughPlatform : MonoBehaviour
{
    PlatformEffector2D platformEffector;
    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        //if gameevent: player press down and jump while on platform
            //effector.rotationaloffset -= 180

        //after x seconds, offset += 180
    }
}
