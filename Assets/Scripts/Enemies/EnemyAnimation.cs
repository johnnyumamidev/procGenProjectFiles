using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Transform enemyParentObject;
    EnemyAI enemyAi;
    EnemyMovement enemyMovement;
    public Animator animator;
    int animatorIndex;
    public List<string> animationStates;

    Sprite sprite;
    SpriteMask spriteMask;

    public GameObject corpse;

    public GameEvent enemyLungeEvent;
    private void Awake()
    {
        if(enemyLungeEvent == null) 
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

        enemyMovement = GetComponentInParent<EnemyMovement>();
        enemyAi = GetComponentInParent<EnemyAI>();

        if(animator == null) animator = GetComponent<Animator>();
    }

    private void Update()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        spriteMask.sprite = sprite;

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
        enemyLungeEvent.Raise();
    }

    private void SpawnCorpse()
    {
        GameObject _corpse = Instantiate(corpse, transform.position, Quaternion.identity);
        SpriteRenderer corpseSprite = _corpse.GetComponent<SpriteRenderer>();
        corpseSprite.sprite = sprite;
        Destroy(enemyParentObject.gameObject);
    }
}
