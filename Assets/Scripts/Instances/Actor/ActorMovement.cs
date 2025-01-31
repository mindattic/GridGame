using Assets.Scripts.Behaviors.Actor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        private Vector2Int currentLocation { get => instance.currentLocation; set => instance.currentLocation = value; }
        private Vector2Int? nextLocation { get => instance.nextLocation; set => instance.nextLocation = value; }
        private Vector2Int? redirectedLocation { get => instance.redirectedLocation; set => instance.redirectedLocation = value; }
        //private Vector3? nextPosition { get => instance.nextPosition; set => instance.nextPosition = value; }



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

                //instance.nextPosition = instance.position;

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
            // Check abort conditions
            //if (!nextLocation.HasValue)
            //    yield break;

            // Assign nextPosition based on nextLocation
            var nextPosition = Geometry.GetPositionByLocation(nextLocation.Value);

            // Before movement begins
            Vector3 initialPosition = instance.position;
            Vector3 initialScale = tileScale;
            scale = tileScale;
            audioManager.Play("Slide");
            instance.sortingOrder = SortingOrder.Moving;

            // Moving toward nextLocation
            while (position != nextPosition)
            {
                sortingOrder = SortingOrder.Moving;

                var delta = nextPosition - instance.position;
                if (Mathf.Abs(delta.x) > snapDistance)
                {
                    var xLockedVector = new Vector3(nextPosition.x, position.y, position.z);
                    position = Vector2.MoveTowards(position, xLockedVector, moveSpeed);

                    if (Mathf.Abs(delta.x) <= snapDistance)
                    {
                        position = xLockedVector;
                        rotation = Geometry.Rotation(0, 0, 0);
                    }
                }
                else if (Mathf.Abs(delta.y) > snapDistance)
                {
                    var yLockedVector = new Vector3(position.x, nextPosition.y, position.z);
                    position = Vector2.MoveTowards(position, yLockedVector, moveSpeed);

                    if (Mathf.Abs(delta.y) <= snapDistance)
                    {
                        position = yLockedVector;
                        rotation = Geometry.Rotation(0, 0, 0);
                    }
                }

                if (flags.IsSwapping)
                {
                    // Calculate velocity
                    Vector3 velocity = nextPosition - position;
                    ApplyTilt(velocity, 25f, 10f, 5f, Vector3.zero);
                }

                CheckLocationChanged();

                // Determine whether to snap to nextPosition
                bool isSnapDistance = Vector2.Distance(position, nextPosition) <= snapDistance;
                if (isSnapDistance)
                    position = nextPosition;

                yield return Wait.OneTick();
            }

            // After reaching the nextPosition
            previousLocation = currentLocation; // Track previous currentLocation
            currentLocation = nextLocation.Value; // Assign new currentLocation
            nextLocation = null; // Reset nextLocation

            flags.IsMoving = false;
            flags.IsSwapping = false;
            scale = tileScale;
            rotation = Quaternion.identity;
        }

        private void CheckLocationChanged()
        {
            //Check if currentFps actor is closer to another tile (i.e.: it has moved)
            var closestLocation = Geometry.GetClosestTile(position).location;

            if (currentLocation == closestLocation)
                return;

            previousLocation = currentLocation;

            CheckActorOverlapping(closestLocation);

            //Assign actor's currentLocation to closest tile currentLocation
            currentLocation = closestLocation;

            //if (isSelectedPlayer)
            //    onSelectedPlayerLocationChanged?.Invoke(currentLocation);
        }


        private void CheckActorOverlapping(Vector2Int closestLocation)
        {
            //Determine if two actors are overlapping the same boardLocation
            var overlappingActor = FindOverlappingActor(closestLocation);
            if (overlappingActor == null)
                return;

            overlappingActor.currentLocation = currentLocation;
            //overlappingActor.nextPosition = Geometry.GetPositionByLocation(overlappingActor.currentLocation);
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
                                                && x.currentLocation == closestLocation);

            return overlappingActor;
        }


        public void HandleOverlappingActor(ActorInstance other)
        {
            Debug.Log($"[HandleOverlappingActor] {instance.name} handling overlap with {other.name}");

            nextLocation = other.currentLocation;
            //nextPosition = Geometry.GetPositionByLocation(nextLocation.Value);
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