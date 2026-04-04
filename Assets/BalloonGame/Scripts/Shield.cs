using UnityEngine;
using UnityEngine.EventSystems;

public class Shield : MonoBehaviour
{
    [SerializeField] private float followSpeed = 15f;
    [SerializeField] private float maxMovePerFrame = 0.8f;

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 targetPosition;
    private ShieldVisuals visuals;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        mainCam = Camera.main;
        targetPosition = rb.position;
        visuals = GetComponent<ShieldVisuals>();
    }

    private void Update()
    {
        if (IsPointerOverUI()) return;

        if (Input.GetMouseButton(0))
        {
            Vector3 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = new Vector2(worldPos.x, worldPos.y);
        }
    }

    private void FixedUpdate()
    {
        Vector2 desired = Vector2.Lerp(rb.position, targetPosition, followSpeed * Time.fixedDeltaTime);
        Vector2 delta = desired - rb.position;
        if (delta.magnitude > maxMovePerFrame)
            desired = rb.position + delta.normalized * maxMovePerFrame;
        rb.MovePosition(desired);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Obstacle>() == null) return;

        Vector2 contactPoint = collision.GetContact(0).point;
        Color obsColor = Color.white;
        var obsSR = collision.gameObject.GetComponent<SpriteRenderer>();
        if (obsSR != null) obsColor = obsSR.color;

        if (ParticleManager.Instance != null)
            ParticleManager.Instance.PlayShieldHit(contactPoint, obsColor);

        if (ScreenShake.Instance != null)
            ScreenShake.Instance.ShakeLight();

        if (visuals != null)
            visuals.PlayHitEffect();

        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayShieldHit();

        if (HapticManager.Instance != null)
            HapticManager.Instance.Light();
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

        return false;
    }

    public void ResetShield(Vector2 position)
    {
        transform.position = position;
        targetPosition = position;
    }
}
