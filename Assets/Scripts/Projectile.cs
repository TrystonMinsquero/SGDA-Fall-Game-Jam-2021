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

    public GameObject explosion;

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
        
        
        rb.velocity = direction * speed;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (player.weapon.weapon.weaponType)
        {
            case WeaponType.STRAIGHT:
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
                break;




            case WeaponType.DUAL:
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
                    this.damage /= 2;   // wallbang -> damage only half
                break;



                // Sniper
            case WeaponType.LONG:
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
                    this.damage /= 2;
                break;



                // RPG
            case WeaponType.RPG:
                Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 3f);

                for (int i = 0; i < collisions.Length; i++)
                {
                    if (collisions[i].gameObject.CompareTag("Projectile"))
                        return;
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        if (collisions[i].GetComponent<Player>() != player)
                            collisions[i].GetComponent<Player>().TakeDamage(damage);
                        Delete();
                    }
                    else if (collisions[i].gameObject.CompareTag("NPC"))
                    {
                        Destroy(collisions[i].gameObject);
                        GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                        Destroy(expl, 3f);
                        Delete();
                    }
                    else
                    {
                        GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                        Destroy(expl, 3f);
                        Delete();
                    }
                }
                break;



        }


       
    }

    public void Delete()
    {
        player.weapon.GetProjectiles().Remove(this);
        Destroy(this.gameObject);
    }

}
