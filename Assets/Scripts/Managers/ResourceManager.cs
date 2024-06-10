using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;



public class ResourceManager : ExtendedMonoBehavior
{

    //Variables
    [SerializeField] public List<ActorSprite> actorSprites = new List<ActorSprite>();
    [SerializeField] public List<ActorDetails> actorDetails = new List<ActorDetails>();
    [SerializeField] public List<StatusSprite> statusSprites = new List<StatusSprite>();
    [SerializeField] public List<PropSprite> propSprites = new List<PropSprite>();
    [SerializeField] public List<SeamlessSprite> seamLessSprites = new List<SeamlessSprite>();
    [SerializeField] public List<SoundEffect> soundEffects = new List<SoundEffect>();
    [SerializeField] public List<MusicTrack> musicTracks = new List<MusicTrack>();

    void Awake() { }
    void Start() { }
    void Update() { }
    void FixedUpdate() { }

    public Sprite ActorThumbnail(string id)
    {
        return actorSprites.First(x => x.id.Equals(id)).thumbnail;
    }

    public Sprite ActorPortrait(string id)
    {
        return actorSprites.First(x => x.id.Equals(id)).portrait;
    }

    public string ActorDetails(string id)
    {
        var details = actorDetails.FirstOrDefault(x => x.id.Equals(id))?.details ?? null;
        if (details != null)
            return details;

        return "";
    }

    public Sprite Prop(string id)
    {
        return propSprites.First(x => x.id.Equals(id)).sprite;
    }


    public Sprite Seamless(string id)
    {
        return seamLessSprites.First(x => x.id.Equals(id)).sprite;
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
