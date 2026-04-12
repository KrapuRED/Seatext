using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class CustomDebug 
{
    public static void DrawCone(Vector2 top_right_corner, Vector2 bottom_left_corner)
    {
        Vector2 displacement = top_right_corner - bottom_left_corner;
        
        float x_projection = Vector2.Dot(displacement, Vector2.right);
        float y_projection = Vector2.Dot(displacement, Vector2.up);

        Vector2 top_left_coner = new Vector2(-x_projection* 0.5f, y_projection * 0.5f);
        Vector2 bottome_right_corner = new Vector2(x_projection * 0.5f, -y_projection * 0.5f);

        Gizmos.DrawLine(top_right_corner, top_left_coner);
        Gizmos.DrawLine(top_left_coner, bottom_left_corner);
        Gizmos.DrawLine(bottom_left_corner, bottome_right_corner);
        Gizmos.DrawLine(bottome_right_corner, top_right_corner);
    }

    public static void OnDrawLineSight(Vector2 directionEye, float viewAngle, float viewDistance)
    {
        //Set Both Left and Right Boundary of Eye Sight
        Vector3 leftBoundary = Quaternion.AngleAxis(-viewAngle / 2, Vector3.forward) * directionEye;
        Vector3 rightBoundary = Quaternion.AngleAxis(viewAngle / 2, Vector3.forward) * directionEye;

        //Set Both Left and Right End Point of Eye Sight
        Vector3 leftEndPoint = (Vector3)directionEye + leftBoundary * viewDistance;
        Vector3 rightEndPoint = (Vector3)directionEye + rightBoundary * viewDistance;

        Debug.DrawLine(directionEye, leftEndPoint, Color.red);
        Debug.DrawLine(directionEye, rightEndPoint, Color.red);
        Debug.DrawLine(leftEndPoint, rightEndPoint, Color.green);
    }
}
