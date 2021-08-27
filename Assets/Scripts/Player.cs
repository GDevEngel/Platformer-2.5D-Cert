using System.Collections;
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

    //global var
    private float _hDirection;
    private Vector3 _velocity;
    private bool _facingBack = false;
    private Transform _standPos;

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
        if (_animator.GetBool("Ledge") != true)
        {
            CalculateMovement();
        }
        else
        {   // if animator ledge bool is not true then...
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _animator.SetTrigger("ClimbUp");
            }
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
                transform.Rotate(0, 180f, 0);
                _facingBack = false;
            }
            else if (_hDirection <= -0.5 && _facingBack == false)
            {
                transform.Rotate(0, 180f, 0);
                _facingBack = true;
            }

            //jump
            if (Input.GetKeyDown(KeyCode.Space))
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
}