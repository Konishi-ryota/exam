using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] GameObject GameStartUI;
    [SerializeField] GameObject[] GameModeUI;
    [SerializeField] GameObject StartImage;
    [SerializeField] GameObject SetExplainUI;
    [SerializeField] GameObject ExplainImage;
    public static int Maxwave;
    //static:別シーンのスクリプトから変数を持ってきたいときに使う

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ExplainImage.SetActive(true);
            GameStartUI.SetActive(false);
            SetExplainUI.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ExplainImage.SetActive(false);
            GameModeUI[0].SetActive(false);
            GameModeUI[1].SetActive(false);
            GameModeUI[2].SetActive(false);
            GameStartUI.SetActive(true);
            SetExplainUI.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameStartUI.SetActive(false);
            SetExplainUI.SetActive(false);
            GameModeUI[0].SetActive(true);
            GameModeUI[1].SetActive(true);
            GameModeUI[2].SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Maxwave = 3;
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Maxwave = 5;
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Maxwave = 7;
            SceneManager.LoadScene("SampleScene");
        }
    }
}
