using System.Collections;
using UnityEngine;

public class ActorHealthBar : MonoBehaviour
{
    //Variables
    private ActorInstance instance;
    private Vector3 initialScale;

    //Properties
    private ActorRenderers renderers => this.instance.render;
    private ActorStats stats => this.instance.stats;

    public void Initialize(ActorInstance parentInstance)
    {
        this.instance = parentInstance;
        initialScale = renderers.healthBarBack.transform.localScale;
    }

    private Vector3 GetScale(float value)
    {
        return new Vector3(
            Mathf.Clamp(initialScale.x * (value / stats.MaxHP), 0, initialScale.x),
            initialScale.y,
            initialScale.z);
    }

    public void Refresh()
    {
        renderers.healthBarDrain.transform.localScale = GetScale(stats.PreviousHP);
        renderers.healthBarFill.transform.localScale = GetScale(stats.HP);
        renderers.healthBarText.text = $@"{stats.HP}/{stats.MaxHP}";

        if (instance.IsActive && instance.IsAlive)
            StartCoroutine(Drain());
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
            renderers.healthBarDrain.transform.localScale = scale;
            yield return Wait.OneTick();
        }

        //After:
        stats.PreviousHP = stats.HP;
        scale = GetScale(stats.PreviousHP);
        renderers.healthBarDrain.transform.localScale = scale;
    }


}
