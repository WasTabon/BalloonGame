using UnityEngine;
using UnityEngine.EventSystems;

public class Shield : MonoBehaviour
{
    [SerializeField] private float followSpeed = 15f;

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 targetPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        targetPosition = rb.position;
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
        Vector2 newPos = Vector2.Lerp(rb.position, targetPosition, followSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
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
