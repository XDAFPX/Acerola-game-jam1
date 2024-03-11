using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveNLoadManager : MonoBehaviour
{
    public static SaveNLoadManager sing;
    public static SaveNLoadManager Singleton { get { return sing; } set { if (sing == null) sing = value; else if(value!=sing) { Destroy(value.gameObject); } } }
    public bool IsLoading;
    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(gameObject);
        MainMenuStart();
    }
    public void MainMenuStart()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            bool ise = System.IO.File.Exists(Application.persistentDataPath + "/Save.json");
            if (!ise)
            {
                Save(GenereteNewGameSave());
            }

        }
    }
    public void Continue()
    {
        string path = Application.persistentDataPath + "/Save.json";
        SaveData data = JsonUtility.FromJson<SaveData>(System.IO.File.ReadAllText(path));
        if (!data.IsDefaultSave)
        {
            Load();
        }
    }
    public void AdvanceStage(int stage)
    {
        string path = Application.persistentDataPath + "/Save.json";
        SaveData data = JsonUtility.FromJson<SaveData>(System.IO.File.ReadAllText(path));
        data.SavePos = stage;
        Save(data);
    }
    public void StartNewGame()
    {
        Save(GenereteNewGameSave());
        Load();
    }
    public SaveData GenereteNewGameSave()
    {
        return new SaveData(new Item[0], 100, 1, 32, 20, true);
    }
    public void Load()
    {
        string path = Application.persistentDataPath + "/Save.json";
        SaveData data = JsonUtility.FromJson<SaveData>(System.IO.File.ReadAllText(path));
        if(data.SavePos == 1)
        {
            SceneManager.LoadScene("P1_a1");
        }
        else if( data.SavePos==2) SceneManager.LoadScene("P2_a1");
        else if( data.SavePos==3) SceneManager.LoadScene("P2_a2");
        IsLoading = true;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (IsLoading)
        {
            string path = Application.persistentDataPath + "/Save.json";
            SaveData data = JsonUtility.FromJson<SaveData>(System.IO.File.ReadAllText(path));
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Hp = data.Hp;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ShotgunAmmo = new Ammo(data.shotgunshels, BaseGun.AmmoType.shotgunshels);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PistolAmmo = new Ammo(data.shotgunshels, BaseGun.AmmoType.bullet);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Items = data.inventory.ToList();
            IsLoading = false;
        }
        
    }
    public void SaveState()
    {
        var pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        int pos = 1;
        if (SceneManager.GetActiveScene().name == "P1_a1") pos = 1; 
        if (SceneManager.GetActiveScene().name == "P2_a1") pos = 2; 
        if (SceneManager.GetActiveScene().name == "P2_a2") pos = 3;
        Save(new SaveData(pl.Items.ToArray(), pl.Hp, pos, pl.PistolAmmo.count, pl.ShotgunAmmo.count, false));
    }
    public void Save(SaveData data)
    {
        string path = Application.persistentDataPath+"/Save.json";
        string jsondata  = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(path, jsondata);
    }
}
[System.Serializable]
public class SaveData
{
    public Item[] inventory; public float Hp; public int SavePos; public int pistolammo; public int shotgunshels; public bool IsDefaultSave;

    public SaveData(Item[] _inventory, float _Hp, int _SaveState, int _pistolammo ,int _shotgunshels,bool isdefault)
    {
        inventory = _inventory;
        Hp = _Hp;
        SavePos = _SaveState;
        pistolammo = _pistolammo;
        shotgunshels = _shotgunshels;
        IsDefaultSave = isdefault;
    }
}
