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
        if (_animator.GetBool("Ledge") == false && _isRolling == false)
        {
            CalculateMovement();
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
                transform.Rotate(0, 180f, 0);
                _facingBack = false;
            }
            else if (_hDirection <= -0.5 && _facingBack == false)
            {
                transform.Rotate(0, 180f, 0);
                _facingBack = true;
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

    public void Ladder(Transform botSnapPos, Transform TopSnapPos)
    {
        StartCoroutine(LadderClimb(botSnapPos, TopSnapPos));
    }

    IEnumerator LadderClimb(Transform startPos, Transform endPos)
    {
        _controller.enabled = false;
        transform.position = startPos.position;
        _animator.SetBool("Ladder", true);
        yield return new WaitForSeconds(3f); //insert distance start n end pos divided by speed?
        _controller.enabled = true;
        _animator.SetBool("Ladder", true);
    }

    public void EndRoll() //used by animation state script "Roll.cs"
    {
        _isRolling = false;
    }
}