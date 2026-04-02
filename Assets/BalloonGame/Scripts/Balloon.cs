using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
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

        float wobbleX = startX + Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;
        float newY = rb.position.y + moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(new Vector2(wobbleX, newY));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.GetComponent<Obstacle>() == null) return;
        isDead = true;
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
