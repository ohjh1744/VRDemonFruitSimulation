using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimationController : MonoBehaviour
{

    [SerializeField] InputActionReference _grip;

    [SerializeField] Animator _anim;

    private int _animHashGrip= Animator.StringToHash("IsGrip");


    private void OnEnable()
    {
        _grip.action.performed += OnGrip;
    }

    private void OnDisable()
    {
        _grip.action.performed -= OnGrip;
    }

    private void OnGrip(InputAction.CallbackContext obj)
    {
        if(_anim.GetBool(_animHashGrip) == true)
        {
            _anim.SetBool(_animHashGrip, false);
        }
        else
        {
            _anim.SetBool(_animHashGrip, true);
        }
    }
}
