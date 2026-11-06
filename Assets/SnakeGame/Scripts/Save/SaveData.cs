
[System.Serializable]
public class SaveData
{
    public int highScore;
}


public interface ISaveService
{
    SaveData Load();
    void Save(SaveData data);
}

