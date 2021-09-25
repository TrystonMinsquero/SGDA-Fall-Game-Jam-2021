
using UnityEngine;

public class Player : MonoBehaviour
{
    public float deathTime_MAX;
    public float dashDamage;
    public SpriteRenderer sr;
    public Weapon weapon;
    public HealthBar healthBar;
    private float timeRemaing;
    private float deathTime;

    private void Start()
    {
        weapon = null;
        sr = GetComponent<SpriteRenderer>();
        deathTime = Time.time + deathTime_MAX;
    }

    private void Update()
    {
        if(Time.time > deathTime)
            Die();
        timeRemaing = deathTime - Time.time;

        healthBar.SetHealth(timeRemaing / deathTime_MAX);
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
        Debug.Log("Take Over" + npc.name);
        //Change anims & sprite
        sr.sprite = npc.image;
        weapon = npc.weapon;
        weapon.Reset();
        Destroy(npc_c.gameObject);

    }

    public void TakeOver(Player player)
    {
        Debug.Log("Take Over" + player.name);
        //Damage other player
        //if damage was enough to kill, take over
        weapon = player.weapon;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
            
    }
}
