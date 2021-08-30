using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                //public method collected
                player.Collected();
                Destroy(this.gameObject);
            }
            else if (player == null) { Debug.LogError(this.gameObject.name + ".player is null"); }            
        }
    }
}
