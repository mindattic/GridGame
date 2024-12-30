using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Instances.Actor
{
    public class ActorActions : ActorModule
    {
        public void Shake(float intensity, float duration = 0)
        {
            if (IsActive && IsAlive)
                instance.StartCoroutine(_Shake(intensity, duration));
        }

        private IEnumerator _Shake(float intensity, float duration = 0)
        {
            // Ensure initial intensity is valid
            if (intensity <= 0)
                yield break;

            // Store the original thumbnail position
            var originalPosition = currentTile.position;
            var elapsedTime = 0f;

            while (intensity > 0 && (duration <= 0 || elapsedTime < duration))
            {
                // Calculate a random offset based on intensity
                var shakeOffset = new Vector3(
                    Random.Float(-intensity, intensity),
                    Random.Float(-intensity, intensity),
                    0 // Keep the z-axis stable
                );

                // Apply the offset to the thumbnail position
                thumbnailPosition = originalPosition + shakeOffset;

                // Wait for the next frame
                yield return Wait.OneTick();

                // Fill elapsed time if duration is specified
                if (duration > 0)
                    elapsedTime += Interval.OneTick;
            }

            // Reset to the original position after shaking is stopped
            thumbnailPosition = originalPosition;
        }



        public void Dodge()
        {
            if (IsActive && IsAlive)
                instance.StartCoroutine(_Dodge());
        }

        public IEnumerator _Dodge()
        {
            // Initial setup
            var rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            var scaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 0.9f);
            float duration = 0.125f; // Total duration for the forward twist
            float returnDuration = 0.125f; // Duration for the return to starting state
            var startRotation = Vector3.zero;
            var targetRotation = new Vector3(15f, 70f, 15f);
            var randomDirection = new Vector3(
               Random.Boolean ? -1f : 1f,
               Random.Boolean ? -1f : 1f,
               Random.Boolean ? -1f : 1f);

            float elapsedTime = 0f;

            // Twist forward
            while (elapsedTime < duration)
            {
                // Normalize time
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / duration);

                // Evaluate rotation and scale using AnimationCurves
                float curveValue = rotationCurve.Evaluate(progress);
                Vector3 currentRotation = Vector3.LerpUnclamped(startRotation, targetRotation, curveValue);
                currentRotation.Scale(randomDirection); // Apply random twist direction

                float scaleFactor = scaleCurve.Evaluate(progress);
                scale = tileScale * scaleFactor;

                // Apply calculated transformations
                rotation = Geometry.Rotation(currentRotation);

                yield return Wait.OneTick();
            }

            // Reset transition
            elapsedTime = 0f; // Reset time
            while (elapsedTime < returnDuration)
            {
                // Normalize time
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / returnDuration);

                // Reverse evaluate rotation and scale using AnimationCurves
                float curveValue = rotationCurve.Evaluate(progress);
                Vector3 currentRotation = Vector3.LerpUnclamped(targetRotation, startRotation, curveValue);
                currentRotation.Scale(randomDirection); // Apply reverse direction

                float scaleFactor = Mathf.LerpUnclamped(0.9f, 1f, progress);
                scale = tileScale * scaleFactor;

                // Apply calculated transformations
                rotation = Geometry.Rotation(currentRotation);

                yield return Wait.OneTick();
            }

            // Ensure exact reset
            scale = tileScale;
            rotation = Geometry.Rotation(Vector3.zero);
        }

        public void Bump(Direction direction)
        {
            if (IsActive && IsAlive)
                instance.StartCoroutine(_Bump(direction));
        }

        public IEnumerator _Bump(Direction direction, IEnumerator triggeredEvent = null)
        {
            // Animation curves for each phase
            var windupCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1); // Windup easing
            var bumpCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1); // Fast movement
            var returnCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);   // Smooth return

            // Durations for each phase
            var windupDuration = 0.15f;
            var bumpDuration = 0.1f;
            var returnDuration = 0.3f;

            // Positions
            var startPosition = currentTile.position;
            var windupPosition = Geometry.GetDirectionalPosition(startPosition, direction.Opposite(), tileSize * percent33);
            var bumpPosition = Geometry.GetDirectionalPosition(startPosition, direction, tileSize * percent33);

            // Increase sorting order to ensure this tile is on top
            sortingOrder = SortingOrder.Max;

            // Phase 1: Windup (move slightly in the opposite direction)
            float elapsedTime = 0f;
            while (elapsedTime < windupDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / windupDuration);
                float curveValue = windupCurve.Evaluate(progress);

                position = Vector3.Lerp(startPosition, windupPosition, curveValue);
                yield return Wait.OneTick();
            }

            // Phase 2: _Bump (quickly move in the direction and rotate slightly)
            elapsedTime = 0f;
            float targetRotationZ = (direction == Direction.East) ? -15f : 15f; // Opposite rotation for East
            while (elapsedTime < bumpDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / bumpDuration);
                float curveValue = bumpCurve.Evaluate(progress);

                position = Vector3.Lerp(windupPosition, bumpPosition, curveValue);
                rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, targetRotationZ, progress));
                yield return Wait.OneTick();
            }

            if (triggeredEvent != null && IsActive && IsAlive)
                instance.StartCoroutine(triggeredEvent);

            // Phase 3: Return to Starting Position (rotate back to zero and move back slowly)
            elapsedTime = 0f;
            while (elapsedTime < returnDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / returnDuration);
                float curveValue = returnCurve.Evaluate(progress);

                position = Vector3.Lerp(bumpPosition, startPosition, curveValue);
                rotation = Quaternion.Euler(0, 0, Mathf.Lerp(targetRotationZ, 0, progress));
                yield return Wait.OneTick();
            }

            // Reset sorting order and position
            sortingOrder = SortingOrder.Default;
            position = startPosition;
            rotation = Quaternion.identity;
        }


        public void Grow(float maxSize = 0f)
        {
            if (maxSize == 0)
                maxSize = tileSize * 1.1f;

            if (IsActive && IsAlive)
                instance.StartCoroutine(_Grow(maxSize));
        }

        private IEnumerator _Grow(float maxSize = 0f)
        {
            //Before:
            if (maxSize == 0)
                maxSize = tileSize * 1.1f;
            sortingOrder = SortingOrder.Attacker;
            float minSize = scale.x;
            float increment = tileSize * 0.01f;
            float size = minSize;
            scale = new Vector3(size, size, 0);

            //During:
            while (size < maxSize)
            {
                size += increment;
                size = Mathf.Clamp(size, minSize, maxSize);
                scale = new Vector3(size, size, 0);
                yield return Wait.OneTick();
            }

            //After:
            scale = new Vector3(maxSize, maxSize, 0);
        }

        public void Shrink(float minSize = 0f)
        {
            if (minSize == 0)
                minSize = tileSize;

            if (IsActive && IsAlive)
                instance.StartCoroutine(_Shrink(minSize));
        }

        private IEnumerator _Shrink(float minSize = 0f)
        {
            //Before:
            if (minSize == 0)
                minSize = tileSize;
            float maxSize = scale.x;
            float increment = tileSize * 0.01f;
            float size = maxSize;
            scale = new Vector3(size, size, 0);

            //During:
            while (size > minSize)
            {
                size -= increment;
                size = Mathf.Clamp(size, minSize, maxSize);
                scale = new Vector3(size, size, 0);
                yield return Wait.OneTick();
            }

            //After:
            scale = new Vector3(minSize, minSize, 0);
            sortingOrder = SortingOrder.Default;

        }

        public void Spin90(IEnumerator triggeredEvent = null)
        {
            if (IsActive && IsAlive)
                instance.StartCoroutine(_Spin90(triggeredEvent));
        }

        private IEnumerator _Spin90(IEnumerator triggeredEvent = null)
        {
            //Before:
            bool isDone = false;
            bool hasTriggered = false;
            var rotY = 0f;
            var spinSpeed = tileSize * 24f;
            rotation = Geometry.Rotation(0, rotY, 0);

            //During:
            while (!isDone)
            {
                rotY += !hasTriggered ? spinSpeed : -spinSpeed;

                if (!hasTriggered && rotY >= 90f)
                {
                    rotY = 90f;
                    hasTriggered = true;

                    if (triggeredEvent != null && IsActive && IsAlive)
                        instance.StartCoroutine(triggeredEvent);
                }

                isDone = hasTriggered && rotY <= 0f;
                if (isDone)
                    rotY = 0f;

                rotation = Geometry.Rotation(0, rotY, 0);
                yield return Wait.OneTick();
            }

            //After:
            rotation = Geometry.Rotation(0, 0, 0);

        }

        public void ExecuteSpin360(IEnumerator triggeredEvent = null)
        {
            if (IsActive && IsAlive)
                instance.StartCoroutine(_Spin360(triggeredEvent));
        }

        private IEnumerator _Spin360(IEnumerator triggeredEvent = null)
        {
            //Before:
            bool isDone = false;
            bool hasTriggered = false;
            var rotY = 0f;
            var speed = tileSize * 24f;
            rotation = Geometry.Rotation(0, rotY, 0);

            //During:
            while (!isDone)
            {
                rotY += speed;
                rotation = Geometry.Rotation(0, rotY, 0);

                //Trigger event and startDelay for it to finish (if applicable)
                if (!hasTriggered && rotY >= 240f)
                {
                    hasTriggered = true;

                    if (triggeredEvent != null && IsActive && IsAlive)
                        yield return triggeredEvent;
                }

                isDone = rotY >= 360f;
                yield return Wait.OneTick();
            }

            //After:
            rotation = Geometry.Rotation(0, 0, 0);
        }

        public void ExecuteFadeIn(float delay = 0f, float increment = 0.05f)
        {
            if (IsActive && IsAlive)
                instance.StartCoroutine(FadeIn(delay, increment));
        }

        private IEnumerator FadeIn(float delay = 0f, float increment = 0.05f)
        {
            //Before:
            float alpha = 0;
            render.SetAlpha(alpha);
            yield return Wait.For(delay);

            //During:
            while (alpha < 1)
            {
                alpha += increment;
                alpha = Mathf.Clamp(alpha, 0, 1);
                render.SetAlpha(alpha);
                yield return Wait.OneTick();
            }

            //After:
            alpha = 1;
            render.SetAlpha(alpha);
        }


        public void ExecuteWeaponWiggle()
        {
            if (stats.AP < stats.MaxAP)
                return;

            if (IsActive && IsAlive)
                Spin90(WeaponWiggle());
        }

        private IEnumerator WeaponWiggle()
        {
            if (stats.AP < stats.MaxAP)
                yield break;

            //Before:
            float start = -45f;
            float rotZ = start;
            render.weaponIcon.transform.rotation = Quaternion.Euler(0, 0, rotZ);

            // During:
            while (instance.stats.AP == instance.stats.MaxAP)
            {
                // Calculate rotation angle using sine wave
                rotZ = start + Mathf.Sin(Time.time * instance.wiggleSpeed) * instance.wiggleAmplitude;

                // Apply the rotation
                render.weaponIcon.transform.rotation = Quaternion.Euler(0, 0, rotZ);

                yield return Wait.OneTick();
            }

            // After:
            rotZ = start;
            render.weaponIcon.transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        public void ExecuteTurnDelayWiggle(bool isLooping = false)
        {
            if (IsActive && IsAlive)
                instance.StartCoroutine(TurnDelayWiggle(isLooping));
        }

        private IEnumerator TurnDelayWiggle(bool isLooping = false)
        {
            float timeElapsed = 0f;
            float amplitude = 10f;
            float speed = instance.wiggleSpeed; // Wiggle spinSpeed
            float dampingRate = 0.99f; // Factor to reduce amplitude each cycle (closer to 1 = slower decay)
            float cutoff = 0.1f;

            render.turnDelayText.transform.rotation = Quaternion.Euler(0, 0, 0);

            while (amplitude > cutoff)
            {
                timeElapsed += Time.deltaTime;
                float rotZ = Mathf.Sin(timeElapsed * speed) * amplitude;
                render.turnDelayText.transform.rotation = Quaternion.Euler(0, 0, rotZ);
                amplitude *= dampingRate;

                yield return Wait.OneTick();
            }

            // Smoothly return to zero rotation after finishing
            float currentZ = render.turnDelayText.transform.rotation.eulerAngles.z;
            while (Mathf.Abs(Mathf.DeltaAngle(currentZ, 0f)) > cutoff)
            {
                timeElapsed += Time.deltaTime * speed;
                currentZ = Mathf.LerpAngle(currentZ, 0f, timeElapsed);
                render.turnDelayText.transform.rotation = Quaternion.Euler(0, 0, currentZ);
                yield return Wait.OneTick();
            }

            // Ensure rotation is exactly zero at the end
            render.turnDelayText.transform.rotation = Quaternion.Euler(0, 0, 0);

        }



    }
}
