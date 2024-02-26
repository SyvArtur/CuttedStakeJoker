using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DataPersistenceManager : MonoBehaviour
{
    private static DataPersistenceManager _instance;
    [HideInInspector] public ShoppingData _ShoppingData;
    [HideInInspector] public RecordsData _RecordsData;
    [HideInInspector] public AchievementsData _AchievementsData;
    [HideInInspector] public SettingsData _SettingsData;
    //private List<IShoppingDataPersistence> _shoppingDataPersistenceList = new List<IShoppingDataPersistence>();
    private FileDataHandler _fileDataHandler;
    [SerializeField] private bool useEncryption;

/*    public void SubscribeToShoppingDataPersistence(IShoppingDataPersistence shoppingDataPersistence)
    {
        if (!_shoppingDataPersistenceList.Contains(shoppingDataPersistence))
        {
            _shoppingDataPersistenceList.Add(shoppingDataPersistence);
        }
    }*/

    public static DataPersistenceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DataPersistenceManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("DataPersistenceManager");
                    _instance = singletonObject.AddComponent<DataPersistenceManager>();
                }

                //DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private void Awake()
    {
        //_shoppingDataPersistenceList = new List<IShoppingDataPersistence>(FindObjectsOfType<MonoBehaviour>().OfType<IShoppingDataPersistence>());
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, useEncryption);
        LoadGame();
    }

    public void LoadGame()
    {
        _ShoppingData = _fileDataHandler.LoadData<ShoppingData>(_ShoppingData);
        if (_ShoppingData == null)
        {
            _ShoppingData = new ShoppingData();
        }

        _RecordsData = _fileDataHandler.LoadData<RecordsData>(_RecordsData);
        if (_RecordsData == null)
        {
            _RecordsData = new RecordsData();
        }

        _AchievementsData = _fileDataHandler.LoadData<AchievementsData>(_AchievementsData);
        if (_AchievementsData == null)
        {
            _AchievementsData = new AchievementsData();
        }

        _SettingsData = _fileDataHandler.LoadData<SettingsData>(_SettingsData);
        if (_SettingsData == null)
        {
            _SettingsData = new SettingsData();
        }
    }

    public void SaveGame()
    {
        _fileDataHandler.SaveData(_ShoppingData);
        _fileDataHandler.SaveData(_RecordsData);
        _fileDataHandler.SaveData(_AchievementsData);
        _fileDataHandler.SaveData(_SettingsData);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveGame();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
