using TMPro;
using UnityEngine;
public class GameStateInit : GameState
{
    public GameObject menuUI;
    [SerializeField] private TextMeshProUGUI hiscoreText;
    [SerializeField] private TextMeshProUGUI fishCountText;
    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Init);
        hiscoreText.text = "HIGHSCORE : " + SaveManager.Instance.save.Highscore.ToString();
        fishCountText.text = "FISH : " + SaveManager.Instance.save.Fish.ToString(); ;

        menuUI.SetActive(true);
    }
    public override void Destruct()
    {
        menuUI.SetActive(false);
    }

    public void OnPlayClick()
    {
        brain.ChangeState(GetComponent<GameStateGame>());
        GameStats.Instance.ResetSession();
    }
    public void OnShopClick()
    {
        brain.ChangeState(GetComponent<GameStateShop>());
    }
        
}
