using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class BulletScript : NetworkBehaviour
{
    public NetworkObject shooter;
    public NetworkObject netObj;
    NetworkRigidbody networkRigidBody;

    
    
    // public void Shoot(Vector3 force,PlayerRef shooter, string shooter_name)
    // {
    //     netObj = GetComponent<NetworkObject>();
    //     networkRigidBody = GetComponent<NetworkRigidbody>();   
    //     networkRigidBody.Rigidbody.AddForce(force, ForceMode.Impulse);
         
    // }

    

    void OnCollisionEnter(Collision collision)
    {
        Runner.Despawn(netObj);
        if(collision.gameObject.tag == "Player")
        {
            var player_scores = shooter.GetComponent<ScoreField>();
            player_scores.HitEnemy();
            
        }
        //despawn bullet
        // Log.Debug("Bullet collided with " + collision.gameObject.name);
        
        
    
        
    }
}
