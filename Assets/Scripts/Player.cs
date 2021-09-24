
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer sr;
    public Weapon weapon;
    public bool dashing;

    private void Start()
    {
        weapon = null;
        sr = GetComponent<SpriteRenderer>();
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

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Die");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (dashing)
        {
            if(collision.collider.CompareTag("NPC"))
                TakeOver(collision.gameObject.GetComponent<NPC_Controller>());
            if (collision.collider.CompareTag("Player"))
                TakeOver(collision.gameObject.GetComponent<Player>());
        }
            
    }
}
