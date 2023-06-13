using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Transform player;
    public float hookRange = 1f;
    [SerializeField] LayerMask obstacleLayer;

    public GameObject hookPrefab;

    public float shootForce = 15f;
    public float offset;
    public float cooldownLength = 0.5f;

    bool hookFired = false;

    Vector2 point;
    Vector2 playerPosition;
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.position;
        point = playerInput.aimDirection.normalized * hookRange;

        if (!hookFired && playerInput.performShoot != 0)
        {
            FireHook();
            hookFired = true;
            StartCoroutine(HookCooldown());
        }
    }

    private void FireHook()
    {
        Debug.Log("fire grappling hook");
        float theta = Mathf.Atan2(point.x, point.y);
        float pointRotation = Mathf.Rad2Deg * theta;
        GameObject hook = Instantiate(hookPrefab, playerPosition + point, Quaternion.Euler(0,0,pointRotation - 90));
        Rigidbody2D hookRigidbody = hook.GetComponentInChildren<Rigidbody2D>();
        hookRigidbody.AddForce(playerInput.aimDirection * shootForce, ForceMode2D.Impulse);
    }

    private IEnumerator HookCooldown()
    {
        while(hookFired)
        {
            yield return new WaitForSeconds(cooldownLength);
            hookFired = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (playerInput != null)
            Gizmos.DrawRay(player.position, playerInput.aimDirection.normalized);
    }
}