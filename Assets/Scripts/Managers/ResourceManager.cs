using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;



public class ResourceManager : ExtendedMonoBehavior
{

    [SerializeField] public List<ActorSprite> actorSprites = new List<ActorSprite>();

    [SerializeField] public List<StatusSprite> statusSprites = new List<StatusSprite>();



    [SerializeField] public List<SoundEffect> soundEffects = new List<SoundEffect>();

    [SerializeField] public List<MusicTrack> musicTracks = new List<MusicTrack>();


    private void Awake()
    {
    }

    public Sprite ActorThumbnail(string id)
    {
        return actorSprites.First(x => x.id.Equals(id)).thumbnail;
    }

    public Sprite ActorPortrait(string id)
    {
        return actorSprites.First(x => x.id.Equals(id)).portrait;
    }


    public Sprite StatusThumbnail(string id)
    {
        return statusSprites.First(x => x.id.Equals(id)).thumbnail;
    }


    public AudioClip SoundEffect(string id)
    {
        return soundEffects.First(x => x.id.Equals(id)).audio;
    }


    public AudioClip MusicTrack(string id)
    {
        return musicTracks.First(x => x.id.Equals(id)).audio;
    }


}
