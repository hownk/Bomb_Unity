using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 싱클턴패턴의 게임매니저 객체
    public static GameManager instance; 

    public GameObject messagePanel;   // 레디 UI

    // 점수 및 메시지(레디, 엔드) 텍스트
    public Text scoreText;
    public Text bestScoreText;
    public Text messageText;

    // 플레이 페이즈인지 아닌지 체크할 변수
    public bool isRoundActive = false;

    // 점수 기록용
    private int score = 0;

    // 포신과 카메라 스크립트 제어를 위한 변수
    public ShooterRotator shooterRotator;
    public CamFollow cam;

    // 생성자를 private으로 해서 외부에서 객체생성 막기
    private GameManager() 
    { }

    // static객체 자신으로 초기화
    private void Awake()
    {
        instance = this;
        UpdateUI();
    }

    // 코루틴함수 시작
    private void Start()
    {
        StartCoroutine("RoundRoutine");
    }

    // 점수 더해주면서 BestScore(), UI() 업데이트
    public void AddScore(int newScore)
    {
        score += newScore;
        UpdateBestScore();
        UpdateUI();
    }

    // PlayerPrefs를 이용해서 최고점수를 기록할 것이다.
    void UpdateBestScore()
    {
        if (GetBestScore() < score)
            PlayerPrefs.SetInt("BestScore", score);
    }

    int GetBestScore()
    {
        return PlayerPrefs.GetInt("BestScore");
    }

    void UpdateUI()
    {
        scoreText.text = "Score : " + score;
        bestScoreText.text = "Best Score : " + GetBestScore();
    }

    // 플레이 페이즈가 끝났음을 알리는 기능
    public void OnBallDestroy()
    {
        UpdateUI();
        isRoundActive = false;
    }

    public void Reset()
    {
        score = 0;
        UpdateUI();

        // 라운드 다시 처음부터 시작
        StartCoroutine("RoundRoutine");
    }


    IEnumerator RoundRoutine()
    {
        // 레디페이즈
        messagePanel.SetActive(true);
        cam.SetTarget(shooterRotator.transform, CamFollow.State.Idle);
        shooterRotator.enabled = false;

        isRoundActive = false;
        messageText.text = "Ready...";
        yield return new WaitForSeconds(3.0f);


        // 플레이페이즈
        isRoundActive = true;
        messagePanel.SetActive(false);
        shooterRotator.enabled = true;
        cam.SetTarget(shooterRotator.transform, CamFollow.State.Ready);

        while(isRoundActive == true)
        {
            yield return null;
        }


        // 엔드페이즈
        messagePanel.SetActive(true);
        shooterRotator.enabled = false;
        messageText.text = "Wait For Next Round...";

        yield return new WaitForSeconds(3.0f);
        Reset();
    }
}
