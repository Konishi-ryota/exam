using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    [SerializeField] Text HouseHPText;
    [SerializeField] Text GameOverWaveText;
    [SerializeField] GameObject GameOverUI;
    public int HouseHP;
    public int _enemyIn;

    private Hero _hero;
    private EnemySpawnpoint _enemySpawnpoint;

    void Start()
    {
        _hero = FindAnyObjectByType<Hero>();
        _enemySpawnpoint = FindAnyObjectByType<EnemySpawnpoint>();
        SetHouseHP();
    }
    void Update()
    {
        if (_hero._HouseEnter)
        {
            HouseHPText.gameObject.SetActive(false);
        }
        else
        {
            HouseHPText.gameObject.SetActive(true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _enemyIn++;
            SetHouseHP();
        }
    }
    public void SetHouseHP()
    {
        HouseHPText.text = "Žc‚è‚Ì‘Ï‹v’l " + $"{HouseHP - _enemyIn}";
    }
}
