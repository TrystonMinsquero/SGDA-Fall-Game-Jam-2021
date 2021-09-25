using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float deathTime_MAX;
    public float dashDamage;
    public float movementSpeed;
    public float dashDelay;
    public float dashForce;
    public HealthBar healthBar;

    private SpriteRenderer sr;
    private Animator anim;
    [HideInInspector]
    public Weapon weapon;

    [HideInInspector]
    public Vector2 lookDirection;
    private Vector3 healthBarPos;
    [HideInInspector]
    public float timeRemaing;
    private float deathTime;
    [HideInInspector]
    public float nextDashTime;

    private void Start()
    {
        weapon = null;
        sr = GetComponent<SpriteRenderer>();
        deathTime = Time.time + deathTime_MAX;
        nextDashTime = 0;
        healthBarPos = healthBar.transform.position - transform.position;
    }

    private void Update()
    {
        if(Time.time > deathTime)
            Die();
        timeRemaing = deathTime - Time.time;

        healthBar.SetHealth(timeRemaing / deathTime_MAX);
        healthBar.transform.position = transform.position + healthBarPos;
    }

    public void Shoot()
    {
        if(weapon != null)
        {
            weapon.Shoot(this);
        }
    }

    public void Dash()
    {
        if (Time.time < nextDashTime)
            return;
        Debug.Log("Dash");
        //nextDashTime = Time.time + dashDelay;
        nextDashTime = Time.time + 0.1f;
        transform.Translate(lookDirection * dashForce, Space.World);
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2);
        foreach (Collider2D collider in collidersHit)
        {
            if (collider.CompareTag("Player"))
            {
                TakeOver(collider.GetComponent<Player>());
                break;
            }
            else if (collider.CompareTag("NPC"))
            {
                TakeOver(collider.GetComponent<NPC_Controller>());
                break;
            }
        }
        //rb.AddForce(lookDirection * dashForce);

    }


    public void TakeOver(NPC_Controller npc_c)
    {
        NPC npc = npc_c.npc;
        Debug.Log("Take Over " + npc.name);
        sr.sprite = npc.image;
        anim = npc.anim;
        weapon = npc.weapon;
        if(weapon != null)
            weapon.Reset();
        deathTime = Time.time + deathTime_MAX;
        Destroy(npc_c.gameObject);

    }

    public void TakeOver(Player player)
    {
        Debug.Log("Take Over " + player.name);
        if (Time.time < player.deathTime - dashDamage)
        {
            player.TakeDamage(dashDamage);
            return;
        }
        weapon = player.weapon;
        if (weapon != null)
            weapon.Reset();
        sr.sprite = player.sr.sprite;
        anim = player.anim;
        deathTime = Time.time + deathTime_MAX;
        player.Die();

    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0)
            Die();
        else
            deathTime -= damage;
    }

    private void Die()
    {
        Debug.Log("Die");
        Destroy(this.gameObject);
    }

    public List<Projectile> GetProjectiles()
    {
        return weapon.projectiles;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
            
    }
}
