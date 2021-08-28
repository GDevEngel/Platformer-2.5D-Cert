using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //handle
    [SerializeField] private Transform _startPos, _endPos;

    //config
    private float _speed = 3f;

    //vars
    private bool _switchTarget;
    private Transform _target;

    private void Start()
    {
        _target = _startPos;
    }

    void FixedUpdate()
    {
        //if distance to target is less then 0.1f
        //then switch target
                
        //Debug.Log("distance"+Vector3.Distance(transform.position, _target.position));
        if (Vector3.Distance(transform.position, _target.position) < 0.1f)
        {
            _switchTarget = !_switchTarget;

            //if switch is on
            if (_switchTarget == true)
            {
                _target = _endPos;
            }
            //set target to end pos
            //else 
            //set target to start pos
            else
            {
                _target = _startPos;
            }
        }        

        //move towards target
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }

}
