using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public SettingsMenu settingsMenuComponent;
    public ParticleSystem highscoreParticle;
    public ParticleSystem highscoreParticle2;
    public ParticleSystem highscoreParticle3;
    public Ability ability;
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI scoreText2;
    public TextMeshProUGUI highscoreText2;
    public TextMeshProUGUI livesText;
    public GameObject borders;
    public GameObject settingsMenu;
    public GameObject gameOverMenu;
    public GameObject mainMenu;
    public AudioSource highscoreAudio;
    public AudioSource gameOverAudio;
    public AudioSource celebratoryAudio;
    public AudioMixer audioMixer;
    public bool isGameActive;
    public bool escaped;
    private int score;
    private int highscore;
    private float spawnRate = 1.0f;
    private float savedVolume;
    public int totalLives;
    public int livesLeft;

    // Start is called before the first frame update
    void Start()
    {
        ability = GameObject.Find("Ability Manager").GetComponent<Ability>();
        totalLives = PlayerPrefs.GetInt("Lives", 0);
        settingsMenuComponent.SetTheme(PlayerPrefs.GetInt("Theme", 0));
        escaped = false;
        savedVolume = PlayerPrefs.GetFloat("Volume", 0);
        audioMixer.SetFloat("volume", savedVolume);
    }

    public void SetTotalLives(string value)
    {
        string inputText = value;
        int inputInt;
        if (int.TryParse(inputText, out inputInt))
        {
            totalLives = inputInt;
            PlayerPrefs.SetInt("Lives", totalLives);
            PlayerPrefs.Save();
        }
        else
        {
            totalLives = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenu.activeSelf && !gameOverMenu.activeSelf)
        {
            escaped = !escaped;
            EscapeMenu();
        }
    }

    void EscapeMenu()
    {
        if (escaped)
        {
            Time.timeScale = 0;
            settingsMenu.SetActive(true);
        }
        else if (!escaped)
        {
            Time.timeScale = 1;
            settingsMenu.SetActive(false);
        }
    }

    public IEnumerator SpawnTarget()
    {
        while(isGameActive)
        { 
        yield return new WaitForSeconds(spawnRate);
        int index = Random.Range(0, targets.Count);
        Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
        highscoreText.text = "Highscore: " + highscore;
    }

    public void UpdateHighscore()
    {
        if (score > highscore)
        {
            StartCoroutine(Celebration());
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
        else
        {
            gameOverAudio.Play();
            highscoreText2.fontSize = 76;
            highscoreText2.color = new Color32(0xFF, 0x6D, 0xF4, 0xFF);
            highscoreText2.rectTransform.anchoredPosition = new Vector2(103, -70);
            highscoreText2.text = "Highscore: " + highscore;
        }
    }

    IEnumerator Celebration()
    {
        highscoreText2.fontSize = 140;
        livesText.color = new Color32(0x2C, 0xE5, 0x6B, 0xFF);
        highscoreText2.rectTransform.anchoredPosition = new Vector2(-275, -40);
        highscoreText2.text = "New Highscore: " + score;
        highscoreAudio.Play();
        yield return new WaitForSeconds(0.8f);
        celebratoryAudio.Play();
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.2f);
            Instantiate(highscoreParticle, new Vector3(-5, 3, 0), highscoreParticle.transform.rotation);
            yield return new WaitForSeconds(0.1f);
            Instantiate(highscoreParticle2, new Vector3(0, 5f, 0), highscoreParticle2.transform.rotation);
            yield return new WaitForSeconds(0.1f);
            Instantiate(highscoreParticle3, new Vector3(5, 3, 0), highscoreParticle3.transform.rotation);
            yield return new WaitForSeconds(0.1f);
            Instantiate(highscoreParticle3, new Vector3(-3, 4, 0), highscoreParticle3.transform.rotation);
            yield return new WaitForSeconds(0.1f);
            Instantiate(highscoreParticle2, new Vector3(-5, 3, 0), highscoreParticle2.transform.rotation);
            Instantiate(highscoreParticle2, new Vector3(5, 3, 0), highscoreParticle2.transform.rotation);
            yield return new WaitForSeconds(0.1f);
            Instantiate(highscoreParticle3, new Vector3(3, 6, 0), highscoreParticle3.transform.rotation);
        }
    }

    public void UpdateLivesDisplay()
    {
        if (livesLeft < (totalLives * 0.75) && livesLeft >= (totalLives * 0.5))
        {
            livesText.color = new Color32(0x00, 0xFF, 0x0D, 0xFF);
        }
        else if (livesLeft < (totalLives * 0.5) && livesLeft >= (totalLives * 0.25))
        {
            livesText.color = new Color32(0xFF, 0xF2, 0x00, 0xFF);
        }
        else if (livesLeft < (totalLives * 0.25) && livesLeft >= 0)
        {
            livesText.color = new Color32(0xFF, 0x00, 0x38, 0xFF);
        }
        else
        {
            livesText.color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        }
        livesText.text = "HP: " + livesLeft;
    }

    public void GameOver()
    {
        scoreText2.text = "Score: " + score;
        isGameActive = false;
        gameOverMenu.SetActive(true);
        DissableScoreboard();
        UpdateHighscore();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        settingsMenuComponent.SetTheme(PlayerPrefs.GetInt("Theme", 0));
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        spawnRate /= difficulty;
        livesLeft = totalLives;
        score = 0;

        EnableScoreboard();
        StartCoroutine(SpawnTarget());
        UpdateScore(0);

        mainMenu.SetActive(false);

        highscore = PlayerPrefs.GetInt("HighScore", 0);
        highscoreText.text = "Highscore: " + highscore;
        settingsMenuComponent.SetTheme(PlayerPrefs.GetInt("Theme", 0));
        UpdateLivesDisplay();
    }

    void EnableScoreboard()
    {
        scoreText.gameObject.SetActive(true);
        highscoreText.gameObject.SetActive(true);
        ability.abilityText.gameObject.SetActive(true);
        borders.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
    }

    void DissableScoreboard()
    {
        scoreText.gameObject.SetActive(false);
        highscoreText.gameObject.SetActive(false);
        ability.abilityText.gameObject.SetActive(false);
        borders.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
    }
}
