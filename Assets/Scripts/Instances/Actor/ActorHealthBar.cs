using Assets.Scripts.Instances.Actor;
using System.Collections;
using UnityEngine;

public class ActorHealthBar : ActorModule
{
    //Properties
    private Vector3 initialScale => render.healthBarBack.transform.localScale;

    private Vector3 GetScale(float value)
    {
        return new Vector3(
            Mathf.Clamp(initialScale.x * (value / stats.MaxHP), 0, initialScale.x),
            initialScale.y,
            initialScale.z);
    }

    public void Refresh()
    {
        render.healthBarDrain.transform.localScale = GetScale(stats.PreviousHP);
        render.healthBarFill.transform.localScale = GetScale(stats.HP);
        render.healthBarText.text = $@"{stats.HP}/{stats.MaxHP}";

        if (instance.IsActive && instance.IsAlive)
            instance.StartCoroutine(Drain());
    }

    private IEnumerator Drain()
    {
        //Check abort conditions
        if (stats.PreviousHP == stats.HP)
            yield break;

        //Before:
        Vector3 scale;

        //During:
        yield return Wait.For(Intermission.Before.HealthBar.Drain);

        while (stats.HP < stats.PreviousHP)
        {
            stats.PreviousHP -= Increment.HealthBar.Drain;
            scale = GetScale(stats.PreviousHP);
            render.healthBarDrain.transform.localScale = scale;
            yield return Wait.OneTick();
        }

        //After:
        stats.PreviousHP = stats.HP;
        scale = GetScale(stats.PreviousHP);
        render.healthBarDrain.transform.localScale = scale;
    }


}
