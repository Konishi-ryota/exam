using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rig = null;
    public Transform spawnpoint = null;
    [SerializeField] private GameObject enemy;
    [Header("EnemySettings")]
    [SerializeField] private int E_speed;
    [SerializeField] private int E_attack;
    [SerializeField] private int E_hp =10;
    public int E_exp;
    public int E_Gold;

    private Hero _hero;
    private Bullet _bullet;
    private EnemySpawnpoint _spawn;

    // Start is called before the first frame update
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 60;

        _hero = FindAnyObjectByType<Hero>();
        _spawn = FindAnyObjectByType<EnemySpawnpoint>();
        if (!_hero)
        {
            Debug.LogWarning("hero is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        _rig.velocity = new Vector3(E_speed, 0);
        if (_hero._remainTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            _bullet = FindAnyObjectByType<Bullet>();
            E_hp = E_hp - _bullet.bulletAttackPower;
            Destroy(collision.gameObject); //íeä€è¡Ç∑

            if (E_hp <= 0)
            {
                Debug.Log($"death {gameObject.name}");

                if (_hero)
                {
                    _hero.AddGold(E_Gold); //ÉSÅ[ÉãÉhÇëùÇ‚Ç∑
                }

                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log($"remain {gameObject.name} health : {E_hp}");
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            _rig.constraints = RigidbodyConstraints2D.FreezePositionX;
            _hero.H_hp = _hero.H_hp - E_attack;
            Debug.Log($"remain {_hero.H_hp} player health");
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Respawn")
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _rig.constraints = RigidbodyConstraints2D.None;
    }
}
