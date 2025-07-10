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
        if (_hero._HouseEnter)//�V���b�v���J������
        {
            HouseHPText.gameObject.SetActive(false);//�Ƃ̑ϋv�l�������Ȃ�����
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
                SceneManager.LoadScene("StartScene");//�V�[���J��
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);//�V�[�����ă��[�h
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
    /// �Ƃ̑ϋv�l�����߂郁�\�b�h
    /// </summary>
    public void SetHouseHP()
    {
        HouseHPText.text = "�Ƃ̑ϋv�l " + $"{HouseHP - _enemyIn}";
    }
}
