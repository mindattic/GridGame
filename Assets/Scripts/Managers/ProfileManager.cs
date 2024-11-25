using Game.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ProfileManager : ExtendedMonoBehavior
{
    public const bool PRETTY_PRINT = true;

    public List<Profile> profiles = new List<Profile>();
    public Profile currentProfile = null;


    public void LoadProfiles(int index = 0)
    {
        var sw = Stopwatch.StartNew();

        //Validate folder structure
        if (!IsValidFolders())
        {
            Debug.LogError($"Folder structure is invalid.");
            return;
        }

        //Retrieve existing profile folders
        var folders = Directory.GetDirectories(FileIO.Folders.Profiles).ToList();

        //If no profile folders found...
        if (folders == null || folders.Count < 1)
        {
            //...create a new profile folder with associated JSON files...
            var successful = CreateProfile();
            if (!successful)
            {
                Debug.LogError($"Failed to create a new profile.");
                return;
            }

            //Retrieve newly created profile folders
            folders = Directory.GetDirectories(FileIO.Folders.Profiles).ToList();
        }

        //Validate profile folders exist
        if (folders == null || folders.Count < 1)
        {
            Debug.LogError($"Failed to retrieve any profile folders from: {FileIO.Folders.Profiles}");
            return;
        }

        //Retrive each profile object
        foreach (var folder in folders)
        {
            //Retrieve GUID from folder name
            string guid = new DirectoryInfo(folder).Name;
            if (string.IsNullOrWhiteSpace(guid))
                continue;

            //Retrieve profile from GUID
            var profile = GetProfile(guid);
            if (profile == null || !profile.IsValid())
                continue;

            profiles.Add(profile);
        }

        if (profiles == null || profiles.Count < 1)
        {
            Debug.LogError($"Failed to load any valid profiles.");
            return;
        }

        sw.Stop();
        Debug.LogWarning($"Loaded current save file in {sw.ElapsedMilliseconds} ms.");
    }

    /// <summary>
    /// Method which is used to create a new folder with GUID containing JSON files
    /// </summary>
    private bool CreateProfile()
    {
        //Generate a new GUID
        var guid = Guid.NewGuid().ToString().ToLower();

        //TODO: Verify guid is unique in folders (extremely unlikely)

        //Instantiate a new Profile object with the generated GUID; create folder
        var profile = new Profile(guid);

        //Save the individual JSON files
        bool globalSaved = SaveComponent(profile.Global, profile);
        bool stageSaved = SaveComponent(profile.Stage, profile);
        bool partySaved = SaveComponent(profile.Party, profile);

        // Check if all files were saved successfully
        if (globalSaved && stageSaved && partySaved)
        {
            Debug.Log($"Created new profile with GUID: {guid}");
            return true;
        }
        else
        {
            Debug.LogError($"Failed to create new profile with GUID: {guid}");
            return false;
        }
    }

    /// <summary>
    /// SaveProfile individual components to separate JSON files
    /// </summary>
    private bool SaveComponent(ProfileComponent component, Profile profile)
    {
        var sw = Stopwatch.StartNew();

        var filePath = Path.Combine(profile.Folder, component.FileName);
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Debug.LogError($"Invalid file path for: {filePath}");
            return false;
        }

        string json = JsonUtility.ToJson(component, PRETTY_PRINT);
        if (string.IsNullOrWhiteSpace(json) || json == "{}")
        {
            Debug.LogError($"Failed to serialize {json}.");
            return false;
        }

        File.WriteAllText(filePath, json);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"{filePath} does not exist after saving.");
            return false;
        }

        sw.Stop();
        Debug.Log($"Saved {component.FileName} successfully in {sw.ElapsedMilliseconds} ms.");

        return true;
    }

    private T LoadComponent<T>(string guid, string fileName) where T : class
    {
        var sw = Stopwatch.StartNew();

        if (string.IsNullOrWhiteSpace(guid))
            return null;

        var folder = Path.Combine(FileIO.Folders.Profiles, guid);
        var filePath = Path.Combine(folder, fileName);
        if (!File.Exists(filePath))
        {
            Debug.LogError($"{filePath} does not exist.");
            return null;
        }

        string json = File.ReadAllText(filePath);
        T component = JsonUtility.FromJson<T>(json);
        if (component == null)
        {
            Debug.LogError($"Failed to deserialize {fileName}.");
        }

        sw.Stop();
        Debug.Log($"Loaded {fileName} successfully in {sw.ElapsedMilliseconds} ms.");

        return component;
    }

    private bool SaveProfile(Profile profile)
    {
        var sw = Stopwatch.StartNew();

        if (profile == null || !profile.IsValid())
        {
            Debug.LogError($"An invalid save file was specified.");
            return false;
        }

        bool globalSaved = SaveComponent(profile.Global, profile);
        bool stageSaved = SaveComponent(profile.Stage, profile);
        bool partySaved = SaveComponent(profile.Party, profile);

        sw.Stop();

        if (!globalSaved || !stageSaved || !partySaved)
        {
            Debug.LogError($"Failed to save one or more components.");
            return false;
        }

        Debug.LogWarning($"Saved all components successfully in {sw.ElapsedMilliseconds} ms.");
        return true;
    }

    public bool QuickSave()
    {
        if (currentProfile == null || !currentProfile.IsValid())
        {
            Debug.LogError($"Current save file is not valid.");
            return false;
        }

        return SaveProfile(currentProfile);
    }


    private Profile GetProfile(string guid)
    {
        if (string.IsNullOrWhiteSpace(guid))
        {
            Debug.LogError($"An invalid GUID was specified: {guid}");
            return null;
        }

        var profile = new Profile(guid);

        var global = LoadComponent<Global>(guid, "global.json");
        var stage = LoadComponent<Stage>(guid, "stage.json");
        var party = LoadComponent<Party>(guid, "party.json");

        profile.Global = global;
        profile.Stage = stage;
        profile.Party = party;

        if (!profile.IsValid())
        {
            Debug.LogError($"Failed to instantiate profile: {guid}");
            return null;
        }

        return profile;
    }


    public void Select(int index)
    {
        if (!HasProfiles())
        {
            Debug.LogError($"Failed to select any profile from: {FileIO.Folders.Profiles}");
            return;
        }

        var profile = profiles[index];
        if (profile == null || !profile.IsValid())
        {
            Debug.LogError($"Failed to select profile by index: {index}");
            return;
        }

        currentProfile = null;
        currentProfile = profile;
    }

    public bool HasProfiles()
    {
        return profiles != null && profiles.Count > 0;
    }

    private bool IsValidFolders()
    {
        //Verify profiles folder can be created
        if (string.IsNullOrWhiteSpace(FileIO.Folders.Profiles))
        {
            Debug.LogError($"FileIO.Folders.Profiles is null or whitespace.");
            return false;
        }

        //Create profiles folder (if applicable)
        if (!Directory.Exists(FileIO.Folders.Profiles))
            Directory.CreateDirectory(FileIO.Folders.Profiles);

        return Directory.Exists(FileIO.Folders.Profiles);
    }

}
