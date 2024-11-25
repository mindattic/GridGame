using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static FileIO;

namespace Game.Models
{
    [Serializable]
    public class Profile
    {
        public string Guid;
        public string Folder;

        public Global Global { get; set; }
        public Stage Stage { get; set; }
        public Party Party { get; set; }

        public Profile() { }

        public Profile(string guid)
        {
            Guid = guid;

            Folder = CreateFolder(Folders.Profiles, Guid);

            Global = new Global
            {
                TotalCoins = 0
            };

            Stage = new Stage
            {
                CurrentStage = 5 // DEBUG: Default to 5
            };

            Party = new Party();
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Guid) || string.IsNullOrWhiteSpace(Folder))
                return false;

            if (Global == null || !Global.IsValid() 
                || Stage == null || !Stage.IsValid()
                || Party == null || !Party.IsValid())
                return false;

            return true;
        }

    }


    [Serializable]
    public class ProfileComponent
    {
        public string FileName;
    }

    [Serializable]
    public class Global: ProfileComponent
    {
        public int TotalCoins;

        public Global()
        {
            FileName = "global.json";
            TotalCoins = 0;
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(FileName))
                return false;

            return true;
        }
    }

    [Serializable]
    public class Stage: ProfileComponent
    {
        public int CurrentStage;

        public Stage()
        {
            FileName = "stage.json";
            CurrentStage = 5; //DEBUG: Default to 5
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(FileName))
                return false;

            return true;
        }
    }

    [Serializable]
    public class Party: ProfileComponent
    {
        public List<Member> Members = new List<Member>();

        public Party()
        {
            FileName = "party.json";
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(FileName))
                return false;

            return true;
        }
    }


   



    [Serializable]
    public class Member
    {
        public string Name;
        public Character Character;
        public int Index = -1;
        public ActorStats Stats;
        //public ActorEquipment Equipment;

        public bool IsInParty => Index > 0;
    }
}
