using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement_Kdx movement;
    [SerializeField] private Animator animator;

    [SerializeField] private int _isWalkingKey;
    [SerializeField] private int _isJumpingKey;
    [SerializeField] private int _velocityYKey;

    
    void Start()
    {
        movement = GetComponent<PlayerMovement_Kdx>();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        _isWalkingKey = Animator.StringToHash("walkingSpeed");
        _isJumpingKey = Animator.StringToHash("jump");
        _velocityYKey = Animator.StringToHash("velocityY"); 
    }

    
    void Update()
    {
        animator.SetBool(_isWalkingKey, Mathf.Abs(movement.horizontal) != 0);
        animator.SetFloat(_velocityYKey, movement.rb.velocity.y);
        animator.SetBool(_isJumpingKey, !movement.IsOnGround);
    }
    
}
