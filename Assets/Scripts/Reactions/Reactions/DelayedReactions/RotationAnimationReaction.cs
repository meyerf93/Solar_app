using UnityEngine;


public class RotationAnimationReaction : DelayedReaction
{
    public Animator animator;
    public string trigger_direction_rotation;

    public SimpleRotation rotation;

    private Vector2 direction;

    // Use this for initialization
    protected override void SpecificInit()
    {

    }

    protected override void ImmediateReaction()
    {
        direction = rotation.GetDirection();
        //Debug.Log("new flaot for the rotation animation : " + direction.x);
        animator.SetFloat(trigger_direction_rotation,direction.x);
    }
}
