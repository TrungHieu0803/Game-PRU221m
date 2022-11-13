using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine;

public class Melee : MonoBehaviour, IMeleeEnemy
{
    [SerializeField]
    protected Image healthBarSprite;
    [SerializeField]
    protected Canvas healthBarCanvas;
    [SerializeField]
    protected float damage;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Vector3 currentPosition;
    [SerializeField]
    protected float maxHealth;
    internal float currentHealth;
    protected bool isDead;
    protected bool isHit;
    protected float hitDuration;
    protected EnemyType enemyType;

    public void Hit() { }
    public void UpdateHealthBar()
    {
        healthBarSprite.fillAmount = currentHealth / maxHealth;
    }

    public void ReceiveDamage(float damage)
    {
        agent.speed = 1;
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            healthBarCanvas.enabled = false;
            isDead = true;
            GameManager.instance.UpdateKill();
        }
        StartCoroutine(Slow());
    }
    public IEnumerator Slow()
    {
        yield return new WaitForSeconds(2f);
        agent.speed = 2;
    }
}
