using UnityEngine;

public class RadarPing : MonoBehaviour {

    [SerializeField] private Color _color;

    private SpriteRenderer _spriteRenderer;
    private float _disappearTimer;
    private float _disappearTimerMax;
    private AudioSource _audioSource;
    

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _disappearTimerMax = 1f;
        _disappearTimer = 0f;
        _color = new Color(1, 1, 1, 1f);
    }

    private void Update() {
        _disappearTimer += Time.deltaTime;

        _color.a = Mathf.Lerp(_disappearTimerMax, 0f, _disappearTimer / _disappearTimerMax);
        _spriteRenderer.color = _color;

        if (_disappearTimer >= _disappearTimerMax) {
            Destroy(gameObject);
        }
    }

    public void SetColor(Color color) {
        this._color = color;
    }    

    public void SetDisappearTimer(float disappearTimerMax) {
        this._disappearTimerMax = disappearTimerMax;
        _disappearTimer = 0f;
    }

    public void PlaySound(AudioClip clip, float volume, float pitch) {
        _audioSource.volume = volume;
        _audioSource.pitch = pitch;
        _audioSource.PlayOneShot(clip);
    }

}
