using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed;
    public int bulletAttackPower;
    public float interval = 1f;

    private Hero _hero;

    // Start is called before the first frame update
    void Start()
    {
        _hero = FindAnyObjectByType<Hero>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(bulletSpeed * Time.deltaTime, 0);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
