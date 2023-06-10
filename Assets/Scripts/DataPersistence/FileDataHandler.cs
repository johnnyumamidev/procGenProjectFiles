using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirectoryPath = "";

    private string dataFileName = "";

    public FileDataHandler(string dataDirectoryPath, string dataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    } 

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }   
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        try
        {
            // create directory path
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}
