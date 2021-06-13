using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // Start is called before the first frame update
    public float frequency = 2f;
    public float bulletTravelDistance = 10f;
    public float force = 1f;
    public GameObject BulletPrefab;
    private Transform loc;
    void Start()
    {
        loc = GetComponent<Transform>();
        InvokeRepeating("Fire", frequency, frequency);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Fire()
    {
        var bulletObj = Instantiate(BulletPrefab, loc.position, Quaternion.Euler(new Vector3(0,0,90)));
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bulletObj.GetComponent<Collider2D>());
        var bulletComponent = bulletObj.GetComponent<CannonBullet>();
        bulletComponent.distance = bulletTravelDistance;
        bulletObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(-force, 0));
    }
}
