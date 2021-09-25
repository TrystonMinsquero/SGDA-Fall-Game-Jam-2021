using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Player player;
    WeaponType weaponType;
    Vector3 startPos;
    float speed;
    float range;
    Vector2 direction;
    int damage;

    Rigidbody2D rb;

    public void Set(Player player, WeaponType weaponType, Vector3 startPos, float speed, float range, Vector2 direction, int damage = 0)
    {
        this.player = player;
        this.weaponType = weaponType;
        this.startPos = startPos;
        this.speed = speed;
        this.range = range;
        this.direction = direction;
        this.damage = damage;

        rb = GetComponent<Rigidbody2D>();
        transform.position = startPos;
        switch (weaponType)
        {
            case WeaponType.STRAIGHT:
                rb.velocity = direction * speed;
                break;
        }
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
            if(collision.GetComponent<Player>() != player)
                collision.GetComponent<Player>().TakeDamage(damage);
        }
        else
        {
            Destroy(collision.gameObject);
            Delete();
        }
    }

    public void Delete()
    {
        player.GetProjectiles().Remove(this);
        Destroy(this.gameObject);
    }

}
