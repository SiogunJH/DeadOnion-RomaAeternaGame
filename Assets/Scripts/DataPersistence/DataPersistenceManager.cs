using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviourSingleton<DataPersistanceManager>
{
    [SerializeField]
    private string _fileName;


    private GameData _gameData;
    private List<IDataPersistence> _persistenceList;
    private FileDataHandler _fileDataHandler;

    private void Start()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
        _persistenceList = FindAllDataPersistence();
        LoadGame();
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }
    public void SaveGame()
    {
        //pass gamedate to other scripts to update it
        foreach(var obj in _persistenceList)
        {
            obj.SaveData(ref _gameData);
        }
        //save gamedata to file using handler
        _fileDataHandler.Save(_gameData);
    }
    public void LoadGame()
    {
        //load gamadata from saved file using handler
        _gameData = _fileDataHandler.Load();
        //new game if no game found
        if(_gameData == null)
        {
            NewGame();
        }
        //push loaded data to other scripts
        foreach(var obj in _persistenceList)
        {
            obj.LoadData(_gameData);
        }
    }

    private List<IDataPersistence> FindAllDataPersistence()
    {
        return new List<IDataPersistence>(FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>());
    }
}
