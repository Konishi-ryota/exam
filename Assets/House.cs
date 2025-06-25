using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    [SerializeField] Text HouseHPText;
    [SerializeField] Text GameOverWaveText;
    [SerializeField] GameObject GameOverUI;
    public int HouseHP;
    public int _enemyIn;

    private Hero _hero;

    void Start()
    {
        _hero = FindAnyObjectByType<Hero>();
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
        if (_hero._isDead)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Back StartScene");
                SceneManager.LoadScene("StartScene");
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1;
            }
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
        HouseHPText.text = "‰Æ‚Ì‘Ï‹v’l " + $"{HouseHP - _enemyIn}";
    }
}
