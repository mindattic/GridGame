using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Instances.Actor
{
    public class ActorMovement : ActorModule
    {
        public IEnumerator TowardCursor()
        {
            // Before:
            flags.IsMoving = true;
            instance.sortingOrder = SortingOrder.Max;

            Vector3 prevPosition = instance.position; // Store the initial position
            float tiltFactor = 25f; // How much tilt to apply based on movement
            float rotationSpeed = 10f; // Speed at which the tilt adjusts
            float resetSpeed = 5f; // Speed at which the rotation resets
            var baseRotation = Vector3.zero;

            // During:
            while (instance.IsFocusedPlayer || instance.IsSelectedPlayer)
            {
                instance.sortingOrder = SortingOrder.Max;

                var cursorPosition = gameManager.mousePosition3D + gameManager.mouseOffset;
                cursorPosition.x = Mathf.Clamp(cursorPosition.x, board.bounds.Left, board.bounds.Right);
                cursorPosition.y = Mathf.Clamp(cursorPosition.y, board.bounds.Bottom, board.bounds.Top);

                //Snap selected player to cursor
                instance.position = cursorPosition;

                //Calculate velocity
                Vector3 velocity = instance.position - prevPosition;

                //Apply tilt effect
                instance.ApplyTilt(velocity, tiltFactor, rotationSpeed, resetSpeed, baseRotation);

                // Update previous position for next frame
                prevPosition = instance.position;

                CheckLocationChanged();

                instance.destination = instance.position;

                yield return Wait.None();
            }

            // After:
            flags.IsMoving = false;

            //TODO: Reset to above overlay if is attacking...
            //sortingOrder = SortingOrder.Default;

            // Reset rotation at the end
            instance.transform.localRotation = Quaternion.Euler(baseRotation);
        }

        public IEnumerator TowardDestination()
        {
            // Before:
            Vector3 initialPosition = instance.position;
            Vector3 initialScale = tileScale;
            scale = tileScale;
            audioManager.Play($"Slide");
            instance.sortingOrder = SortingOrder.Moving;

            // During:
            while (!instance.HasReachedDestination)
            {
                sortingOrder = SortingOrder.Moving;

                var delta = instance.destination - instance.position;
                if (Mathf.Abs(delta.x) > gameManager.snapDistance)
                {
                    position = Vector2.MoveTowards(position, new Vector3(destination.x, position.y, position.z), moveSpeed);

                    // Snap horizontal boardPosition (if applicable)
                    if (Mathf.Abs(delta.x) <= snapDistance)
                        position = new Vector3(destination.x, position.y, position.z);
                }
                else if (Mathf.Abs(delta.y) > snapDistance)
                {
                    position = Vector2.MoveTowards(position, new Vector3(position.x, destination.y, position.z), moveSpeed);

                    // Snap vertical boardPosition (if applicable)
                    if (Mathf.Abs(delta.y) <= snapDistance)
                        position = new Vector3(position.x, destination.y, position.z);
                }

                if (flags.IsSwapping)
                {
                    //Calculate velocity
                    Vector3 velocity = destination - position;

                    //Apply tilt effect
                    instance.ApplyTilt(velocity, 25f, 10f, 5f, Vector3.zero);
                }

                CheckLocationChanged();

                //Determine whether to snap to destination
                bool isSnapDistance = Vector2.Distance(position, destination) <= snapDistance;
                if (isSnapDistance)
                    position = destination;

                yield return Wait.OneTick();
            }

            //After:
            flags.IsMoving = false;
            flags.IsSwapping = false;
            scale = tileScale;
            rotation = Quaternion.identity; //Reset rotation to default

            //TODO: Reset to above overlay if is attacking or defending...
            //sortingOrder = SortingOrder.Default;

            //TODO: Add enemy attacking here so that enemy attacks once they reach their intended destination...


        }

        public void CheckLocationChanged()
        {
            //Check if currentFps actor is closer to another tile (i.e.: it has moved)
            var closestTile = Geometry.GetClosestTile(position);
            if (location == closestTile.location)
                return;

            previousLocation = location;


            CheckActorOverlapping(closestTile);

            //Assign actor's location to closest tile location
            location = closestTile.location;
        }

        public void CheckActorOverlapping(TileInstance closestTile)
        {
            var overlappingActor = FindOverlappingActor(closestTile);
            if (overlappingActor == null)
                return;

            overlappingActor.location = location;
            overlappingActor.destination = Geometry.GetPositionByLocation(overlappingActor.location);
            overlappingActor.flags.IsMoving = true;
            overlappingActor.flags.IsSwapping = true;

            if (IsActive && IsAlive)
                instance.StartCoroutine(overlappingActor.move.TowardDestination());
        }


        ActorInstance FindOverlappingActor(TileInstance closestTile)
        {
            //Determine if two actors are overlapping the same boardLocation
            var overlappingActor = actors.FirstOrDefault(x => x != null
                                                && !x.Equals(this)
                                                && x.IsActive
                                                && x.IsAlive
                                                && !x.Equals(focusedActor)
                                                && !x.Equals(selectedPlayer)
                                                && x.location.Equals(closestTile.location));

            return overlappingActor;
        }


    }
}