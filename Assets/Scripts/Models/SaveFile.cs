using System;
using System.Collections.Generic;

namespace Game.Models
{
    [Serializable]
    public class SaveFile
    {
        public SaveFile() { }

        public SaveFile(string guid)
        {
            Guid = guid;
            CurrentStage = 5; //DEBUG: Might need to be changed to a string to support branching paths... for now default to 5
            TotalCoins = 0;
        }

        public string Guid; 
        public int CurrentStage;
        public int TotalCoins;
        public string FilePath;

        public List<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Guid))
                return false;

            return true;
        }

        public bool HasFilePath => !string.IsNullOrWhiteSpace(FilePath);

    }

    public class TeamMember
    {
        public string Name { get; set; }
        public Archetype Archetype { get; set; }
        public int PartyIndex { get; set; } = -1;
        public ActorStats Stats { get; set; }
        //public ActorEquipment Equipment { get; set; }

        public bool IsInParty => PartyIndex != -1;
    }

}
