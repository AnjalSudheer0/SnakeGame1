
using System;
using System.IO;
using UnityEngine;

public class JsonSaveService : ISaveService
{
    private readonly string savePath;

    public JsonSaveService()
    {
        savePath = Path.Combine(
            Application.persistentDataPath,
            "snake_save.json"
        );
    }

    public SaveData Load()
    {
        if (!File.Exists(savePath))
            return new SaveData();

        try
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data == null || data.highScore < 0)
                return new SaveData();

            return data;
        }
        catch (Exception exception)
        {
            Debug.LogWarning("Save file could not be loaded: " + exception.Message);
            return new SaveData();
        }
    }

    public void Save(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
        }
        catch (Exception exception)
        {
            Debug.LogError("Could not save game data: " + exception.Message);
        }
    }
}
