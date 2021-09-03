﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //handle
    private CharacterController _controller;
    private Animator _animator;

    //config
    private float _speed = 12f;
    private float _gravity = 0.4f;
    private float _jumpHeight = 21f;
    private float _climbSpeed = 3f;
    private float _rollSpeed = 12f;

    //global var
    private float _hDirection;
    private Vector3 _velocity;
    private bool _facingBack = false;
    private Transform _standPos;
    private int _collected = 0;
    private bool _isRolling = false;
    private float _rollDirection; 

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null) { Debug.LogError("Player. char controller is null"); }

        _animator = GetComponentInChildren<Animator>();
        if (_controller == null) { Debug.LogError("Player child animator is null"); }
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator.GetBool("Ledge") == false && _isRolling == false && _animator.GetBool("Ladder") == false)
        {
            CalculateMovement();
        }
        else if (_animator.GetBool("Ladder") == true)
        {
            _velocity = new Vector3(0, Input.GetAxisRaw("Vertical") * _climbSpeed, 0);
            _animator.SetFloat("ClimbDirection", Input.GetAxisRaw("Vertical"));
            _controller.Move(_velocity * Time.deltaTime);
        }
        else if (_animator.GetBool("Ledge") == true && Input.GetAxisRaw("Vertical") == 1)
        {
            _animator.SetTrigger("ClimbUp");
        }
        else if (_isRolling == true)
        {
            Debug.Log("is rolling is true");
            Vector3 rollVelocity = new Vector3(0, 0, _rollDirection * _rollSpeed);
            _controller.Move(rollVelocity * Time.deltaTime);
        }
    }

    private void CalculateMovement()
    {
        if (_controller.isGrounded)
        {
            _velocity.y = 0;
            _animator.SetBool("Jump", false);

            _hDirection = Input.GetAxisRaw("Horizontal");
            _velocity = new Vector3(0, 0, _hDirection) * _speed;

            _animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));

            //Debug.Log(_hDirection);

            if (_hDirection >= 0.5f && _facingBack == true)
            {
                FaceDirectionSwitch();
            }
            else if (_hDirection <= -0.5 && _facingBack == false)
            {
                FaceDirectionSwitch();
            }

            // roll l-shift
            if (Input.GetButtonDown("Fire3") && _animator.GetFloat("Speed") > 0.1f)
            {
                _animator.SetTrigger("Roll");
                _isRolling = true;
                if (_facingBack == true)
                {
                    _rollDirection = -1;
                }
                else
                {
                    _rollDirection = 1;
                }
                // store roll direction
                // move controller according to stored roll direction
                // _isRolling bool
                // on anim state exit we call player script to _isRolling = false                
            }
            //jump
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("jump");
                _velocity.y += _jumpHeight;
                //
                _animator.SetBool("Jump", true);
            }

        }
        // not grounded
        else
        {
            _velocity.y -= _gravity;
        }
        //_velocity.y = _yVelocity;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void FaceDirectionSwitch()
    {
        transform.Rotate(0, 180f, 0);
        _facingBack = !_facingBack;
    }

    public void LedgeGrab(Transform snapPos, Transform standPos)
    {
        _animator.SetBool("Ledge", true);
        _animator.SetBool("Jump", false);
        _animator.SetFloat("Speed", 0f);
        _controller.enabled = false;
        transform.position = snapPos.position;
        _standPos = standPos;
    }

    public void StandUp()
    {
        transform.position = _standPos.position;
        _animator.SetBool("Ledge", false);        
        _controller.enabled = true;
    }

    public void Collected()
    {
        _collected++;
        UIManager.Instance.UICollectableTextUpdate(_collected);
    }
    
    public void Ladder(Transform botSnapPos, Transform topSnapPos, Transform topEndTrigger)
    {
        // get on ladder
        if (_animator.GetBool("Ladder") == false)
        {
            //check which snap pos is closer then snap to it
            if (Vector3.Distance(transform.position, botSnapPos.position) < Vector3.Distance(transform.position, topSnapPos.position) 
                && Input.GetAxisRaw("Vertical") == 1)
            {
                //disable controller to be able to snap to a position
                _controller.enabled = false;
                transform.position = botSnapPos.position;
                _animator.SetBool("Ladder", true);
                _controller.enabled = true;
            }
            else if (Vector3.Distance(transform.position, botSnapPos.position) > Vector3.Distance(transform.position, topSnapPos.position)
                && Input.GetAxisRaw("Vertical") == -1)
            {
                //disable controller to be able to snap to a position
                _controller.enabled = false;
                transform.position = topEndTrigger.position;
                FaceDirectionSwitch();
                _animator.SetBool("Ladder", true);
                _controller.enabled = true;
            }
        }
        // get off ladder
        if (_animator.GetBool("Ladder") == true)
        {
            if (Vector3.Distance(transform.position, botSnapPos.position) < 1f
                && Input.GetAxisRaw("Vertical") == -1)
            {
                //disable controller to be able to snap to a position
                _controller.enabled = false;
                transform.position = botSnapPos.position;
                _animator.SetBool("Ladder", false);
                _controller.enabled = true;
            }
            else if (Vector3.Distance(transform.position, topEndTrigger.position) < 1f
                && Input.GetAxisRaw("Vertical") == 1)
            {
                //disable controller to be able to snap to a position
                _controller.enabled = false;
                transform.position = topSnapPos.position;
                _animator.SetBool("Ladder", false);
                _controller.enabled = true;
            }
        }
    }

    public void EndRoll() //used by animation state script "Roll.cs"
    {
        _isRolling = false;
    }
}