using UnityEngine;

public static class AnimatorExtension
{
    public static void SwitchAnimation(this Animator animator, int hashNextAnimation, ref int currentHashAnimation)
    {
        if (currentHashAnimation != hashNextAnimation)
        {
            animator.Play(hashNextAnimation);
        }
        
        currentHashAnimation = hashNextAnimation;      
    }
}
