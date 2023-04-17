using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    static BinaryFormatter binaryFormatter = new BinaryFormatter();
    static string dataPath = Application.persistentDataPath + "/color.data";
    static ColorData colorsData = new ColorData();

    public static void SaveColorData(string hexModel)//no es hexmodel es colorData(rgb pero en porcentajes)
    {
        if (!File.Exists(dataPath)) //When no exist path
            using (FileStream fs = File.Create(dataPath))
            {
                Debug.Log("Path created");
                colorsData.SetHexModel(hexModel);
                binaryFormatter.Serialize(fs, colorsData);
                Debug.Log($"Saved: {hexModel}");
                return;
            }

        else if (IsEmptyFile()) //When path exist but no contains info
            using (FileStream writter = File.OpenWrite(dataPath))
            {
                Debug.Log($"Empty file");
                colorsData.SetHexModel(hexModel);
                binaryFormatter.Serialize(writter, colorsData);
                Debug.Log($"Saved: {hexModel}");
                return;
            }

        /*  When path exist and contains data  */
        using (FileStream opener = File.Open(dataPath, FileMode.Open)) //Recovering data and adding new info
        {
            colorsData = (ColorData)binaryFormatter.Deserialize(opener);
            colorsData.SetHexModel(hexModel);
        }

        using (FileStream writer = new FileStream(dataPath, FileMode.Open)) //Saving new data
        {
            binaryFormatter.Serialize(writer, colorsData);
            Debug.Log($"Saved: {hexModel}");
        }
        //ShowHexColors(colorsData);
    }

#nullable enable

    public static int CountData() //count data on file
    {
        if (!File.Exists(dataPath))
        {
            Debug.LogError("No path created to count data");
            return 0;
        }
        else if (IsEmptyFile())
        {
            Debug.LogError("Empty file, no counted data");
            return 0;
        }

        using (FileStream opener = File.Open(dataPath, FileMode.Open))
        {
            colorsData = (ColorData)binaryFormatter.Deserialize(opener);
        }

        return colorsData.GetHexModels().Count;
    }

    public static ColorData? LoadColorData()
    {
        /*
         * When path no exist \ok\
         * when path exist but is empty \ok\
         * when path contains info \ok\
         * 
         */

        if (!File.Exists(dataPath))
        {
            Debug.LogError("Path no exist");
            return null;
        }

        if (IsEmptyFile())
        {
            Debug.Log($"Empty file");
            return null;
        }

        /*  Recovering data  */
        using (FileStream reader = File.OpenRead(dataPath))
        {
            Debug.Log("Data recovered");
            ColorData hexData = (ColorData)binaryFormatter.Deserialize(reader);
            return hexData;
        }
    }
#nullable disable
    
    static bool IsEmptyFile()
    {
        try
        {
            using (FileStream oFS = File.Open(dataPath, FileMode.Open))
            {
                /*  If the path is empty this block trhows an exception */
                colorsData = (ColorData)binaryFormatter.Deserialize(oFS);
                return false;
            }
        }
        catch (SerializationException)
        {
            return true;
            //throw;
        }
    }

    public static void DeleteData()
    {
        colorsData = new ColorData();
        File.Delete(dataPath);
    }

    static void ShowHexColors(ColorData colorsData, string msm ="")
    {
        foreach (string hex in colorsData.GetHexModels())
        {
            Debug.Log($"{msm} Conteo: {colorsData.GetHexModels().Count}, HEX: {hex}");
        }
    }

}
