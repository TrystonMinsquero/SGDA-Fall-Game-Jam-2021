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

        //if (player.weapon.weapon.weaponType == WeaponType.GRENADE)
        //    GetComponent<Collider2D>().isTrigger = false;
    }

    private void Update()
    {

        //Destroy if too far
        Vector2 distance = startPos - transform.position;
        if (distance.magnitude >= range)
            Delete();


        rb.velocity = direction * speed;
    }

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    direction *= -1;
    //}



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
                }
                else
                    this.damage /= 2;
                break;

            case WeaponType.RPG:
                AreaDamageEnemies(transform.position, 1.5f, damage);
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
                {
                    Delete();
                }
                    
                break;

            // RPG
            //case WeaponType.RPG:
            //    if (collision.gameObject.CompareTag("Projectile"))
            //        return;
            //    if (collision.gameObject.CompareTag("Player"))
            //    {
            //        if (collision.GetComponent<Player>() != player)
            //            collision.GetComponent<Player>().TakeDamage(damage);

            //        AreaDamageEnemies(transform.position, 1.5f, damage);
            //    }
            //    else if (collision.gameObject.CompareTag("NPC"))
            //    {
            //        AreaDamageEnemies(transform.position, 1.5f, damage);

            //        Destroy(collision.gameObject);
            //        Delete();

            //        GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            //        Destroy(expl, 3f);

            //    }
            //    else
            //    {
            //        AreaDamageEnemies(transform.position, 1.5f, damage);
            //        GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            //        Destroy(expl, 3f);
            //        Delete();         
            //    }

            //    break;

            case WeaponType.GRENADE:
                direction *= -1;
                break;

            default:
                break;



        }
    }

    public void Delete()
    {
        player.weapon.GetProjectiles().Remove(this);
        Destroy(this.gameObject);
    }

    public void AreaDamageEnemies(Vector3 location, float radius, float damage)
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(location, radius);

        foreach (Collider2D collision in objectsInRange)
        {

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
            {
                Delete();
            }
        }
    }

}
