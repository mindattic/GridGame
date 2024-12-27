using Game.Behaviors;
using System.Collections;
using UnityEngine;

public class ActorActionBar : MonoBehaviour
{
    //Variables
    private ActorInstance instance;
    private Vector3 initialScale;
 

    //Properties
    private ActorRenderers renderers => this.instance.renderers;
    private ActorStats stats => this.instance.stats;

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
        StartCoroutine(Drain());
    }

    IEnumerator Drain()
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
            stats.PreviousAP -= Increment.ActionBarDrainAmount;
            scale = GetScale(stats.PreviousAP);
            renderers.actionBarDrain.transform.localScale = scale;
            yield return Wait.OneTick();
        }

        //After:
        stats.PreviousAP = stats.AP;
        scale = GetScale(stats.PreviousAP);
        renderers.healthBarDrain.transform.localScale = scale;
    }

}
