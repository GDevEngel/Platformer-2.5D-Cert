using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    // handle
    [SerializeField] private GameObject _stageClearUI;
    [SerializeField] private GameObject _finishCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Finish();
                _stageClearUI.SetActive(true);
                _finishCam.SetActive(true);
            }            
        }
    }
}
