using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private float m_Speed = 10f;
    private Rigidbody2D _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (Mathf.Abs(transform.position.x) > 20 || Mathf.Abs(transform.position.y) > 20) {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable() {
        _rb.velocity = Vector2.zero;
    }

    public void ShootBullet(Vector3 direction) {
        _rb.velocity = direction * m_Speed;        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            gameObject.SetActive(false);
            collision.gameObject.GetComponent<Enemy>().Hit();
        }
    }
}
