using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace assets
{
    public class DataController
    {
        public static GameData LoadGameData(string gameDataFileName)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
            string jsonString;

            if (Application.platform == RuntimePlatform.Android) //Need to extract file from apk first
            {
                WWW reader = new WWW(filePath);
                while (!reader.isDone) { }
                jsonString = reader.text;
            }
            else
            {
                jsonString = File.ReadAllText(filePath);
            }

            GameData deserializedJsonObject = JsonUtility.FromJson<GameData>(jsonString);
            return deserializedJsonObject;

        }
    }
}