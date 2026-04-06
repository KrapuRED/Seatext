using UnityEngine;

public class FishEyeSight : MonoBehaviour
{
    [Header("Eye Fish Config")]
    [SerializeField] private float viewDistance;
    [SerializeField] private Transform LeftEye;
    [SerializeField] private Transform RightEye;
    [SerializeField] private LayerMask visionLayerMask;
    [SerializeField] private string dectactionTag;

    [SerializeField] private Transform currentObject;

    private void Update()
    {
        InitileizeEyeSight();
    }

    private void InitileizeEyeSight()
    {
        //Set Raycast to see
        CheckEyeSight(LeftEye);
        CheckEyeSight(RightEye);
    }

    private void CheckEyeSight(Transform eye)
    {
        //Direction Eye Facing
        Vector2 directionEye = eye.up;

        Debug.DrawLine(eye.position, eye.position + (Vector3)(directionEye * viewDistance), Color.red);

        RaycastHit2D hit = Physics2D.Raycast(eye.position, directionEye, viewDistance, visionLayerMask);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag(dectactionTag)){
                currentObject = hit.transform;
                SetBeenHunted(true);
                Debug.Log($"[FishEyeSight - CheckEyeSight] {gameObject.name} is see {currentObject.name}");
            }
        }
        else
        {
            SetBeenHunted(false);
            currentObject = null;
        }
    }

    private void SetBeenHunted(bool hunted)
    {
        if (currentObject == null || currentObject.tag == "Fish")
        {
            return;
        }

        Fish currentFish = currentObject.GetComponent<Fish>();
        currentFish.SetBeenHunted(hunted, currentFish);
    }

    // Able identefy is an edible fish or not
    public bool IsEdibleFood()
    {
        //If Between in left and right eye set true
        
        return false;
    }
}
