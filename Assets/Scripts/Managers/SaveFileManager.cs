using Game.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SaveFileManager : ExtendedMonoBehavior
{

    public const bool PRETTY_PRINT = true;

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

        //Retrieve file directoryPath
        if (!saveFile.HasFilePath)
            saveFile.FilePath = GetFilePathFromGuid(saveFile.Guid);

        //Verify file directoryPath
        if (string.IsNullOrWhiteSpace(saveFile.FilePath))
        {
            Debug.LogError($"An invalid file directoryPath: {saveFile.FilePath} was specified.");
            return false;
        }

        //Parse save file into json
        string json = JsonUtility.ToJson(saveFile, PRETTY_PRINT);
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

        currentSaveFile = GetSaveFile(currentSaveFile.Guid);
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
    public void LoadSaveFiles(int index = 0)
    {
        var sw = Stopwatch.StartNew();

        //Retrieve existing {GUID}\save.json file paths
        var saveJsonFilePaths = GetSaveJsonFilePaths();

        //Create new save file if none are detected
        if (saveJsonFilePaths == null || saveJsonFilePaths.Count < 1)
        {
            if (Create())
                saveJsonFilePaths = GetSaveJsonFilePaths();
            else
                Debug.LogError($"Failed to create a new save file.");
        }

        //Iterate accross all save json files
        foreach (var filePath in saveJsonFilePaths)
        {
            //GetSaveFile guid out of filename
            string guid = GetGuidFromFilePath(filePath);
            if (string.IsNullOrWhiteSpace(guid))
                continue;

            var saveFile = GetSaveFile(guid);
            if (saveFile == null || !saveFile.IsValid())
                continue;

            saveFiles.Add(saveFile);
        }

        if (saveFiles == null || saveFiles.Count < 1)
        {
            Debug.LogError($"Failed to load any valid save file from: {string.Join(",", saveJsonFilePaths)}.");
            return;
        }

        //LoadSaveFiles selected save file
        currentSaveFile = saveFiles[index];


        sw.Stop();
        Debug.LogWarning($"Loaded current save file in {sw.ElapsedMilliseconds} ms.");
    }

    public void Delete()
    {

    }

    /// <summary>
    /// Method which is used to retrieve an existing save file 
    /// by deserializing JSON and creating SaveFile object
    /// </summary>
    private SaveFile GetSaveFile(string guid)
    {
        if (string.IsNullOrWhiteSpace(guid))
            return null;

        var filePath = GetFilePathFromGuid(guid);
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
    private List<string> GetSaveJsonFilePaths()
    {
        List<string> jsonFilePaths = new List<string>();

        //Retrieve all save file folders under persistent data directoryPath
        var saveFileFolders = Directory.GetDirectories(Application.persistentDataPath).ToList();

        //Retrieve each save.json full directoryPath and filename in each folder
        foreach (string folder in saveFileFolders)
        {
            var files = Directory.GetFiles(folder, "save.json", SearchOption.TopDirectoryOnly).ToList();
            jsonFilePaths.AddRange(files);
        }

        return jsonFilePaths;
    }

    /// <summary>
    /// Method which is used to extract guid from json file directoryPath
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private string GetGuidFromFilePath(string filePath)
    {
        return new DirectoryInfo(Path.GetDirectoryName(filePath)).Name;
    }


    private string GetFilePathFromGuid(string guid)
    {
        if (string.IsNullOrWhiteSpace(guid))
            return null;

        var directoryPath = Path.Combine(Application.persistentDataPath, guid);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var filePath = Path.Combine(directoryPath, "save.json");

//        //Adjust folder directoryPath slashes based on environment
//#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
//        directoryPath = directoryPath.Replace("//", "/").Replace("/", @"\");
//#else
//        path = path.Replace("\\", "\");
//#endif

        return filePath;
    }

}
