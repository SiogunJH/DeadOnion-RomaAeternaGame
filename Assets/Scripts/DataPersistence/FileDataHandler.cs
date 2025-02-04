using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string fileName = "";
    public FileDataHandler(string dataDirPath, string fileName)
    {
        this.dataDirPath = dataDirPath;
        this.fileName = fileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, fileName);
        GameData loadedData = null;
        if (!File.Exists(fullPath))
        {
            return loadedData;
        }

        string dataToLoad = "";
        using(FileStream stream = new FileStream(fullPath, FileMode.Open))
        {
            using(StreamReader reader = new StreamReader(stream))
            {
                dataToLoad = reader.ReadToEnd();
            }
        }

        loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
        return loadedData;
    }
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        string dataToStore = JsonUtility.ToJson(fullPath, true);

        using(FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            using(StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(dataToStore);
            }
        }
    }
}

