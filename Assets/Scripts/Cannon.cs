using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // Start is called before the first frame update
    public float frequency = 2f;
    public float bulletTravelDistance = 10f;
    public float force = 5f;
    public GameObject BulletPrefab;
    public GameObject ShotTarg;
    public GameObject Wheel;
    private Transform loc;
    void Start()
    {
        loc = GetComponent<Transform>();
        InvokeRepeating("Fire", frequency, frequency);
    }

    void Fire()
    {
        // Make the bullet
        var angle = loc.eulerAngles.z + 90;
        var bulletObj = Instantiate(BulletPrefab, ShotTarg.transform.position, Quaternion.Euler(new Vector3(0,0,angle)));
        
        // Don't let it touch me
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bulletObj.GetComponent<Collider2D>());
        
        // Don't let it go too far
        var bulletComponent = bulletObj.GetComponent<CannonBullet>();
        bulletComponent.distance = bulletTravelDistance;
        
        // Yeet that fucker
        bulletObj.GetComponent<Rigidbody2D>()
            .AddForce(bulletObj.transform.TransformDirection(new Vector2(0, 1)).normalized * force,
                ForceMode2D.Impulse);
    }
}
