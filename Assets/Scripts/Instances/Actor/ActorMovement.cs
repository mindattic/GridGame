using Assets.Scripts.Behaviors.Actor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.Instances.Actor
{
    public class ActorMovement
    {
        protected float percent33 => Constants.percent33;
        protected Vector3 tileScale => GameManager.instance.tileScale;

        protected ActorInstance focusedActor => GameManager.instance.focusedActor;

        protected List<ActorInstance> actors => GameManager.instance.actors;
        protected AudioManager audioManager => GameManager.instance.audioManager;
        protected BoardInstance board => GameManager.instance.board;
        protected float moveSpeed => GameManager.instance.moveSpeed;
        protected float snapDistance => GameManager.instance.snapDistance;
        protected float tileSize => GameManager.instance.tileSize;
        protected Vector3 mousePosition3D => GameManager.instance.mousePosition3D;
        protected Vector3 mouseOffset => GameManager.instance.mouseOffset;
        protected ActorFlags flags => instance.flags;
        protected ActorRenderers render => instance.render;
        protected ActorStats stats => instance.stats;
        private bool isActive => instance.isActive;
        private bool isAlive => instance.isAlive;
        private int sortingOrder { get => instance.sortingOrder; set => instance.sortingOrder = value; }
        private Quaternion rotation { get => instance.rotation; set => instance.rotation = value; }
        protected Vector2Int previousLocation { get => instance.previousLocation; set => instance.previousLocation = value; }
        private Vector2Int location { get => instance.location; set => instance.location = value; }
        private Vector3 destination { get => instance.destination; set => instance.destination = value; }
        private Vector3 position { get => instance.position; set => instance.position = value; }
        private Vector3 scale { get => instance.scale; set => instance.scale = value; }

        protected ActorInstance selectedPlayer => GameManager.instance.selectedPlayer;
        protected bool hasSelectedPlayer => GameManager.instance.hasSelectedPlayer;
        protected bool isSelectedPlayer => hasSelectedPlayer && selectedPlayer == instance;
        protected UnityEvent<Vector2Int> onSelectedPlayerLocationChanged => GameManager.instance.onSelectedPlayerLocationChanged;

  
        private ActorInstance instance;

        
        public void Initialize(ActorInstance parentInstance)
        {
            this.instance = parentInstance;

        }

        public IEnumerator MoveTowardCursor()
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
            while (instance.isFocusedPlayer || instance.isSelectedPlayer)
            {
                instance.sortingOrder = SortingOrder.Max;

                var cursorPosition = mousePosition3D + mouseOffset;
                cursorPosition.x = Mathf.Clamp(cursorPosition.x, board.bounds.Left, board.bounds.Right);
                cursorPosition.y = Mathf.Clamp(cursorPosition.y, board.bounds.Bottom, board.bounds.Top);

                //Snap selected player to cursor
                instance.position = cursorPosition;

                //Calculate velocity
                Vector3 velocity = instance.position - prevPosition;

                //Apply tilt effect
                ApplyTilt(velocity, tiltFactor, rotationSpeed, resetSpeed, baseRotation);

                // Update previous position for next frame
                prevPosition = instance.position;

                CheckLocationChanged();

                instance.destination = instance.position;

                yield return Wait.UntilNextFrame();
            }

            // After:
            flags.IsMoving = false;

            //TODO: Initialize to above overlay if is attacking...
            //sortingOrder = SortingOrder.Default;

            // Initialize rotation at the end
            instance.transform.localRotation = Quaternion.Euler(baseRotation);
        }

        public IEnumerator MoveTowardDestination()
        {
            // Before:
            Vector3 initialPosition = instance.position;
            Vector3 initialScale = tileScale;
            scale = tileScale;
            audioManager.Play($"Slide");
            instance.sortingOrder = SortingOrder.Moving;

            // During:
            while (!instance.hasReachedDestination)
            {
                sortingOrder = SortingOrder.Moving;

                var delta = instance.destination - instance.position;
                if (Mathf.Abs(delta.x) > snapDistance)
                {
                    position = Vector2.MoveTowards(position, new Vector3(destination.x, position.y, position.z), moveSpeed);

                    // Snap horizontal boardPosition (if applicable)
                    if (Mathf.Abs(delta.x) <= snapDistance)
                    {
                        position = new Vector3(destination.x, position.y, position.z);
                        rotation = Geometry.Rotation(0, 0, 0);
                    }

                }
                else if (Mathf.Abs(delta.y) > snapDistance)
                {
                    position = Vector2.MoveTowards(position, new Vector3(position.x, destination.y, position.z), moveSpeed);

                    // Snap vertical boardPosition (if applicable)
                    if (Mathf.Abs(delta.y) <= snapDistance)
                    {
                        position = new Vector3(position.x, destination.y, position.z);
                        rotation = Geometry.Rotation(0, 0, 0);
                    }
                }

                if (flags.IsSwapping)
                {
                    //Calculate velocity
                    Vector3 velocity = destination - position;

                    //Apply tilt effect
                    ApplyTilt(velocity, 25f, 10f, 5f, Vector3.zero);
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
            rotation = Quaternion.identity; //Initialize rotation to default

            //TODO: Initialize to above overlay if is attacking or defending...
            //sortingOrder = SortingOrder.Default;

            //TODO: SpawnActor enemy attacking here so that enemy attacks once they reach their intended destination...


        }

        private void CheckLocationChanged()
        {
            //Check if currentFps actor is closer to another tile (i.e.: it has moved)
            var closestLocation = Geometry.GetClosestTile(position).location;

            if (location == closestLocation)
                return;

            previousLocation = location;

            CheckActorOverlapping(closestLocation);

            //Assign actor's location to closest tile location
            location = closestLocation;

            //if (isSelectedPlayer)
            //    onSelectedPlayerLocationChanged?.Invoke(location);
        }

        private void CheckActorOverlapping(Vector2Int closestLocation)
        {
            //Determine if two actors are overlapping the same boardLocation
            var overlappingActor = FindOverlappingActor(closestLocation);
            if (overlappingActor == null)
                return;

            overlappingActor.location = location;
            overlappingActor.destination = Geometry.GetPositionByLocation(overlappingActor.location);
            overlappingActor.flags.IsMoving = true;
            overlappingActor.flags.IsSwapping = true;

            Debug.Log($"[CheckActorOverlapping] Invoking event on {overlappingActor.name} from {instance.name}");

            // Raise the event so the overlapping actor can respond immediately
            overlappingActor.OnOverlappingActorDetected?.Invoke(instance);

            //if (isActive && isAlive)
            //    instance.StartCoroutine(overlappingActor.move.MoveTowardDestination());
        }

        private ActorInstance FindOverlappingActor(Vector2Int closestLocation)
        {
            //Determine if two actors are overlapping the same boardLocation
            var overlappingActor = actors.FirstOrDefault(x => x != null
                                                && x != instance
                                                && x != focusedActor
                                                && x != selectedPlayer
                                                && x.isActive
                                                && x.isAlive
                                                && x.location == closestLocation);

            return overlappingActor;
        }


        public void HandleOverlappingActor(ActorInstance other)
        {
            Debug.Log($"[HandleOverlappingActor] {instance.name} handling overlap with {other.name}");

            location = other.location;
            destination = Geometry.GetPositionByLocation(location);
            flags.IsMoving = true;
            flags.IsSwapping = true;

            if (isActive && isAlive)
                instance.StartCoroutine(this.MoveTowardDestination());
        }


        public void ApplyTilt(Vector3 velocity, float tiltFactor, float rotationSpeed, float resetSpeed, Vector3 baseRotation)
        {
            if (velocity.magnitude > 0.01f) //Apply tilt if there is noticeable movement
            {
                // Determine if the movement is primarily vertical or horizontal
                bool isMovingVertical = Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x);
                float velocityFactor = isMovingVertical ? velocity.y : velocity.x;
                float tiltZ = velocityFactor * tiltFactor; // Tilt on Z-axis based on velocity
                instance.transform.localRotation = Quaternion.Slerp(
                    instance.transform.localRotation,
                    Quaternion.Euler(0, 0, tiltZ),
                    Time.deltaTime * rotationSpeed
                );
            }
            else
            {
                //Initialize rotation smoothly when velocity is minimal
                instance.transform.localRotation = Quaternion.Slerp(
                    instance.transform.localRotation,
                    Quaternion.Euler(baseRotation),
                    Time.deltaTime * resetSpeed
                );
            }
        }





    }
}