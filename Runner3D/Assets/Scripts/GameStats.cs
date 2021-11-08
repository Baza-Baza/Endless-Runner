using System;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get { return instance; } }
    private static GameStats instance;

    public float score;
    public float highscore;
    public float distanceModifier = 1.5f;

    public int totalFish;
    public int fishCollectedThisSession;
    public int pointsPerFish = 10;

    private float lastScoreUpdate;
    private float scoreUpdateDelta = 0.2f;

    public Action<int> OnCollectFish;
    public Action<float> OnScoreChange;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        float s = GameManager.Instance.motor.transform.position.z * distanceModifier;
        s += fishCollectedThisSession * pointsPerFish;
        if (s >= score)
        {
            score = s;
            if (Time.time - lastScoreUpdate > scoreUpdateDelta)
            {
                lastScoreUpdate = Time.time;
                OnScoreChange?.Invoke(score);
            }
        }

    }
    public void CollectFish()
    {
        fishCollectedThisSession++;
        OnCollectFish?.Invoke(fishCollectedThisSession);
    }
    public void ResetSession()
    {
        score = 0;
        fishCollectedThisSession = 0;
        OnScoreChange?.Invoke(score);
        OnCollectFish?.Invoke(fishCollectedThisSession);
    }
    public string ScoreToText()
    {
        return score.ToString("0000000");
    }
    public string FishToText()
    {
        return fishCollectedThisSession.ToString("000");
    }
}
    