using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Weapon
{
    public GameObject projectile;
    public float fireRate;
    public float projectileSpeed;
    public float range;
    public int damage;

    [HideInInspector]
    public List<Projectile> projectiles;

    private float nextFireTime;


    public void Shoot(PlayerController pc)
    {
        if (Time.time < nextFireTime)
            return;
        nextFireTime = Time.time + fireRate;
        Projectile pro = GameObject.Instantiate(projectile).GetComponent<Projectile>();
        pro.Set(
            pc.player,
            pc.transform.position,
            projectileSpeed,
            range,
            pc.lookDirection,
            damage
        );
        projectiles.Add(pro);
        
    }

    public void Reset()
    {
        nextFireTime = 0;
    }

}
