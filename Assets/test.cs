using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Image healthBarSprite;
    private NavMeshAgent agent;
    public bool showPath;
    public bool showAhead;
    [SerializeField]
    private Animator animator;
    private Vector3 currentPosition;
    private float maxHealth;
    private float currentHealth;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        maxHealth = 100;
        currentHealth = 100;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 4;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = PlayerController.Instance.transform.position;
        animator.SetFloat("Walk", agent.nextPosition.x - currentPosition.x);
        animator.SetBool("IsStopped", agent.nextPosition.x == currentPosition.x);
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
                isDead = true;
            }
        }
        if (isDead && currentHealth != -1)
        {
            currentHealth = -1;
            agent.speed = 0;
            animator.SetTrigger("Death");
            Destroy(gameObject, 1.1f);
        }
    }
}
