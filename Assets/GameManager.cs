using System;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float maxTimer;
    private float remainingTime;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;
    [SerializeField] private TextMeshProUGUI pictureAmountEndText;
    [SerializeField] private TextMeshProUGUI pictureAmountText;
    private int amountOfPictures;
    
    [SerializeField] private GameEventChannel eventChannel;

    [SerializeField] private Transform endGameScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        eventChannel.OnEvidenceCaptured += EvidenceCaptured;
        eventChannel.OnEvidenceShot += EvidenceShot;
        remainingTime = maxTimer;
    }

    private void EvidenceShot(object sender, EventArgs e)
    {
        amountOfPictures++;
        pictureAmountText.text = amountOfPictures.ToString();
    }

    private void EvidenceCaptured(object sender, EventArgs e)
    {
        score++;
        //scoreText.text = score.ToString();
    }
    
    // Update is called once per frame
    void Update()
    {
        float minutes = Mathf.FloorToInt(remainingTime / 60);
        float seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        remainingTime -= Time.deltaTime;
        if (remainingTime < 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        pictureAmountEndText.text = amountOfPictures.ToString();
        scoreText.text = score.ToString();
        endGameScreen.gameObject.SetActive(true);
    }

    public void Restart()
    {
        /*Time.timeScale = 1;
        endGameScreen.gameObject.SetActive(false);*/
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
