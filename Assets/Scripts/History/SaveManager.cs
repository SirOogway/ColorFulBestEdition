using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    static string dataPath = Application.persistentDataPath + "/color.data";
    static BinaryFormatter binaryFormatter = new BinaryFormatter();

    public static void SaveColorData(string color)
    {
        ColorData colorData = new ColorData(color);
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        binaryFormatter.Serialize(fileStream, colorData);
        fileStream.Close();
    }

#nullable enable
    public static ColorData? LoadColorData()
    {
        if (File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            ColorData colorData = (ColorData) binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return colorData;
        }
        else
        {
            Debug.LogError("there is no route or color data");
            return null;
        }
    }
}
