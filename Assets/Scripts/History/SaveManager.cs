using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    static string dataPath = Application.persistentDataPath + "/color.data";
    static BinaryFormatter binaryFormatter = new BinaryFormatter();


    /*ESTE TAMBIEN 3
    string hexModel
    */
    public static void SaveColorData(string hexModel)
    {
        /*
         * when path no exist \ok\
         * when path exist but no contains info \ok\
         * when path exist and contains info \ok\
         * 
         */

        ColorData colorsData = new ColorData();

        if (!File.Exists(dataPath))
            using (FileStream fs = File.Create(dataPath))
            {
                Debug.Log("Path created");
                colorsData.SetHexModel(hexModel);
                binaryFormatter.Serialize(fs, colorsData);
                Debug.Log($"Saved: {hexModel}");
                return;
            }

        else if (IsEmptyFile())
            using (FileStream writter = File.OpenWrite(dataPath))
            {
                Debug.Log($"Empty file");
                colorsData.SetHexModel(hexModel);
                binaryFormatter.Serialize(writter, colorsData);
                Debug.Log($"Saved: {hexModel}");
                return;
            }

        /*  When path contains data  */
        //Recovering data and adding new info
        using (FileStream opener = File.Open(dataPath, FileMode.Open))
        {
            colorsData = (ColorData)binaryFormatter.Deserialize(opener);
            colorsData.SetHexModel(hexModel);
        }

        //Saving new data
        using (FileStream writer = new FileStream(dataPath, FileMode.Open))
        {
            binaryFormatter.Serialize(writer, colorsData);
            Debug.Log($"Saved: {hexModel}");
        }
        //ShowHexColors(colorsData);
    }

#nullable enable
    public static ColorData? LoadHexData()
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

    private static bool IsEmptyFile()
    {
        try
        {
            using (FileStream oFS = File.Open(dataPath, FileMode.Open))
            {
                /*  If the path is empty this block trhows an exception */
                ColorData colorsData = (ColorData)binaryFormatter.Deserialize(oFS);
                return false;
            }
        }
        catch (SerializationException)
        {
            return true;
            //throw;
        }
    }

    public static void ShowHexColors(ColorData colorsData, string msm ="")
    {
        foreach (string hex in colorsData.GetHexModels())
        {
            Debug.Log($"{msm} Conteo: {colorsData.GetHexModels().Count}, HEX: {hex}");
        }
    }

}
