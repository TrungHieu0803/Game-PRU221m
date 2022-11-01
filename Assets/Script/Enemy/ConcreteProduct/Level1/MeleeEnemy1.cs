using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MeleeEnemy1 : MonoBehaviour, IMeleeEnemy
{

    [SerializeField]
    private Image healthBarSprite;
    [SerializeField]
    private Canvas healthBarCanvas;
    [SerializeField]
    private float damage;
    private NavMeshAgent agent;
    public bool showPath;
    public bool showAhead;
    private Animator animator;
    private Vector3 currentPosition;
    private float maxHealth;
    private float currentHealth;
    private bool isDead;
    private bool isHit;
    private float spellDuration;

    // Start is called before the first frame update
    void Start()
    {
        spellDuration = 0f;
        isHit = false;  
        isDead = false;
        maxHealth = 100;
        currentHealth = 100;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 2;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = PlayerController.Instance.transform.position;
        animator.SetFloat("Walk", agent.nextPosition.x - currentPosition.x);
        animator.SetBool("IsStopped", agent.nextPosition.x == currentPosition.x);    
        if (isHit)
        {
            Hit();
        }
        currentPosition = transform.position;
    }

    public void UpdateHealthBar()
    {
        healthBarSprite.fillAmount = currentHealth / maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && !isDead)
        {
            Destroy(collision.gameObject);
            currentHealth -= collision.gameObject.GetComponent<Bullet>().damage;
            UpdateHealthBar();
            agent.speed = 1;
            if (currentHealth < 0)
            {
                healthBarCanvas.enabled = false;

                isDead = true;
            }
        }
        if (isDead && currentHealth != -1)
        {
            currentHealth = -1;
            agent.speed = 0;
            animator.SetTrigger("Death");
            SoundController.instance.playSound1();
            Destroy(gameObject, 1.1f);
            AmmoSpawner.Instance.SpawnRandomAmmo(currentPosition);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && !isDead)
        {
            agent.speed = 2;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isHit = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isHit = false;
            animator.SetBool("IsHit", false);
            agent.speed = 2;
        }
    }

    public void Hit()
    {
        
        if (spellDuration == 0f)
        {
            PlayerController.Instance.currentHealth -= damage;
        } else if(spellDuration >= 2f)
        {
            PlayerController.Instance.currentHealth -= damage;
            spellDuration = 0f; 
        }
        spellDuration += Time.deltaTime;
        agent.speed = 0.5f;
        animator.SetBool("IsHit", true);
        animator.SetFloat("Horizontal", agent.nextPosition.x - currentPosition.x > 0 ? 1 : -1);
        
    }
}
