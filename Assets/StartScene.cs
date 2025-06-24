using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI GameStartUI;
    [SerializeField] TextMeshProUGUI[] GameModeUI;
    [SerializeField] GameObject StartImage;
    [SerializeField] GameObject SetExplainUI;
    [SerializeField] GameObject ExplainImage;
    public static int Maxwave; 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ExplainImage.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ExplainImage.SetActive(false);
            GameModeUI[0].gameObject.SetActive(false);
            GameModeUI[1].gameObject.SetActive(false);
            GameModeUI[2].gameObject.SetActive(false);
            GameStartUI.gameObject.SetActive(true);
            SetExplainUI.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameStartUI.gameObject.SetActive(false);
            SetExplainUI.gameObject.SetActive(false);
            GameModeUI[0].gameObject.SetActive(true);
            GameModeUI[1].gameObject.SetActive(true);
            GameModeUI[2].gameObject.SetActive(true);
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
