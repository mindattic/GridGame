using Assets.Scripts.Entities;
using SQLiteDatabase;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Collection
{
    public List<Enemy> Enemies = new List<Enemy>();
}



public class DatabaseManager : ExtendedMonoBehavior
{
    private SQLiteDB instance = SQLiteDB.Instance;
    public Collection db = new Collection();

    // Events
    void OnEnable()
    {
        SQLiteEventListener.onError += OnError;
    }

    void OnDisable()
    {
        SQLiteEventListener.onError -= OnError;
    }

    void OnError(string err)
    {
        logManager.error(err);
    }

    // use this to avoid any lock on database, otherwise restart editor or application after each run
    void OnApplicationQuit()
    {
        // release all resource using by database.
        instance.Dispose();
    }

    // Use this for initialization
    void Start()
    {
        // set database location (directory)
        instance.DBLocation = Application.persistentDataPath;
        instance.DBName = "MyDatabase.db";
        logManager.info("Database Directory : " + instance.DBLocation);

        if (instance.Exists)
        {
            Connect();
        }
        else
        {
            Setup();
        }
    }

    void Connect()
    {
        //instance.CreateDatabase (instance.DBName,false);

        instance.ConnectToDefaultDatabase(instance.DBName, loadFresh: true);
        Refresh();
    }

    void Setup()
    {
        bool success = instance.CreateDatabase(instance.DBName, isOverWrite: true); // Replace old

        //string sourcePath = Path.Combine(Application.streamingAssetsPath, "MyDatabase.db");
        //string destinationPath = Path.Combine(Application.persistentDataPath, "MyDatabase.db");

        //if (!File.Exists(destinationPath))
        //{
        //    File.Copy(sourcePath, destinationPath);
        //}
        if (success)
            Connect();
    }

    void Refresh()
    {
        db.Enemies.Clear(); // Clear the current list of enemies

        // Get all data from the Enemies table
        DBReader reader = instance.GetAllData("Enemies");

        while (reader != null && reader.Read())
        {
            // Create a new Enemy object and populate it with data from the reader
            Enemy enemy = new Enemy
            {
                Id = reader.GetIntValue("Id"),
                Name = reader.GetStringValue("Name"),
                Description = reader.GetStringValue("Description"),
                Level = reader.GetFloatValue("Level"),
                MaxHp = reader.GetFloatValue("MaxHp"),
                Strength = reader.GetFloatValue("Strength"),
                Vitality = reader.GetFloatValue("Vitality"),
                Agility = reader.GetFloatValue("Agility"),
                Speed = reader.GetFloatValue("Speed"),
                Luck = reader.GetFloatValue("Luck")
            };

            // Add the populated enemy to the list
            db.Enemies.Add(enemy);
        }


        foreach (var enemy in db.Enemies)
        {
            logManager.info(JsonUtility.ToJson(enemy));
        }



    }

}
