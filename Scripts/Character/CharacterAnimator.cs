using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterMovement _characterMovement;


    public bool isJumping = false;
    public bool isFalling = false;
    public bool isRuning = false;
    public bool isSprinting = false;

    #region Animation State
    private const string JUMP = "isJump";
    private const string FALL = "isFall";
    private const string RUN = "isRun";
    private const string SPRINT = "isSprint";
    #endregion

    private void Update()
    {
        ChangeAnimationState(isJumping, JUMP);
        ChangeAnimationState(isFalling, FALL);
        ChangeAnimationState(isRuning, RUN);
        ChangeAnimationState(isSprinting, SPRINT);
    }

    private void ChangeAnimationState(bool state, string animation)
    {
        if (state)
            _animator.SetBool(animation, true);
        else
            _animator.SetBool(animation, false);
    }

}
