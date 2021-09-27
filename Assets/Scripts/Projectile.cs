using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Player player;
    Vector3 startPos;
    float speed;
    float range;
    Vector2 direction;
    int damage;

    Rigidbody2D rb;

    public void Set(Player player, Vector3 startPos, float speed, float range, Vector2 direction, int damage = 0)
    {
        this.player = player;
        this.startPos = startPos;
        this.speed = speed;
        this.range = range;
        this.direction = direction;
        this.damage = damage;

        rb = GetComponent<Rigidbody2D>();
        transform.position = startPos;

        rb.velocity = direction * speed;

        //switch (weaponType)
        //{
        //    case WeaponType.STRAIGHT:
                
        //        break;

        //    case WeaponType.DUAL:
        //        rb.velocity = direction * speed;
        //        break;


        //}
    }

    private void Update()
    {
        //Destroy if too far
        Vector2 distance = startPos - transform.position;
        if (distance.magnitude >= range)
            Delete();

        rb.velocity = direction * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetComponent<Player>() != player)
                collision.GetComponent<Player>().TakeDamage(damage);
        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            Destroy(collision.gameObject);
            Delete();
        }
        else
            Delete();
    }

    public void Delete()
    {
        player.weapon.GetProjectiles().Remove(this);
        Destroy(this.gameObject);
    }

}
