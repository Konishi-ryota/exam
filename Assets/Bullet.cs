using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed;
    public int PistlebulletAttack;
    public int ARbulletAttack;
    public int SMGbulletAttack;
    public int SRbulletAttack;
    public float interval = 1f;

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
    public void bulletSettings()
    {

    }
}
