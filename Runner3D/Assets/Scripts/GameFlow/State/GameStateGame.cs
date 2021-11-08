using TMPro;
using UnityEngine;
public class GameStateGame : GameState
{
    public GameObject gameUI;
    [SerializeField] TextMeshProUGUI fishCount;
    [SerializeField] TextMeshProUGUI scoreCount;
    public override void Construct()
    {
        GameManager.Instance.motor.ResumePlayer();
        GameManager.Instance.ChangeCamera(GameCamera.Game);

        GameStats.Instance.OnCollectFish += OnCollectFisf;
        GameStats.Instance.OnScoreChange += OnScoreChange; 
        gameUI.SetActive(true);
    }

    private void OnCollectFisf(int amnCollected)
    {
        fishCount.text = GameStats.Instance.FishToText();
    }
    private void OnScoreChange(float score)
    {
        scoreCount.text = GameStats.Instance.ScoreToText();
    }
    public override void Destruct()
    {
        gameUI.SetActive(false);
        GameStats.Instance.OnCollectFish -= OnCollectFisf;
        GameStats.Instance.OnScoreChange -= OnScoreChange;
    }
    public override void UpdateState()
    {
        GameManager.Instance.worldGeneration.ScanPosition();
        GameManager.Instance.sceneChunkGeneration.ScanPosition();
    }
}
