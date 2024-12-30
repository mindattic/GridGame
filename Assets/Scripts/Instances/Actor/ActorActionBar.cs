using Assets.Scripts.Behaviors.Actor;
using System.Collections;
using UnityEngine;

public class ActorActionBar : ExtendedMonoBehavior
{
    //Variables
    private ActorInstance instance;
    private Vector3 initialScale;

    //Properties
    private ActorRenderers renderers => this.instance.render;
    private ActorStats stats => this.instance.stats;
    private ActorFlags flags => this.instance.flags;

    public void Initialize(ActorInstance parentInstance)
    {
        this.instance = parentInstance;
        initialScale = renderers.actionBarBack.transform.localScale;
    }

    private Vector3 GetScale(float value)
    {
        return new Vector3(
            Mathf.Clamp(initialScale.x * (value / stats.MaxAP), 0, initialScale.x),
            initialScale.y,
            initialScale.z);
    }

    public void Refresh()
    {
        renderers.actionBarDrain.transform.localScale = GetScale(stats.PreviousAP);
        renderers.actionBarFill.transform.localScale = GetScale(stats.AP);
        renderers.actionBarText.text = $@"{stats.AP}/{stats.MaxAP}";


        instance.action.ExecuteWeaponWiggle();

        if (instance.IsActive && instance.IsAlive)
            StartCoroutine(_Drain());
    }

    private IEnumerator _Drain()
    {
        //Check abort conditions
        if (stats.PreviousAP == stats.AP)
            yield break;

        //Before:
        Vector3 scale;

        //During:
        yield return Wait.For(Intermission.Before.ActionBar.Drain);

        while (stats.AP < stats.PreviousAP)
        {
            stats.PreviousAP -= Increment.ActionBar.Drain;
            scale = GetScale(stats.PreviousAP);
            renderers.actionBarDrain.transform.localScale = scale;
            yield return Wait.OneTick();
        }

        //After:
        stats.PreviousAP = stats.AP;
        scale = GetScale(stats.PreviousAP);
        renderers.healthBarDrain.transform.localScale = scale;
    }

    public void Fill()
    {
        if (instance.IsActive && instance.IsAlive)
            StartCoroutine(_Fill());
    }

    private IEnumerator _Fill()
    {
        //Check abort conditions
        if (debugManager.isEnemyStunned || !HasSelectedPlayer || !instance.IsEnemy || !instance.IsActive || !instance.IsAlive || instance.HasMaxAP || flags.isGainingAP)
            yield break;

        //Before:
        flags.isGainingAP = true;
        float amount = stats.Speed * 0.1f;

        //During:
        while (HasSelectedPlayer && instance.IsEnemy && instance.IsActive && instance.IsAlive && !instance.HasMaxAP)
        {
            stats.AP += amount;
            stats.AP = Mathf.Clamp(stats.AP, 0, stats.MaxAP);
            stats.PreviousAP = stats.AP;
            Refresh();
            yield return Wait.OneTick();
        }

        //After:
        stats.PreviousAP = stats.AP;
        Refresh();
        flags.isGainingAP = false;
    }

    public void Reset()
    {
        stats.AP = 0;
        stats.PreviousAP = 0;
        Refresh();
    }


    public void AddInitiative()
    {
        //TODO: Add randomization based on stats.Luck...
        float amount = stats.Speed * 0.01f;
        stats.AP = amount;
        stats.PreviousAP = amount;
        Refresh();
    }







}
