using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed;
    public int bulletAttackPower;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(-bulletSpeed * Time.deltaTime, 0,0);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
