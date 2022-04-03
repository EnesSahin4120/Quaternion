using UnityEngine;

public class QuaternionTest : MonoBehaviour
{
    public GameObject cube;
    public Vector3 angles;
    Matrix rotMatrix;
    float angle;
    Coordinates rotAxis;

    private void Start()
    {
        rotMatrix = Mathematics.RotationMatrix(angles.x * Mathf.Deg2Rad, false, angles.y * Mathf.Deg2Rad, false, angles.z * Mathf.Deg2Rad, false);
        angle = Mathematics.RotationAxisAngle(rotMatrix);
        rotAxis = Mathematics.RotationAxis(rotMatrix, angle);
    }

    private void Update()
    {
        Coordinates quaternion = Mathematics.Quaternion(rotAxis, angle);
        angle += Time.deltaTime * 50;
        cube.transform.rotation = new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }
}
