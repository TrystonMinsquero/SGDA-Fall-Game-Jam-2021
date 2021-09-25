using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Weapon
{
    public GameObject projectile;
    public WeaponType weaponType;
    public float fireRate;
    public float projectileSpeed;
    public float range;
    public int damage;

    [HideInInspector]
    public List<Projectile> projectiles;

    private float nextFireTime;


    public void Shoot(Player player)
    {
        if (Time.time < nextFireTime)
            return;
        nextFireTime = Time.time + fireRate;
        Projectile pro = GameObject.Instantiate(projectile).GetComponent<Projectile>();

        switch (weaponType)
        {
            case WeaponType.STRAIGHT:
                pro.Set(player, player.transform.position, projectileSpeed, range, player.lookDirection.normalized, damage);
                break;

            case WeaponType.DUAL:
                Vector2 shotgun = new Vector2(player.lookDirection.normalized.x + 1f, player.lookDirection.normalized.y + 1f);
                Vector2 shotgun2 = new Vector2(player.lookDirection.normalized.x + 1f, player.lookDirection.normalized.y - 1f);

                Projectile pro2 = GameObject.Instantiate(projectile).GetComponent<Projectile>();

                pro.Set(player, player.transform.position, projectileSpeed, range, shotgun, damage);
                pro2.Set(player, player.transform.position, projectileSpeed, range, shotgun2, damage);
                //pro.Set(player, player.transform.position, projectileSpeed, range, player.lookDirection.normalized, damage);

                projectiles.Add(pro);
                projectiles.Add(pro2);

                break;
        }


        
        
    }

    public void Reset()
    {
        nextFireTime = 0;
    }

}

public enum WeaponType
{
    STRAIGHT,
    DUAL,
    RPG,
    GRENADE
}