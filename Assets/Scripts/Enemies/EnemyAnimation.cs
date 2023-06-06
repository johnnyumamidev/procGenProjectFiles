using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
}
