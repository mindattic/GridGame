using Assets.Scripts.Entities;
using SQLiteDatabase;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseEntities
{
    public List<Enemy> Enemies = new List<Enemy>();
}

public class DatabaseManager : ExtendedMonoBehavior
{
    private const string DATABASE_NAME = "MyDatabase.db";
    private SQLiteDB instance = SQLiteDB.Instance;
    public DatabaseEntities db = new DatabaseEntities();

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

    void OnApplicationQuit()
    {
        instance.Dispose();
    }

    private void Awake()
    {
        instance.DBLocation = Application.persistentDataPath;
        instance.DBName = DATABASE_NAME;

        if (!instance.Exists)
            instance.CreateDatabase(instance.DBName, isOverWrite: true);

        var isConnected = instance.ConnectToDefaultDatabase(instance.DBName, loadFresh: true);

        if (!isConnected)
            throw new UnityException($"Failed to connect to database: {instance.DBName}");

   
    }

    private void Start()
    {
        Load();
    }

    void Load()
    {
        DBReader reader;

        #region Load Enemies

        db.Enemies.Clear();
        reader = instance.GetAllData("Enemy");
        while (reader != null && reader.Read())
        {
            var enemy = new Enemy
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

            db.Enemies.Add(enemy);

            logManager.info(JsonUtility.ToJson(enemy));

            #endregion


        }
    }

}
