using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isPause = false;

    private static GameManager m_instance;
    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();

            return m_instance;
        }
    }

    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private Slider bgmVolume;
    [SerializeField]
    private Slider fxSoundVolume;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private GameObject hitEffect;
    private int score = 0;
    private AudioSource bgm;
    private AudioSource fxSound;

    void Start()
    {
        pausePanel.SetActive(false);
        hitEffect.SetActive(false);
        bgm = GetComponent<AudioSource>();
        fxSound = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
                Time.timeScale = 1f;
            else
                Time.timeScale = 0f;
            isPause = !isPause;
            pausePanel.SetActive(isPause);
        }
        // test code
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(HitEffect());
        }
    }

    private IEnumerator HitEffect()
    {
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitEffect.SetActive(false);
    }

    public void SetMusicVolume()
    {
        bgm.volume = bgmVolume.value;
    }

    public void SetFxVolume()
    {
        fxSound.volume = fxSoundVolume.value;
    }

    public void ResumeButton()
    {
        isPause = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void UpdateScoreText(int newScore)
    {
        score += newScore;
        scoreText.text = "SCORE : " + score;
    }

    //private void GameRestart()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}
}