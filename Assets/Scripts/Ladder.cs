using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    // handle
    [SerializeField] private Transform _topSnapPos, _botSnapPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("ladder triggered");
            // snap to ladder
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Ladder(_botSnapPos, _topSnapPos);
            }
            else { Debug.LogError(gameObject.name + " player is null"); }
           
        }
    }
}