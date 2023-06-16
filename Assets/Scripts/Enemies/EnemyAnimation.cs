using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Transform enemyParentObject;
    EnemyAI enemyAi;
    EnemyStates enemyStates;
    public Animator animator;
    [SerializeField] int animatorIndex;
    public List<string> animationStates;

    Sprite sprite;
    SpriteMask spriteMask;

    public GameObject corpse;

    public GameEvent enemyLungeEvent;
    private void Awake()
    {
        if(enemyStates == null) enemyStates = GetComponentInParent<EnemyStates>();
        enemyAi = GetComponentInParent<EnemyAI>();
        if(animator == null) animator = GetComponent<Animator>();

        if (enemyLungeEvent == null) 
            enemyLungeEvent = new GameEvent();

        spriteMask = GetComponent<SpriteMask>();

        string enemyName = enemyParentObject.name;
        animationStates = new List<string>
        {
            enemyName + "Walk",
            enemyName + "Search",
            enemyName + "Walk",
            enemyName + "Attack",
            enemyName + "Death"
        };
    }

    private void Update()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        spriteMask.sprite = sprite;

        if (enemyStates.currentState == EnemyStates.State.Patrol) animatorIndex = 0;
        if (enemyStates.currentState == EnemyStates.State.Search) animatorIndex = 1;
        if (enemyStates.currentState == EnemyStates.State.Chase) animatorIndex = 2;
        if (enemyStates.currentState == EnemyStates.State.Attack) animatorIndex = 3;
        if (enemyStates.currentState == EnemyStates.State.Dead) animatorIndex = 4;

        animator.Play(animationStates[animatorIndex]);
    }

    private void Lunge()
    {
        enemyAi.velocity = Vector2.zero;
        enemyLungeEvent.Raise();
    }

    private void EndAttackState()
    {
        enemyStates.SetEnemyState(EnemyStates.State.Chase);
    }

    private void SpawnCorpse()
    {
        GameObject _corpse = Instantiate(corpse, transform.position, Quaternion.identity);
        SpriteRenderer corpseSprite = _corpse.GetComponent<SpriteRenderer>();
        corpseSprite.sprite = sprite;
        Destroy(enemyParentObject.gameObject);
    }
}
