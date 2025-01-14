using Assets.Scripts.Models;
using Assets.Scripts.Utilities;
using Game.Behaviors;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region Properties
    protected TurnManager turnManager => GameManager.instance.turnManager;
    protected AttackLineManager attackLineManager => GameManager.instance.attackLineManager;
    protected AudioManager audioManager => GameManager.instance.audioManager;
    protected BoardOverlay boardOverlay => GameManager.instance.boardOverlay;
    protected PortraitManager portraitManager => GameManager.instance.portraitManager;
    protected SupportLineManager supportLineManager => GameManager.instance.supportLineManager;
    protected TimerBarInstance timerBar => GameManager.instance.timerBar;
    protected List<ActorInstance> actors { get => GameManager.instance.actors; set => GameManager.instance.actors = value; }
    protected IQueryable<ActorInstance> enemies => GameManager.instance.enemies;
    protected IQueryable<ActorInstance> players => GameManager.instance.players;
    protected ActorInstance previousSelectedPlayer  => GameManager.instance.previousSelectedPlayer;
    #endregion

    private CombatParticipants participants = new CombatParticipants();

    public void Clear() => participants.Clear();

    #region Combat Flow

    public void TriggerCombat()
    {
        participants.Clear();

        if (!HandleAlignedActors(players))
        {
            turnManager.NextTurn();
            return;
        }

        if (!AssignParticipants())
        {
            turnManager.NextTurn();
            return;
        }

        StartCoroutine(Combat());
    }

    private IEnumerator Combat()
    {
        SetupCombatState();

        foreach (var pair in participants.attackingPairs)
        {
            attackLineManager.Spawn(pair);
            yield return ResolveAttack(pair);
            ResetRolesAfterCombat(pair);
        }

        CleanupCombatState();
        turnManager.NextTurn();
    }

    #endregion

    #region Combat Logic

    private bool HandleAlignedActors(IQueryable<ActorInstance> actors)
    {
        foreach (var actor1 in actors)
        {
            foreach (var actor2 in actors)
            {
                if (IsValidAlignment(actor1, actor2) && !participants.HasAlignedPair(actor1, actor2))
                {
                    var pair = CreateAlignmentPair(actor1, actor2);
                    participants.alignedPairs.Add(pair);
                }
            }
        }
        return participants.alignedPairs.Any();
    }

    private bool AssignParticipants()
    {
        AssignAttackingPairs();
        if (!participants.attackingPairs.Any()) return false;

        AssignSupportingPairs();
        return true;
    }

    private IEnumerator ResolveAttack(ActorPair pair)
    {
        yield return portraitManager.Play(pair);

        var attacks = CalculateAttackResults(pair);
        yield return PerformAttacks(pair, attacks);

        CleanupLines(pair);
        yield return CleanupDefeatedEnemies(attacks);
    }

    private void SetupCombatState()
    {
        SortActorsToDefault();
        SortParticipantsAboveBoardOverlay();
        boardOverlay.TriggerFadeIn();
        SpawnSupportLines();
    }

    private void CleanupCombatState()
    {
        boardOverlay.TriggerFadeOut();
        ClearCombatParticipants();
    }

    #endregion

    #region Helper Methods

    private bool IsValidAlignment(ActorInstance actor1, ActorInstance actor2)
    {
        return actor1 != null && actor2 != null && actor1 != actor2 &&
               actor1.isActive && actor1.isAlive &&
               actor2.isActive && actor2.isAlive &&
               (actor1.IsSameColumn(actor2.location) || actor1.IsSameRow(actor2.location));
    }

    private ActorPair CreateAlignmentPair(ActorInstance actor1, ActorInstance actor2)
    {
        // Ensure actor1 is always positioned "before" actor2 based on left-to-right, top-to-bottom sorting
        if (actor1.location.y > actor2.location.y ||
            (actor1.location.y == actor2.location.y && actor1.location.x > actor2.location.x))
        {
            (actor1, actor2) = (actor2, actor1);
        }

        // Determine the axis for the alignment
        var axis = actor1.IsSameColumn(actor2.location) ? Axis.Vertical : Axis.Horizontal;

        return new ActorPair(actor1, actor2, axis);
    }


    private void AssignAttackingPairs()
    {
        foreach (var pair in participants.alignedPairs)
        {
            if (pair.alignment.hasOpponentsBetween &&
                !pair.alignment.hasAlliesBetween &&
                !pair.alignment.hasGapsBetween &&
                !participants.HasAttackingPair(pair))
            {
                participants.attackingPairs.Add(pair);
                pair.actor1.SetAttacking();
                pair.actor2.SetAttacking();
                pair.alignment.opponents.ForEach(opponent => opponent.SetDefending());
            }
        }
    }

    private void AssignSupportingPairs()
    {
        foreach (var pair in participants.alignedPairs)
        {
            if (!pair.alignment.hasOpponentsBetween &&
                !pair.alignment.hasAlliesBetween &&
                (pair.actor1.flags.IsAttacking || pair.actor2.flags.IsAttacking) &&
                !participants.HasSupportingPair(pair))
            {
                participants.supportingPairs.Add(pair);
                pair.actor1.SetSupporting();
                pair.actor2.SetSupporting();
            }
        }
    }



    private List<AttackResult> CalculateAttackResults(ActorPair pair)
    {
        return pair.alignment.opponents.Select(opponent =>
        {
            var isHit = Formulas.IsHit(pair.actor1, opponent);
            var damage = isHit ? Formulas.CalculateDamage(pair.actor1, opponent) : 0;
            return new AttackResult
            {
                Pair = pair,
                Opponent = opponent,
                IsHit = isHit,
                Damage = damage,
            };
        }).ToList();
    }

    private IEnumerator PerformAttacks(ActorPair pair, List<AttackResult> attacks)
    {
        yield return GrowAndShrink(pair.actor1, pair.actor2);

        foreach (var attack in attacks)
        {
            yield return PerformAttack(pair.actor1, attack);
        }
    }

    private IEnumerator GrowAndShrink(ActorInstance actor1, ActorInstance actor2)
    {
        yield return CoroutineHelper.WaitForAll(this, actor1.action.Grow(), actor2.action.Grow());
        yield return CoroutineHelper.WaitForAll(this, actor1.action.Shrink(), actor2.action.Shrink());
    }

    private IEnumerator PerformAttack(ActorInstance attacker, AttackResult attack)
    {
        var direction = attacker.GetDirectionTo(attack.Opponent);
        var trigger = new Trigger(attacker.Attack(attack));
        yield return attacker.action.Bump(direction, trigger);
    }

    private IEnumerator CleanupDefeatedEnemies(List<AttackResult> attacks)
    {
        var dyingEnemies = attacks.Where(x => x.Opponent.isDying).Select(x => x.Opponent).ToList();
        foreach (var enemy in dyingEnemies)
        {
            if (enemy == dyingEnemies.Last())
                yield return enemy.Die();
            else
                enemy.TriggerDie();

            yield return Wait.For(Interval.QuarterSecond);
        }
    }

    private void CleanupLines(ActorPair pair)
    {
        attackLineManager.Despawn(pair);
        supportLineManager.Despawn(pair);
    }

    private void ResetRolesAfterCombat(ActorPair pair)
    {
        ResetRoles(new[] { pair.actor1, pair.actor2 });
        ResetRoles(pair.alignment.opponents);
    }

    private void ResetRoles(params IEnumerable<ActorInstance>[] actorGroups)
    {
        foreach (var group in actorGroups)
        {
            foreach (var actor in group.Where(x => x.isActive && x.isAlive))
            {
                if (actor.attackingPairCount > 0 && --actor.attackingPairCount == 0)
                    actor.SetDefault();

                if (actor.supportingPairCount > 0 && --actor.supportingPairCount == 0)
                    actor.SetDefault();

                if (actor.flags.IsDefending)
                    actor.SetDefault();
            }
        }
    }

    private void SortActorsToDefault()
    {
        foreach (var actor in actors.Where(x => x.isActive && x.isAlive))
            actor.sortingOrder = SortingOrder.Default;
    }

    private void SortParticipantsAboveBoardOverlay()
    {
        foreach (var actor in participants.SelectAll())
            actor.sortingOrder = SortingOrder.BoardOverlay;
    }

    private void SpawnSupportLines()
    {
        foreach (var pair in participants.supportingPairs)
            supportLineManager.Spawn(pair);
    }

    private void ClearCombatParticipants()
    {
        foreach (var actor in participants.SelectAll())
        {
            actor.attackingPairCount = 0;
            actor.supportingPairCount = 0;
            actor.SetDefault();
        }
    }

    #endregion
}
