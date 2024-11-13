using Game.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveFileManager : ExtendedMonoBehavior
{

    List<SaveFile> saveFiles = new List<SaveFile>();

    SaveFile currentSaveFile = null;


    public SaveFileManager()
    {


    }


    void Awake()
    {

    }


    void Start()
    {

    }

    private void Create(string name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            //Retrive name by environment
            name = SystemInfo.deviceUniqueIdentifier;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            name = Environment.UserName;
#elif UNITY_IOS || UNITY_ANDROID
            name = PlayerPrefs.GetString("Username", SystemInfo.deviceUniqueIdentifier);
#endif
        }1

        name = name.SanitizeFileName();
        Save(new SaveFile(name));
    }


    private void Save(SaveFile saveFile)
    {
        if (saveFile == null || !saveFile.IsValid())
        {
            Debug.LogError($"An invalid save file: {name} was specified.");
            return;
        }

        var filePath = GetFilePath(saveFile.Name);
        string json = JsonUtility.ToJson(saveFile, true);
        File.WriteAllText(filePath, json);
    }

    private SaveFile Get(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var filePath = GetFilePath(name);
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Failed to load save file: {name}.");
            return null;
        }

        string json = File.ReadAllText(filePath);
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(json);
        if (saveFile == null || !saveFile.IsValid())
            return null;

        return saveFile;
    }

    public void Load()
    {
        List<string> filePaths;
        List<string> GetFilePaths() => filePaths = Directory.GetFiles(Application.persistentDataPath, "save-*.json").ToList();

        GetFilePaths();
        if (filePaths == null || filePaths.Count < 1)
        {
            Create();
            GetFilePaths();
        }
           
        //Iterate accross all save file paths
        foreach (var x in filePaths)
        {
            string name = Path.GetFileNameWithoutExtension(x).Substring("save-".Length);
            if (string.IsNullOrWhiteSpace(name))
                continue;

            var saveFile = Get(name);
            if (saveFile == null || !saveFile.IsValid())
                continue;

            saveFiles.Add(saveFile);
        }

        if (saveFiles == null || saveFiles.Count < 1)
        {
            Debug.LogError($"Failed to load any save file.");
            return;
        }

        //TODO: Eventually let user select save file, for now just select first
        currentSaveFile = saveFiles[0];
    }

    private string GetFilePath(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return Path.Combine(Application.persistentDataPath, $"save-{name}.json");
    }

}
