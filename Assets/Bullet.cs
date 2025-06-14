using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;
    public int bulletAttack;
    // Start is called before the first frame update
    void Start()
    {
      
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
