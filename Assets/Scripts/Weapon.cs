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
        pro.Set(
            player,
            weaponType,
            player.transform.position,
            projectileSpeed,
            range,
            player.lookDirection.normalized,
            damage
        );
        projectiles.Add(pro);
        
    }

    public void Reset()
    {
        nextFireTime = 0;
    }

}

public enum WeaponType
{
    STRAIGHT,
    DUAL
}
