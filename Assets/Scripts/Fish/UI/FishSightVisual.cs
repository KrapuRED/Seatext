using UnityEngine;

public class FishSightVisual : MonoBehaviour
{
    [SerializeField] private Transform dangerSight;
    [SerializeField] private Transform safeSight;

    private float DistaceToEndPoint(Transform target)
    {
        float distance = Vector2.Distance(transform.position, target.position);
        return distance;
    }

    public void OnSightVisual(Transform target)
    {
        Debug.Log($"[FishSightVisual - OnSightVisual] Distance To Danger : {DistaceToEndPoint(target)} | Distance To Safe : {DistaceToEndPoint(target)}");

        if (dangerSight == null)
            return;

        SetSightDistance(dangerSight, DistaceToEndPoint(target));
        SetSightDistance(safeSight, DistaceToEndPoint(target));
    }

    public void Dettach()
    {
        dangerSight.SetParent(null);
        safeSight.SetParent(null);
    }

    private void SetSightDistance(Transform sight, float distance)
    {
        Vector3 scale = sight.localScale;
        scale.y = distance;
        sight.localScale = scale;

        Vector3 pos = sight.localPosition;
        pos.y = distance / 2f;
        sight.localPosition = pos;
    }

    private void OnDestroy()
    {
        if (dangerSight != null) Destroy(dangerSight.gameObject);
        if (safeSight != null) Destroy(safeSight.gameObject);
    }
}
