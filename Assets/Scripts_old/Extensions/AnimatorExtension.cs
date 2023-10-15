using UnityEngine;

public static class AnimatorExtension
{
    public static void SwitchAnimation(this Animator animator, int hashNextAnimation, ref int hashCurrentAnimation)
    {
        if (hashCurrentAnimation.Equals(hashNextAnimation)) return;     
        animator.Play(hashNextAnimation);
        hashCurrentAnimation = hashNextAnimation;      
    }
}
