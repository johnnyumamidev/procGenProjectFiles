using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimation : MonoBehaviour, IEventListener
{
    Enemy enemy;
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
    [SerializeField] GameEvent projectileEvent;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
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
            enemyName + "Death",
            enemyName + "RangeAttack"
        };
    }

    private void Update()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        spriteMask.sprite = sprite;

        animator.Play(animationStates[animatorIndex]);

        if (enemyStates.currentState == EnemyStates.State.Patrol) animatorIndex = 0;
        if (enemyStates.currentState == EnemyStates.State.Search) animatorIndex = 1;
        if (enemyStates.currentState == EnemyStates.State.Chase) animatorIndex = 2;
        if (enemyStates.currentState == EnemyStates.State.Attack) animatorIndex = 3;
        if (enemyStates.currentState == EnemyStates.State.Dead) animatorIndex = 4;
        if (!enemy.enemyData.hasRangedWeapon) return;

        if (enemyStates.currentState == EnemyStates.State.RangedAttack)
        {
            animatorIndex = ProjectileAnimationState();
        }
    }

    private void Lunge()
    {
        enemyAi.velocity = Vector2.zero;
        enemyLungeEvent.Raise();
    }

    private void EndAttackState()
    {
        enemyStates.SetEnemyState(EnemyStates.State.Chase);
        if(isFiringProjectile) isFiringProjectile = false;
    }

    private void SpawnCorpse()
    {
        GameObject _corpse = Instantiate(corpse, transform.position, Quaternion.identity);
        SpriteRenderer corpseSprite = _corpse.GetComponent<SpriteRenderer>();
        corpseSprite.sprite = sprite;
        Destroy(enemyParentObject.gameObject);
    }

    bool isFiringProjectile = false;
    public int ProjectileAnimationState()
    {
        if (!isFiringProjectile) return 0;
        else { return 5; }
    }

    private void OnEnable()
    {
        projectileEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        projectileEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        if(gameEvent == projectileEvent) isFiringProjectile = true;
    }
}
