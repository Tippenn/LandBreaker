using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public MuzzleFlash flash;

    [Header("Melee Attack")]
    public float meleeAttackRange = 2f;        // The maximum range of the melee attack
    public float meleeAttackRadius = 1.5f;     // The radius of the sphere for a wider attack
    public int meleeAttackDamage = 20;         // Damage dealt by the melee attack
    public float meleeAttackCooldown = 1f;     // Time between attacks
    public bool readyToMeleeAttack = true;
    public Transform attackPoint;         // Point in front of the player where the attack is centered
    public LayerMask enemyLayers;         // Layer mask to detect enemies

    [Header("Range Attack")]
    public int rangeAttackDamage = 10;         // Damage dealt by the ranged attack
    public float rangeAttackRange = 50f;       // Maximum range of the attack
    public float rangeAttackCooldown = 0.6f;     // Time between attacks
    public bool readyToRangeAttack = true;
    public Camera playerCamera;           // The player's camera to get the aiming direction

    private float nextAttackTime = 0f;

    private void Awake()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && readyToRangeAttack)
        {
            readyToRangeAttack = false;
            Debug.Log("nembak");
            RangeAttack();
            
            Invoke(nameof(ResetRangeAttack), rangeAttackCooldown);
        }
        // Trigger attack on left mouse click (or customize the key)
        if (Input.GetButton("Fire2") && readyToMeleeAttack)
        {
            Debug.Log("attacking");
            readyToMeleeAttack = false;
            MeleeAttack();
            Invoke(nameof(ResetMeleeAttack), meleeAttackCooldown);
        }
    }

    void MeleeAttack()
    {
        // Play attack animation if necessary
        // animator.SetTrigger("Attack");

        // Create a sphere in front of the player to detect enemies in the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, meleeAttackRadius, enemyLayers);

        // Apply damage to all enemies hit by the sphere
        foreach (Collider enemy in hitEnemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(rangeAttackDamage);
            }
        }

        // (Optional) Visual feedback like attack sound or particle effects
    }

    void RangeAttack()
    {
        flash.Shoot();

        // Perform a raycast from the camera's position (center screen) in the direction the player is aiming
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rangeAttackRange, enemyLayers))
        {
            // If the raycast hits something on the enemy layer, apply damage
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(rangeAttackDamage);
            }

            // Optional: Add hit effects like particles or sound
            Debug.Log("Hit: " + hit.collider.name);
        }
    }

    public void ResetRangeAttack()
    {
        readyToRangeAttack = true;
    }

    public void ResetMeleeAttack()
    {
        readyToMeleeAttack = true;
    }

    // Visualize the attack range in the scene view for debugging purposes
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, meleeAttackRadius);
    }
}
