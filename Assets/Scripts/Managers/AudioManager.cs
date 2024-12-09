public class AudioManager : ExtendedMonoBehavior
{

    public void Play(string sfx)
    {
        var soundEffect = resourceManager.SoundEffect(sfx);
        if (soundEffect == null)
        {
            logManager.Error($@"Sound Effect ""{sfx}"" was not found.");
            return;
        }

        soundSource.PlayOneShot(soundEffect);
    }


}
