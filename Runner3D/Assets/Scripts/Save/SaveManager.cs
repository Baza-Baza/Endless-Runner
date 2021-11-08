using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; } }
    private static SaveManager instance;

    private const string saveFiledName = "data.ggwp";
    private BinaryFormatter formatter;

    public SaveState save;

    public Action<SaveState> OnLoad;
    public Action<SaveState> OnSave;

    private void Awake()
    {
        instance = this;
        formatter = new BinaryFormatter();
        Load();
    }

    private void Load()
    {

        try
        {
            FileStream file = new FileStream(Application.persistentDataPath + saveFiledName, FileMode.Open, FileAccess.Read);
            save = (SaveState)formatter.Deserialize(file);
            file.Close();
            OnLoad?.Invoke(save);
        }
        catch
        {
            Debug.Log("Save file not found");
            Save();
        }
       
    }
    public void Save()
    {
        if (save == null)
            save = new SaveState();

        save.LastSaveTime = DateTime.Now;

        FileStream file = new FileStream(Application.persistentDataPath + saveFiledName, FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(file, save);
        file.Close();


        OnSave?.Invoke(save);
    }
}
