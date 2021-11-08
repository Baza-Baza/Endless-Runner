using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStateShop : GameState
{
    public GameObject shopUI;
    public TextMeshProUGUI totalFsh;
    public TextMeshProUGUI currentHatName;
    public HatLogic hatLogic;

    public GameObject hatPrefab;
    public Transform hatContainer;
    private Hat[] hats;
    private bool isInit = false;


    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }
    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Shop);
        hats = Resources.LoadAll<Hat>("Hat");
        shopUI.SetActive(true);
        if (!isInit)
        {
            totalFsh.text = SaveManager.Instance.save.Fish.ToString("000");
            currentHatName.text = hats[SaveManager.Instance.save.CurrentHatIndex].ItemName;
            PopelateShop();
            isInit = true;
        }
    }

    public override void Destruct()
    {
        shopUI.SetActive(false);
    }
    public void PopelateShop()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            int index = i;
            GameObject go = Instantiate(hatPrefab, hatContainer);
            go.GetComponent<Button>().onClick.AddListener(() => OnHatClick(index));
            go.transform.GetChild(0).GetComponent<Image>().sprite = hats[index].Thumbnail;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hats[index].ItemName;

            if (SaveManager.Instance.save.UnlockedHatFlag[i] == 0)
                    go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = hats[index].ItemPrice.ToString();
            else
            {
                go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
            }
           
        }
    }
    private void OnHatClick(int i)
    {
        if (SaveManager.Instance.save.UnlockedHatFlag[i] == 1)
        {
            SaveManager.Instance.save.CurrentHatIndex = i;
            currentHatName.text = hats[i].ItemName;
            hatLogic.SelecHat(i);
            SaveManager.Instance.Save();
        }
        else if (hats[i].ItemPrice <= SaveManager.Instance.save.Fish)
        {
            SaveManager.Instance.save.Fish -= hats[i].ItemPrice;
            SaveManager.Instance.save.UnlockedHatFlag[i] = 1;
            SaveManager.Instance.save.CurrentHatIndex = i;
            currentHatName.text = hats[i].ItemName;
            hatLogic.SelecHat(i);
            totalFsh.text = SaveManager.Instance.save.Fish.ToString("000");
            SaveManager.Instance.Save();
            hatContainer.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";

        }
        else
        {
            Debug.Log("Not enoght fish");
        }
    }
    public void OnHomeClick()
    {
        brain.ChangeState(GetComponent<GameStateInit>());
    }
}
