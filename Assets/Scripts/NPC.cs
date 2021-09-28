using UnityEngine;

[CreateAssetMenu(fileName = "new NPC")]
public class NPC : ScriptableObject
{
    public Sprite image;
    public AnimationClip idle;
    public AnimationClip dash;
    public AnimationClip walk;
    public AnimationClip left;
    public AnimationClip right;
    public bool hasWeapon;

    public void SwitchAnimations(Animator anim)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        aoc["Idle"] = idle;
        aoc["Walk"] = walk;
        aoc["Dash"] = dash;
        aoc["Left"] = left;
        aoc["Right"] = right;
    }


}
