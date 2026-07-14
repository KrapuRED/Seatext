using UnityEngine;

public class FishSightVisual : MonoBehaviour
{
    [SerializeField] private Transform dangerSight;
    [SerializeField] private Transform safeSight;

    private Vector3 _dangerSightLocalPos;
    private Vector3 _safeSightLocalPos;

    private void Awake()
    {
        if (dangerSight != null) _dangerSightLocalPos = dangerSight.localPosition;
        if (safeSight != null) _safeSightLocalPos = safeSight.localPosition;
    }
    
    public void Attach()
    {
        if (dangerSight != null && dangerSight.parent != transform)
        {
            dangerSight.SetParent(transform);
            dangerSight.localPosition = _dangerSightLocalPos;
            dangerSight.localRotation = Quaternion.identity;
        }

        if (safeSight != null && safeSight.parent != transform)
        {
            safeSight.SetParent(transform);
            safeSight.localPosition = _safeSightLocalPos;
            safeSight.localRotation = Quaternion.identity;
        }
    }
    
    private void AimSight(Transform sight, Vector2 direction, float distance)
    {
        if (sight == null || direction.sqrMagnitude < 0.0001f)
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        sight.rotation = Quaternion.Euler(0f, 0f, angle);

        Vector3 scale = sight.localScale;
        scale.y = distance;
        sight.localScale = scale;

        sight.position = transform.position + (Vector3)(direction.normalized * (distance / 2f));
    }
    
    public void OnSightVisual(Transform target)
    {
        if (dangerSight == null)
            return;

        Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
        float distance = direction.magnitude;

        AimSight(dangerSight, direction, distance);
        AimSight(safeSight, direction, distance);
    }

    public void Dettach()
    {
        dangerSight.SetParent(null);
        safeSight.SetParent(null);
    }

    private void OnDestroy()
    {
        if (dangerSight != null) Destroy(dangerSight.gameObject);
        if (safeSight != null) Destroy(safeSight.gameObject);
    }
}
