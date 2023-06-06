using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class BulletShoot : NetworkBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) // Left mouse button
        {
            shoot();
        }
    }

    private void shoot()
    {
        NetworkObject bullet = Runner.Spawn(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
