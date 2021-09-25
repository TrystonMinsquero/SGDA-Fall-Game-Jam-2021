using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float deathTime_MAX;
    public float dashDamage;
    public HealthBar healthBar;

    private SpriteRenderer sr;
    private Animator anim;
    private Weapon weapon;

    private Vector3 healthBarPos;
    private float timeRemaing;
    private float deathTime;

    private void Start()
    {
        weapon = null;
        sr = GetComponent<SpriteRenderer>();
        deathTime = Time.time + deathTime_MAX;
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

    public void Shoot(PlayerController pc)
    {
        if(weapon != null)
        {
            weapon.Shoot(pc);
        }
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
    }

    public List<Projectile> GetProjectiles()
    {
        return weapon.projectiles;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
            
    }
}
