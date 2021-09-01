using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    // handle
    [SerializeField] private Transform _topFloor, _bottomFloor, _startPos;
    
    // config
    private float _speed = 3f;

    // global vars
    private Transform _target;
    private bool _switch;

    // Start is called before the first frame update
    void Start()
    {
        _target = _startPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // child player
            other.transform.SetParent(this.transform);
            Debug.Log(transform);

            if (Vector3.Distance(transform.position, _target.position) < 0.1f)
            {
                SwitchTarget();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        // unchild player
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
    

    private void SwitchTarget()
    {
        _switch = !_switch;
        if (_switch)
        {
            _target = _bottomFloor;
        }
        else if (_switch == false)
        {
            _target = _topFloor;
        }
    }
}
