using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed;
    public int bulletAttackPower;

    private void Start()
    {
        Destroy(gameObject,2);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(-bulletSpeed * Time.deltaTime, 0,0);
    }
}
