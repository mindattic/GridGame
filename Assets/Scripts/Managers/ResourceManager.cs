using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class ResourceManager : ExtendedMonoBehavior
{

    //Variables
    [SerializeField] public List<ActorSprite> actorSprites = new List<ActorSprite>();
    [SerializeField] public List<ActorMaterial> actorMaterials = new List<ActorMaterial>();
    [SerializeField] public List<ActorDetails> actorDetails = new List<ActorDetails>();
    [SerializeField] public List<StatusSprite> statusSprites = new List<StatusSprite>();
    [SerializeField] public List<PropSprite> propSprites = new List<PropSprite>();
    [SerializeField] public List<SeamlessSprite> seamLessSprites = new List<SeamlessSprite>();
    [SerializeField] public List<SoundEffect> soundEffects = new List<SoundEffect>();
    [SerializeField] public List<MusicTrack> musicTracks = new List<MusicTrack>();
    [SerializeField] public List<VisualEffect> visualEffects = new List<VisualEffect>();


    public Sprite ActorThumbnail(string id)
    {
        try
        {
            return actorSprites.First(x => x.id.Equals(id)).thumbnail;
        }
        catch (Exception ex)
        {
            logManager.error(ex.Message);
        }

        return null;
    }


    public Material ActorMaterial(string id)
    {
        try
        {
            return actorMaterials.First(x => x.id.Equals(id)).material;
        }
        catch (Exception ex)
        {
            logManager.error(ex.Message);
        }

        return null;
    }

    public Sprite ActorPortrait(string id)
    {
        try
        {
            return actorSprites.First(x => x.id.Equals(id)).portrait;
        }
        catch (Exception ex)
        {
            logManager.error(ex.Message);
        }

        return null;
    }

    public string ActorDetails(string id)
    {
        try
        {
            var details = actorDetails.FirstOrDefault(x => x.id.Equals(id))?.details ?? null;
            if (details != null)
                return details;
        }
        catch (Exception ex)
        {
            logManager.error(ex.Message);
        }

        return null;
    }

    public Sprite Prop(string id)
    {
        try
        {
            return propSprites.First(x => x.id.Equals(id)).sprite;
        }
        catch (Exception ex)
        {
            logManager.error(ex.Message);
        }

        return null;
    }


    public Sprite Seamless(string id)
    {
        try
        {
            return seamLessSprites.First(x => x.id.Equals(id)).sprite;
        }
        catch (Exception ex)
        {
            logManager.error(ex.Message);
        }

        return null;
    }


    public Sprite StatusThumbnail(string id)
    {
        try
        {
            return statusSprites.First(x => x.id.Equals(id)).thumbnail;
        }
        catch (Exception ex)
        {
            logManager.error(ex.Message);
        }

        return null;
    }


    public AudioClip SoundEffect(string id)
    {
        try
        {
            return soundEffects.First(x => x.id.Equals(id)).audio;
        }
        catch (Exception ex)
        {
            logManager.error(ex.Message);
        }

        return null;
    }


    public AudioClip MusicTrack(string id)
    {
        try
        {
            return musicTracks.First(x => x.id.Equals(id)).audio;
        }
        catch (Exception ex)
        {
            logManager.error(ex.Message);
        }

        return null;
    }


    public VisualEffect VisualEffect(string id)
    {
        try
        {
            return visualEffects.First(x => x.id.Equals(id));
        }
        catch (Exception ex)
        {
            logManager.error(ex.Message);
        }

        return null;
    }

}
