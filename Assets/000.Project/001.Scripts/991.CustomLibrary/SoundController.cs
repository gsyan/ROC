using UnityEngine;

public enum SoundType
{
    Master,
    Music,
    Fx,
}

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    public SoundType type;
    private AudioSource[] _source;

    private void Awake()
    {
        _source = GetComponents<AudioSource>();
    }

    private void Start()
    {
        for(int i = 0; i< _source.Length; ++i)
        {
            _source[i].outputAudioMixerGroup = GameSettings.Instance.GetAudioMixerGroup(type.ToString().ToLower());
        }
    }


}
