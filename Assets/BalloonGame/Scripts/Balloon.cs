using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private float wobbleAmplitude = 0.15f;
    [SerializeField] private float wobbleFrequency = 1.5f;

    private float startX;
    private Rigidbody2D rb;
    private bool isDead;

    public System.Action OnBalloonHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startX = transform.position.x;
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        if (TapToStart.Instance != null && !TapToStart.Instance.HasStarted) return;

        float speed = DifficultyManager.Instance != null ? DifficultyManager.Instance.BalloonSpeed : 3f;
        float wobbleX = startX + Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;
        float newY = rb.position.y + speed * Time.fixedDeltaTime;
        rb.MovePosition(new Vector2(wobbleX, newY));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.GetComponent<Obstacle>() == null) return;
        isDead = true;

        if (ParticleManager.Instance != null)
            ParticleManager.Instance.PlayBalloonPop(transform.position, new Color(1f, 0.45f, 0.5f));

        if (ScreenShake.Instance != null)
            ScreenShake.Instance.ShakeHeavy();

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayBalloonPop();
            SFXManager.Instance.PlayGameOver();
        }

        if (HapticManager.Instance != null)
            HapticManager.Instance.Heavy();

        OnBalloonHit?.Invoke();
    }

    public void ResetBalloon(Vector2 position)
    {
        isDead = false;
        transform.position = position;
        startX = position.x;
    }

    public float GetHeight()
    {
        return transform.position.y;
    }
}
