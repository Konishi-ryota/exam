using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed;
    public int PistlebulletAttack;
    public int ARbulletAttack;
    public int SMGbulletAttack;
    public int SRbulletAttack;
    public float interval = 1f;

    [SerializeField] GameObject PistleBullet;
    [SerializeField] GameObject ARBullet;
    [SerializeField] GameObject SMGBullet;
    [SerializeField] GameObject SRBullet;

    private Hero _hero;
    private int _index;

    // Start is called before the first frame update
    void Start()
    {
        _hero = FindObjectOfType<Hero>();
        _index = _hero.playerIndex;
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
    public void bulletshot()
    {
        if (_index == 0)
        {
            Instantiate(PistleBullet);
        }
        if (_index == 1)
        {
            Instantiate(ARBullet);
        }
        if (_index == 2)
        {
            Instantiate(SMGBullet);
        }
        if (_index == 3)
        {
            Instantiate(SRBullet);
        }
    }
}
