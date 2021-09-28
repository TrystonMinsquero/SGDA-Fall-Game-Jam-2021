using UnityEngine;

public class Projectile : MonoBehaviour
{
    Player player;
    Vector3 startPos;
    float speed;
    float range;
    Vector2 direction;
    int damage;
    float timeFromShot;

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
        rb.velocity += player.GetVelocity();
    }

    private void Update()
    {
        //Destroy if too far
        Vector2 distance = startPos - transform.position;
        if (distance.magnitude >= range)
            Delete();

        switch (player.weapon.weapon.weaponType)
        {
            case WeaponType.RPG:
                if (Time.time / timeFromShot % 0.1 == 0)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x);
                    Vector2 dirNew = new Vector2(direction.x - (0.05f) * Mathf.Sin(angle),
                        player.lookDirection.y + (0.05f) * Mathf.Cos(angle));
                    direction = dirNew;
                }
                break;

            default:
                break;
        }
        
        
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
