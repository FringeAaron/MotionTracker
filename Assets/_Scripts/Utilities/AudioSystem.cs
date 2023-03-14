using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : PersistentSingleton<AudioSystem> {
    public AudioSource musicSource;
    public AudioSource soundSource;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _soundMixerGroup;

    public static float MusicVolume { get; private set; }
    public static float SoundVolume { get; private set; }

    protected override void Awake() {
        base.Awake();
        MusicVolume = PlayerPrefs.GetFloat("Music Volume", 1f);
        SoundVolume = PlayerPrefs.GetFloat("Sound Volume", 1f);
    }

    private void Start() {        

        if (musicSource != null) {
            SetMusicSource(musicSource);
        }
        if (soundSource != null) {
            SetSoundSource(soundSource);
        }
        
        UpdateMixerVolume();
    }

    public void SetMusicSource(AudioSource source) {
        musicSource = source;
        musicSource.outputAudioMixerGroup = _musicMixerGroup;
    }
    public void SetSoundSource(AudioSource source) {
        soundSource = source;
        soundSource.outputAudioMixerGroup = _soundMixerGroup;
    }

    public void PlayMusic(AudioClip clip) {
        musicSource.clip = clip;
        musicSource.volume = .2f;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic() {
        musicSource.Stop();
    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1, float pitch = 1) {
        soundSource.transform.position = pos;
        PlaySound(clip, vol, pitch);
    }

    public void PlaySound(AudioClip clip, float vol = 1, float pitch = 1) {
        soundSource.dopplerLevel = pitch/10f;
        soundSource.pitch = pitch;
        soundSource.PlayOneShot(clip, vol);        
    }

    public IEnumerator PlaySound(AudioClip clip, Vector3 pos, float vol = 1, float pitch = 1, float delay = 0) {
        yield return new WaitForSecondsRealtime(delay);
        PlaySound(clip, pos, vol, pitch);
    }

    public void UpdateMixerVolume() {
        _musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(MusicVolume) * 40);
        _soundMixerGroup.audioMixer.SetFloat("Sound Volume", Mathf.Log10(SoundVolume) * 40);
    }

    public float GetMusicVolume() {
        return MusicVolume;
    }
    public float GetSoundVolume() {
        return SoundVolume;
    }
    public void SetMusicVolume(float value) {
        MusicVolume = value;
        PlayerPrefs.SetFloat("Music Volume", value);
        UpdateMixerVolume();
    }
    public void SetSoundVolume(float value) {
        SoundVolume = value;
        PlayerPrefs.SetFloat("Sound Volume", value);
        UpdateMixerVolume();
    }

    public bool MusicIsPlaying() {
        return musicSource.isPlaying;
    }
}