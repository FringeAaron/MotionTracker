using UnityEngine;

public class MuzzleFlash : MonoBehaviour {
    [SerializeField] private float _fps = 30.0f;
    [SerializeField] private Texture2D[] _frames;
    [SerializeField] private AudioClip[] _sounds;

    private int _frameIndex;
    private MeshRenderer _renderer;
    private AudioSource _audioSource;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
        _renderer = GetComponent<MeshRenderer>();
    }

    void Start() {
        NextFrame();
        InvokeRepeating(nameof(NextFrame), 1 / _fps, 1 / _fps);        
    }

    private void OnEnable() {
        PlaySound();
    }

    void NextFrame() {
        _renderer.sharedMaterial.mainTexture = _frames[_frameIndex];
        _frameIndex = (_frameIndex + 1) % _frames.Length;
    }

    void PlaySound() {
        var clip = _sounds[Random.Range(0, _sounds.Length)];
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void Update() {
        if (!_audioSource.isPlaying) {
            PlaySound();
        }
    }
}