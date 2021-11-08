using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateDeath : GameState
{
    public GameObject deathUI;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI fishTotal;
    [SerializeField] private TextMeshProUGUI currentFish;


    [SerializeField] private Image completionCircle;
    public float timeToDecision = 2.5f;
    private float deathTime;
    public override void Construct()
    {
        GameManager.Instance.motor.PausePlayer();
        deathUI.SetActive(true);
        
        deathTime = Time.time;

        if (SaveManager.Instance.save.Highscore < (int)GameStats.Instance.score)
        {
            SaveManager.Instance.save.Highscore = (int)GameStats.Instance.score;
            currentScore.color = Color.green;
        }
        else
        {
            currentScore.color = Color.white;
        }


        SaveManager.Instance.save.Fish += GameStats.Instance.fishCollectedThisSession;

        SaveManager.Instance.Save();

        completionCircle.gameObject.SetActive(true);
        highscore.text = "HIGHSCORE :"+ SaveManager.Instance.save.Highscore;
        currentScore.text = GameStats.Instance.ScoreToText();
        fishTotal.text = "TOTAL FISH : " + SaveManager.Instance.save.Fish;
        currentFish.text = GameStats.Instance.FishToText();
    }
    public override void Destruct()
    {
        deathUI.SetActive(false);
    }
    public override void UpdateState()
    {
        float ratio = (Time.time - deathTime) / timeToDecision;
        completionCircle.color = Color.Lerp(Color.green, Color.red, ratio);
        completionCircle.fillAmount = 1 -ratio;

        if (ratio > 1)
        {
            completionCircle.gameObject.SetActive(false);
        }

    }
    public void ToMenu()
    {
        

        brain.ChangeState(GameManager.Instance.GetComponent<GameStateInit>());
        GameManager.Instance.motor.ResetPlayer();
        GameManager.Instance.worldGeneration.ResetWorld();
        GameManager.Instance.sceneChunkGeneration.ResetWorld();

    }
    public void ResumeTheGAme()
    {

        brain.ChangeState(GameManager.Instance.GetComponent<GameStateGame>());
        GameManager.Instance.motor.RespawnPlayer();
       

       
    }
}
  
