using UnityEngine;

public class FishEyeSight : MonoBehaviour
{
    [Header("Eye Fish Config")]
    [SerializeField] private float viewDistance;
    [SerializeField] private Transform LeftEye;
    [SerializeField] private Transform RightEye;
    [SerializeField] private LayerMask visionLayerMask;
    [SerializeField] private string dectactionTag;

    [SerializeField] private Transform _currentObject;

    public bool isCanSee { private get; set; }
    public Vector2 AttackDirection {get; private set;}
    public Transform currentObject => _currentObject;


    public void UpdateEyeSight()
    {
        //Set Raycast to see
        if (!isCanSee)
            return;

        Transform leftEyeResult     = CheckEyeSight(LeftEye);
        Transform rightEyeResult    = CheckEyeSight(RightEye);

        _currentObject = leftEyeResult != null ? leftEyeResult : rightEyeResult;
        bool isSeeing = _currentObject != null;

        if (isSeeing)
        {
            SetBeenHunted(isSeeing);
        }

    }

    private Transform CheckEyeSight(Transform eye)
    {
        //Direction Eye Facing
        Vector2 directionEye = eye.up;

        Debug.DrawLine(eye.position, eye.position + (Vector3)(directionEye * viewDistance), Color.red);

        RaycastHit2D hit = Physics2D.Raycast(eye.position, directionEye, viewDistance, visionLayerMask);

        if (hit.collider != null && hit.collider.CompareTag(dectactionTag))
        {
            return hit.transform;
        }

        return null;
    }

    private void SetBeenHunted(bool hunted)
    {
        if (_currentObject == null) return;

        Fish currentFish = _currentObject.GetComponent<Fish>();
        if (currentFish != null)
        {
            AttackDirection = (_currentObject.position - transform.position).normalized;
            currentFish.SetBeenHunted(hunted, currentFish);
        }
    }

    // Able identefy is an edible fish or not
    public bool IsEdibleFood()
    {
        //If Between in left and right eye set true
        
        return false;
    }
}
