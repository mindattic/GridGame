using System.Linq;
using UnityEngine;

public class LineManager : ExtendedMonoBehavior
{
    private void Start()
    {
    
    }

    private void Update()
    {
        var northLine = lines[0];
        var eastLine = lines[1];
        var southLine = lines[2];
        var westLine = lines[3];

        northLine.Hide();
        eastLine.Hide();
        southLine.Hide();
        westLine.Hide();

        if (!HasSelectedPlayer)
            return;

        //TODO: Trigger "ShowLine" event so that this isn't calculated every frame...

        var sortedActors = actors.OrderBy(x => Vector3.Distance(selectedPlayer.position, x.position));

        var northActor = sortedActors
            .FirstOrDefault(x =>
            {
                return !selectedPlayer.Equals(x)
                && x.team.Equals(Team.Player)
                && selectedPlayer.IsSameColumn(x)
                && selectedPlayer.location.y > x.location.y
                && Geometry.IsInRange(selectedPlayer.position.x, selectedPlayer.currentTile.position.x, tileSize / 4);
            });
        if (northActor != null)
            northLine.Set(selectedPlayer.currentTile.position, northActor.currentTile.position);
     
        var eastActor = sortedActors
            .FirstOrDefault(x =>
            {
                return !selectedPlayer.Equals(x)
                && x.team.Equals(Team.Player)
                && selectedPlayer.IsSameRow(x)
                && selectedPlayer.location.x > x.location.x
                && Geometry.IsInRange(selectedPlayer.position.y, selectedPlayer.currentTile.position.y, tileSize / 4);
            });
        if (eastActor != null)
            eastLine.Set(selectedPlayer.currentTile.position, eastActor.currentTile.position);
   
        var southActor = sortedActors
            .FirstOrDefault(x =>
            {
                return !selectedPlayer.Equals(x)
                && x.team.Equals(Team.Player)
                && selectedPlayer.IsSameColumn(x)
                && selectedPlayer.location.y < x.location.y
                && Geometry.IsInRange(selectedPlayer.position.x, selectedPlayer.currentTile.position.x, tileSize / 5);
            });
        if (southActor != null)
            southLine.Set(selectedPlayer.currentTile.position, southActor.currentTile.position);

        var westActor = sortedActors
            .FirstOrDefault(x =>
            {
                return !selectedPlayer.Equals(x)
                && x.team.Equals(Team.Player)
                && selectedPlayer.IsSameRow(x)
                && selectedPlayer.location.x < x.location.x
                && Geometry.IsInRange(selectedPlayer.position.y, selectedPlayer.currentTile.position.y, tileSize / 4);
            });
        if (westActor != null)
            westLine.Set(selectedPlayer.currentTile.position, westActor.currentTile.position);
       
    }
}
