using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    public float speed;
    public float damage;
    public Player player;

    void Start()
    {
        
    }

    
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroythis();
            

        }
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().collided = true;
            Destroythis();
        }
    }

    

    void Destroythis()
    {
        NetworkServer.Destroy(gameObject);
    }

   
    
}
