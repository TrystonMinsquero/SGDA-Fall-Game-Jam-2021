using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "new Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Visual")]
    public Color color = Color.white;
    public Sprite sprite;
    public AnimationClip idle;
    public AnimationClip dash;
    public AnimationClip walk;
    public AnimationClip left;
    public AnimationClip right;

    [Header("Muzzle Flash")]
    public Sprite flashSprite;
    public AnimationClip flash;

    [Header("Projectile Details")]
    public GameObject projectilePrefab;
    public WeaponType weaponType;

    [Header("Stats")]
    public float fireDelay;
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
        nextFireTime = Time.time + fireDelay;
        Projectile pro = GameObject.Instantiate(projectilePrefab).GetComponent<Projectile>();


        switch (weaponType)
        {
            case WeaponType.STRAIGHT:
                pro.Set(player, player.transform.position, projectileSpeed, range, player.lookDirection.normalized, damage);
                projectiles.Add(pro);
                break;


            case WeaponType.DUAL:

                float angle = Mathf.Atan2(player.lookDirection.y, player.lookDirection.x);
                    // angle from player to x-axis

                Vector2 shotgun = new Vector2(player.lookDirection.x - (0.05f) * Mathf.Sin(angle), 
                    player.lookDirection.y + (0.05f) * Mathf.Cos(angle));
                Vector2 shotgun2 = new Vector2(player.lookDirection.x + (0.05f) * Mathf.Sin(angle), 
                    player.lookDirection.y - (0.05f) * Mathf.Cos(angle));
                // vectors for direction of each shotgun bullet/projectile

                Projectile pro2 = GameObject.Instantiate(projectilePrefab).GetComponent<Projectile>();
                    // second bullet created

                pro.Set(player, player.transform.position, projectileSpeed, range, shotgun, damage);
                pro2.Set(player, player.transform.position, projectileSpeed, range, shotgun2, damage);
                

                projectiles.Add(pro);
                projectiles.Add(pro2);

                break;

            case WeaponType.RPG:
                pro.Set(player, player.transform.position, projectileSpeed, range, player.lookDirection.normalized, damage);
                projectiles.Add(pro);

                break;

            case WeaponType.GRENADE:

                break;
        }


        
        
    }

    public void Reset()
    {
        nextFireTime = 0;
    }

    public void SwitchAnimations(Animator gunAnim, Animator flashAnim)
    {
        AnimatorOverrideController gunAoc = new AnimatorOverrideController(gunAnim.runtimeAnimatorController);
        gunAoc["Idle"] = idle;
        gunAoc["Walk"] = walk;
        gunAoc["Dash"] = dash;
        gunAoc["Left"] = left;
        gunAoc["Right"] = right;

        AnimatorOverrideController flashAoc = new AnimatorOverrideController(flashAnim.runtimeAnimatorController);
        flashAoc["Shoot"] = flash;
    }

}

public enum WeaponType
{
    STRAIGHT,
    DUAL,
    RPG,
    GRENADE
}