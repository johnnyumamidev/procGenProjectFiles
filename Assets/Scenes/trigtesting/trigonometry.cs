using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class trigonometry : MonoBehaviour
{
    public GameObject triangleBullet;
    float fireRate;
    public float bulletLifetime = 1f;
    float bulletFiredTime;
    List<GameObject> bullets = new List<GameObject>();
    public float bulletSpeed = 2f;

    public Transform reticle;

    public int bulletSpread = 10;
    // Start is called before the first frame update
    void Start()
    {
        fireRate = bulletLifetime;
        InvokeRepeating("Fire", 0, fireRate);        
    }

    // Update is called once per frame
    void Update()
    {
        fireRate = bulletLifetime;
        bulletFiredTime += Time.deltaTime;
        if(bulletFiredTime >= bulletLifetime)
        {
            foreach(GameObject bullet in bullets) { Destroy(bullet); }
            bulletFiredTime = 0;
        }
    }

    private void Fire()
    {
        float reticleXPos = reticle.position.x;
        float reticleYPos = reticle.position.y;
        float theta = Mathf.Atan2(reticleYPos, reticleXPos);

        float spreadRadians = Mathf.PI * bulletSpread / 90f;

        for(int i = 0; i <= bulletSpread; i++)
        {
            GameObject bullet = Instantiate(triangleBullet, transform.position, Quaternion.identity);
            bullets.Add(bullet);

            float bulletSpreadMultiplier = spreadRadians * i;
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            Vector2 _velocity = new Vector2(Mathf.Cos(theta + bulletSpreadMultiplier), Mathf.Sin(theta + bulletSpreadMultiplier));
            bulletRigidbody.velocity = _velocity.normalized * bulletSpeed;

            float reticlePosInDegrees = Mathf.Rad2Deg * theta;
            bullet.transform.rotation = Quaternion.Euler(0, 0, reticlePosInDegrees - 90);
        }
    }

}
