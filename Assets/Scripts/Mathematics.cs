using UnityEngine;

public class Mathematics : MonoBehaviour
{
    static public float Square(float grade)
    {
        return grade * grade;
    }

    static public float Distance(Coordinates coord1, Coordinates coord2)
    {
        float diffSquared = Square(coord1.x - coord2.x) +
            Square(coord1.y - coord2.y) +
            Square(coord1.z - coord2.z);
        float squareRoot = Mathf.Sqrt(diffSquared);
        return squareRoot;
    }

    static public float Dot(Coordinates vector1, Coordinates vector2)
    {
        return (vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z);
    }

    static public float Angle(Coordinates dirVector1, Coordinates dirVector2)
    {
        float usableNumerical = Dot(dirVector1, dirVector2) /
                    (Distance(new Coordinates(0, 0, 0), dirVector1) * Distance(new Coordinates(0, 0, 0), dirVector2));

        return Mathf.Acos(usableNumerical);
    }

    static public Coordinates Rotate_2D(Coordinates positionVector, float angle, bool clockwise) 
    {
        if (clockwise)
        {
            angle = 2 * Mathf.PI - angle;
        }

        float value_X = positionVector.x * Mathf.Cos(angle) - positionVector.y * Mathf.Sin(angle);
        float value_Y = positionVector.x * Mathf.Sin(angle) + positionVector.y * Mathf.Cos(angle);

        return new Coordinates(value_X, value_Y, 0);
    }
    
    static public Coordinates Rotate(Coordinates positionVector, float angle_X, bool clockwise_X,
                                                                 float angle_Y, bool clockwise_Y,
                                                                 float angle_Z, bool clockwise_Z)
    {
        if (clockwise_X)
        {
            angle_X = 2 * Mathf.PI - angle_X;
        }
        if (clockwise_Y)
        {
            angle_Y = 2 * Mathf.PI - angle_Y;
        }
        if (clockwise_Z)
        {
            angle_Z = 2 * Mathf.PI - angle_Z;
        }

        float[,] xElements = { { 1,0,0,0 },
                               { 0,Mathf.Cos(angle_X),-Mathf.Sin(angle_X),0 },
                               { 0,Mathf.Sin(angle_X), Mathf.Cos(angle_X),0 },
                               { 0,0,0,1 } };
        Matrix XRoll = new Matrix(4, 4, xElements);

        float[,] yElements = { { Mathf.Cos(angle_Y),0,Mathf.Sin(angle_Y),0 },
                               { 0,1,0,0 },
                               {-Mathf.Sin(angle_Y),0,Mathf.Cos(angle_Y),0 },
                               { 0,0,0,1 } };
        Matrix YRoll = new Matrix(4, 4, yElements);

        float[,] zElements = { { Mathf.Cos(angle_Z),-Mathf.Sin(angle_Z),0,0 },
                               { Mathf.Sin(angle_Z),Mathf.Cos(angle_Z),0,0 },
                               { 0,0,1,0 },
                               { 0,0,0,1 } };
        Matrix z_Matrix = new Matrix(4, 4, zElements);

        Matrix position_Matrix = new Matrix(4, 1, positionVector.AsMatrixElements());

        Matrix resultMatrix = z_Matrix * YRoll * XRoll * position_Matrix;

        return resultMatrix.AsCoordinates();
    }

    static public Matrix RotationMatrix(float angle_X, bool clockwise_X,
                                        float angle_Y, bool clockwise_Y,
                                        float angle_Z, bool clockwise_Z)
    {
        if (clockwise_X)
        {
            angle_X = 2 * Mathf.PI - angle_X;
        }
        if (clockwise_Y)
        {
            angle_Y = 2 * Mathf.PI - angle_Y;
        }
        if (clockwise_Z)
        {
            angle_Z = 2 * Mathf.PI - angle_Z;
        }

        float[,] x_Elements = {{ 1,0,0,0 },
                               { 0,Mathf.Cos(angle_X),-Mathf.Sin(angle_X),0 },
                               { 0,Mathf.Sin(angle_X), Mathf.Cos(angle_X),0 },
                               { 0,0,0,1 } };
        Matrix x_Matrix = new Matrix(4, 4, x_Elements);

        float[,] y_Elements = {{ Mathf.Cos(angle_Y),0,Mathf.Sin(angle_Y),0 },
                               { 0,1,0,0 },
                               {-Mathf.Sin(angle_Y),0,Mathf.Cos(angle_Y),0 },
                               { 0,0,0,1 } };
        Matrix y_Matrix = new Matrix(4, 4, y_Elements);

        float[,] z_Elements = {{ Mathf.Cos(angle_Z),-Mathf.Sin(angle_Z),0,0 },
                               { Mathf.Sin(angle_Z),Mathf.Cos(angle_Z),0,0 },
                               { 0,0,1,0 },
                               { 0,0,0,1 } };
        Matrix z_Matrix = new Matrix(4, 4, z_Elements);

        Matrix resultMatrix = z_Matrix * y_Matrix * x_Matrix;

        return resultMatrix;
    }

    static public Coordinates Quaternion(Coordinates rotationAxis, float angle)
    {
        Coordinates rot_AxisNormal = rotationAxis.GetNormal();
        float w = Mathf.Cos(angle * Mathf.Deg2Rad / 2);
        float s = Mathf.Sin(angle * Mathf.Deg2Rad / 2);

        Coordinates q = new Coordinates(rot_AxisNormal.x * s, rot_AxisNormal.y * s, rot_AxisNormal.z * s, w);
        return q;
    }

    static public Coordinates QuaternionRotate(Coordinates positionVector, Coordinates rotationAxis, float angle)
    {
        Coordinates rot_AxisNormal = rotationAxis.GetNormal();
        float w = Mathf.Cos(angle * Mathf.Deg2Rad / 2);
        float s = Mathf.Sin(angle * Mathf.Deg2Rad / 2);
        Coordinates q = new Coordinates(rot_AxisNormal.x * s, rot_AxisNormal.y * s, rot_AxisNormal.z * s, w);

        float[,] quaternionElements = {
             { 1 - 2*q.y*q.y - 2*q.z*q.z, 2*q.x*q.y - 2*q.w*q.z,      2*q.x*q.z + 2*q.w*q.y,     0 },
             { 2*q.x*q.y + 2*q.w*q.z,     1 - 2*q.x*q.x - 2*q.z*q.z,  2*q.y*q.z - 2*q.w*q.x,     0 },
             { 2*q.x*q.z - 2*q.w*q.y,     2*q.y*q.z + 2*q.w*q.x,      1 - 2*q.x*q.x - 2*q.y*q.y, 0 },
             { 0,                         0,                          0,                         1 } 
        };

        Matrix quaternionMatrix = new Matrix(4, 4, quaternionElements);
        Matrix positionMatrix = new Matrix(4, 1, positionVector.AsMatrixElements());

        Matrix resultMatrix = quaternionMatrix * positionMatrix;
        return resultMatrix.AsCoordinates();
    }

    static public float RotationAxisAngle(Matrix rotationMatrix)
    {
        float angle;
        angle = Mathf.Acos(0.5f * (rotationMatrix.elements[0, 0] +
                                   rotationMatrix.elements[1, 1] +
                                   rotationMatrix.elements[2, 2] +
                                   rotationMatrix.elements[3, 3] - 2));
        return angle;
    }
    static public Coordinates RotationAxis(Matrix rotationMatrix, float angle)
    {
        float x = (rotationMatrix.elements[2, 1] - rotationMatrix.elements[1, 2]) / (2 * Mathf.Sin(angle));
        float y = (rotationMatrix.elements[0, 2] - rotationMatrix.elements[2, 0]) / (2 * Mathf.Sin(angle));
        float z = (rotationMatrix.elements[1, 0] - rotationMatrix.elements[0, 1]) / (2 * Mathf.Sin(angle));

        return new Coordinates(x, y, z, 0);
    }
}


