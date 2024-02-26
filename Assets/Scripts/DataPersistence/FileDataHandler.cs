using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class FileDataHandler 
{
    private string _dataDirPath = "";
    private bool _useEnctyption = false;
    private string _encryptionCodeWord = "word";

    public FileDataHandler(string dataDirPath, bool useEnctyption)
    {
        _dataDirPath = dataDirPath;
        _useEnctyption = useEnctyption;
    }

    public void SaveData(object myDataObject)
    {
        string fullPath = Path.Combine(_dataDirPath, myDataObject.GetType().Name);
        try
        {
            string dataToStore = JsonUtility.ToJson(myDataObject, true);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            if (_useEnctyption)
            {
                dataToStore = EcncryptDecrypt(dataToStore);
            }

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

    public T LoadData<T>(object myDataObject)
    {
        string fullPath = Path.Combine(_dataDirPath, 
            myDataObject.GetType().Name);

        T data = default;
        if (File.Exists(fullPath))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (_useEnctyption)
                {
                    dataToLoad = EcncryptDecrypt(dataToLoad);
                }

                return JsonUtility.FromJson<T>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }

        } 
        return data;
    }

    private string EcncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
