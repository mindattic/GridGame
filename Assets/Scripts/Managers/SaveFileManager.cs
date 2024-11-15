using Game.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SaveFileManager : ExtendedMonoBehavior
{
    //Variables
    public List<SaveFile> saveFiles = new List<SaveFile>();
    public SaveFile currentSaveFile = null;

    /// <summary>
    /// Method which is used to create a new save file
    /// </summary>
    private bool Create()
    {
        var guid = Guid.NewGuid().ToString().Replace("-", "").ToLower();
        var saveFile = new SaveFile(guid);
        return Save(saveFile);
    }

    /// <summary>
    /// Method which is used to update an existing save file 
    /// by converting to JSON and writing to file
    /// </summary>
    private bool Save(SaveFile saveFile)
    {
        var sw = Stopwatch.StartNew();

        if (saveFile == null || !saveFile.IsValid())
        {
            Debug.LogError($"An invalid save file was specified.");
            return false;
        }

        //Retrieve file path
        if (!saveFile.HasFilePath)
            saveFile.FilePath = GetFilePath(saveFile.Guid);

        //Verify file path
        if (string.IsNullOrWhiteSpace(saveFile.FilePath))
        {
            Debug.LogError($"An invalid file path: {saveFile.FilePath} was specified.");
            return false;
        }

        //Parse save file into json
        string json = JsonUtility.ToJson(saveFile, prettyPrint: true);
        if (string.IsNullOrWhiteSpace(json) || json == "{}")
        {
            Debug.LogError($"Failed to serialize save file: save-{saveFile.Guid}.json.");
            return false;
        }

        //Write json to file on hard drive
        File.WriteAllText(saveFile.FilePath, json);

        //Verify save file exists
        if (!File.Exists(saveFile.FilePath))
        {
            Debug.LogError($"Save file: {saveFile.Guid} does not exist.");
            return false;
        }

        sw.Stop();
        Debug.LogWarning($"Updated save-{saveFile.Guid}.json successfully in {sw.ElapsedMilliseconds} ms.");

        return true;
    }

    public bool QuickSave()
    {
        if (currentSaveFile == null || !currentSaveFile.IsValid())
        {
            Debug.LogError($"Current save file is not valid.");
            return false;
        }

        return Save(currentSaveFile);
    }

    public bool Reload()
    {
        if (currentSaveFile == null || !currentSaveFile.IsValid())
        {
            Debug.LogError($"Current save file is not valid.");
            return false;
        }

        currentSaveFile = Get(currentSaveFile.Guid);
        if (currentSaveFile == null || !currentSaveFile.IsValid())
        {
            Debug.LogError($"Current save file is not valid.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Method which is used to load all save files into List<>
    /// </summary>
    public void Load(int index = 0)
    {
        var sw = Stopwatch.StartNew();

        //Retrieve existing save-*.json files
        var jsonFilePaths = GetJsonFilePaths();

        //Create new save file if none are detected
        if (jsonFilePaths == null || jsonFilePaths.Count < 1)
        {
            if (Create())
                jsonFilePaths = GetJsonFilePaths();
            else
                Debug.LogError($"Failed to create a new save file.");
        }

        //Iterate accross all save json files
        foreach (var x in jsonFilePaths)
        {
            //Get guid out of filename
            string guid = Path.GetFileNameWithoutExtension(x).Substring("save-".Length);
            if (string.IsNullOrWhiteSpace(guid))
                continue;

            var saveFile = Get(guid);
            if (saveFile == null || !saveFile.IsValid())
                continue;

            saveFiles.Add(saveFile);
        }

        if (saveFiles == null || saveFiles.Count < 1)
        {
            Debug.LogError($"Failed to load any valid save file from: {string.Join(",", jsonFilePaths)}.");
            return;
        }

        //Load selected save file
        currentSaveFile = saveFiles[index];


        sw.Stop();
        Debug.LogWarning($"Loaded current save file in {sw.ElapsedMilliseconds} ms.");
    }

    /// <summary>
    /// Method which is used to retrieve an existing save file 
    /// by deserializing JSON and creating SaveFile object
    /// </summary>
    private SaveFile Get(string guid)
    {
        if (string.IsNullOrWhiteSpace(guid))
            return null;

        var filePath = GetFilePath(guid);
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Save file: {guid} does not exist.");
            return null;
        }

        string json = File.ReadAllText(filePath);
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(json);
        if (saveFile == null || !saveFile.IsValid())
        {
            Debug.LogError($"Failed to parse save file: {guid}.");
            return null;
        }

        return saveFile;
    }

    /// <summary>
    /// Method which is used to retrieve list of all .json 
    /// file paths from persistentDataPath
    /// </summary>
    private List<string> GetJsonFilePaths()
    {
        return Directory.GetFiles(Application.persistentDataPath, "save-*.json").ToList();
    }

    private string GetFilePath(string guid)
    {
        if (string.IsNullOrWhiteSpace(guid))
            return null;

        var path = Path.Combine(Application.persistentDataPath, $"save-{guid}.json");

        //Adjust folder path slashes based on environment
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        path = path.Replace("/", @"\");
#else
        path = path.Replace("\\", "/");
#endif

        return path;
    }

}
