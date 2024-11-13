using System;
using System.Collections.Generic;

namespace Game.Models
{
    public class SaveFile
    {
        public SaveFile() { }

        public SaveFile(string name)
        {
            Guid = Guid.NewGuid();
            Name = name;
            CurrentLevel = 1;
            TotalCoins = 0;
        }

        public Guid Guid { get; set; }
        public string Name { get; set; }
        public int CurrentLevel { get; set; }
        public int TotalCoins { get; set; }

        public List<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();



        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Guid.ToString())) return false;
            if (string.IsNullOrWhiteSpace(Name)) return false;

            return true;
        }
    }

    public class TeamMember
    {
        public string Name { get; set; }
        public Archetype Archetype { get; set; }
        public int SquadIndex { get; set; } = -1;
        public ActorStats Stats { get; set; }
        //public ActorEquipment Equipment { get; set; }
    }

}
