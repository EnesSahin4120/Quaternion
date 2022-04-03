using UnityEngine;

public class AxisRotateTest : MonoBehaviour
{
    public GameObject ball;
    public Vector3 angle;

    private void Start()
    {
        angle = angle * Mathf.Deg2Rad;

        Coordinates position = new Coordinates(ball.transform.position, 1);
        ball.transform.position = Mathematics.Rotate(position, angle.x, true, angle.y, true, angle.z, true).ToVector();

        Matrix rotMatrix = Mathematics.RotationMatrix(angle.x, true, angle.y, true, angle.z, true);
        float rotAngle = Mathematics.RotationAxisAngle(rotMatrix);
        Coordinates rotAxis = Mathematics.RotationAxis(rotMatrix, rotAngle);

        Debug.Log("Angle : " + rotAngle + "about" + rotAxis.ToString());

        Debug.DrawLine(Vector3.zero, 5 * rotAxis.ToVector(), Color.black, Mathf.Infinity);
    }
}
