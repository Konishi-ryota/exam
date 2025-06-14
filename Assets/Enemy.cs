using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rig = null;
    public Transform spawnpoint = null;
    [SerializeField] private GameObject[] E_num;
    [Header("EnemySettings")]
    [SerializeField] private int E_speed;
    [SerializeField] private int E_attack;
    [SerializeField] private int E_hp;
    public int E_exp;
    public int E_Gold;
    private Hero _hero;

    // Start is called before the first frame update
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 60;
        Bullet bullet =FindObjectOfType<Bullet>();
        //E_damage = bullet.bulletAttack;

        _hero = FindAnyObjectByType<Hero>();
        if (!_hero)
        {
            Debug.LogWarning("hero is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        _rig.velocity = new Vector3(E_speed, 0);
        //int rnd = 0;
        //if(Time.frameCount%60 == 0 && CompareTag("king"))
        //{
        //    rnd = Random.Range(0, 21);
        //    Instantiate(E_num[rnd], spawnpoint);
        //}
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            _rig.constraints = RigidbodyConstraints2D.FreezePositionX;
            E_hp = E_hp - _hero.AttackPower;
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
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _rig.constraints = RigidbodyConstraints2D.None;
    }
}
