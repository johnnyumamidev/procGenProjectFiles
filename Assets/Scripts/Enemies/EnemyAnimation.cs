using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    EnemyAI enemyAi;
    EnemyMovement enemyMovement;
    public Animator animator;
    int animatorIndex;
    public List<string> animationStates = new List<string>();
    private void Awake()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
        enemyAi = GetComponentInParent<EnemyAI>();
        if(animator == null) animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (enemyAi.currentState == EnemyAI.EnemyState.Patrol) animatorIndex = 0;
        if (enemyAi.currentState == EnemyAI.EnemyState.Search) animatorIndex = 1;
        if (enemyAi.currentState == EnemyAI.EnemyState.Chase) animatorIndex = 2;
        if (enemyAi.currentState == EnemyAI.EnemyState.Attack) animatorIndex = 3;
        if (enemyAi.currentState == EnemyAI.EnemyState.Dead) animatorIndex = 4;

        animator.Play(animationStates[animatorIndex]);
    }

    private void Lunge()
    {
        enemyMovement.SetLungeTrue();
        EventManager.instance.TriggerEvent("lunge");
    }
}
