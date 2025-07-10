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
        if (_hero._HouseEnter)//ショップを開いたら
        {
            HouseHPText.gameObject.SetActive(false);//家の耐久値を見えなくする
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
                SceneManager.LoadScene("StartScene");//シーン遷移
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);//シーンを再ロード
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _enemyIn++;
            SetHouseHP();
        }
    }
    /// <summary>
    /// 家の耐久値を決めるメソッド
    /// </summary>
    public void SetHouseHP()
    {
        HouseHPText.text = "家の耐久値 " + $"{HouseHP - _enemyIn}";
    }
}
