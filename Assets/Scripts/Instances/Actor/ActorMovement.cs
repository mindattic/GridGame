﻿using Assets.Scripts.Behaviors.Actor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Instances.Actor
{
    public class ActorMovement
    {
        #region Properties
        protected float percent33 => Constants.percent33;
        protected Vector3 tileScale => GameManager.instance.tileScale;
        protected ActorInstance focusedActor => GameManager.instance.focusedActor;
        protected ActorInstance selectedPlayer => GameManager.instance.selectedPlayer;
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
        #endregion

        private ActorInstance instance;

        public void Initialize(ActorInstance parentInstance)
        {
            this.instance = parentInstance;
        }

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
                instance.ApplyTilt(velocity, tiltFactor, rotationSpeed, resetSpeed, baseRotation);

                // Update previous position for next frame
                prevPosition = instance.position;

                CheckLocationChanged();

                instance.destination = instance.position;

                yield return Wait.None();
            }

            // After:
            flags.IsMoving = false;

            //TODO: TriggerReset to above overlay if is attacking...
            //sortingOrder = SortingOrder.Default;

            // TriggerReset rotation at the end
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
            while (!instance.hasReachedDestination)
            {
                sortingOrder = SortingOrder.Moving;

                var delta = instance.destination - instance.position;
                if (Mathf.Abs(delta.x) > snapDistance)
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
            rotation = Quaternion.identity; //TriggerReset rotation to default

            //TODO: TriggerReset to above overlay if is attacking or defending...
            //sortingOrder = SortingOrder.Default;

            //TODO: Add enemy attacking here so that enemy attacks once they reach their intended destination...


        }

        private void CheckLocationChanged()
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

        private void CheckActorOverlapping(TileInstance closestTile)
        {
            var overlappingActor = FindOverlappingActor(closestTile);
            if (overlappingActor == null)
                return;

            overlappingActor.location = location;
            overlappingActor.destination = Geometry.GetPositionByLocation(overlappingActor.location);
            overlappingActor.flags.IsMoving = true;
            overlappingActor.flags.IsSwapping = true;

            if (isActive && isAlive)
                instance.StartCoroutine(overlappingActor.move.TowardDestination());
        }


        private ActorInstance FindOverlappingActor(TileInstance closestTile)
        {
            //Determine if two actors are overlapping the same boardLocation
            var overlappingActor = actors.FirstOrDefault(x => x != null
                                                && !x.Equals(this)
                                                && x.isActive
                                                && x.isAlive
                                                && !x.Equals(focusedActor)
                                                && !x.Equals(selectedPlayer)
                                                && x.location.Equals(closestTile.location));

            return overlappingActor;
        }


    }
}