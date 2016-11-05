using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// GestureAction performs custom actions based on 
/// which gesture is being performed.
/// </summary>
public class GestureAction : MonoBehaviour
{
    //[Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 5.0f;
    public float TranslationSensitiviy = 2.0f;

    private Vector3 manipulationPreviousPosition;   // hand position of previous step
    private Vector3 manipulationInitialPosition;    // hand position atbegining of this manipulation session

    // magnetic alignment parameters
    private float magneticFieldRadius = 0.03f;
    public bool inMagneticField = false;

    // rotation parameters
    private float rotationFactor_X;
    private float rotationFactor_Y;
    private float rotationFactor_Z;
    private float rotationThreshold = 0.1f;
    private float unitRotation = 15f;

    void Update()
    {
        if(Manipulator.Instance.tappedBrick.GetComponent<MagneticCollision>()!= null)
        {
            MagneticAlignment(Manipulator.Instance.tappedBrick.GetComponent<MagneticCollision>().DetectedGameObjects);
        }     
    }

    void PerformRotation_X(Vector3 position)
    {
        if (GestureManager.Instance.IsRotating == 1 && Manipulator.Instance.tappedBrick != null)
        {
            Vector3 handVectorProject = Vector3.Project((position - manipulationPreviousPosition), Manipulator.Instance.tappedBrick.transform.up);
            rotationFactor_X = RotationSensitivity * handVectorProject.magnitude;

            if (rotationFactor_X > rotationThreshold)
            {
                gameObject.GetComponentInParent<Transform>().RotateAround(transform.position, Manipulator.Instance.tappedBrick.transform.right, unitRotation * Mathf.Cos(Vector3.Angle(handVectorProject, Manipulator.Instance.tappedBrick.transform.up)));    // rotate the handlers
                Manipulator.Instance.tappedBrick.transform.RotateAround(transform.position, Manipulator.Instance.tappedBrick.transform.right, unitRotation * Mathf.Cos(Vector3.Angle(handVectorProject, Manipulator.Instance.tappedBrick.transform.up)));      // rotate the brick
                manipulationPreviousPosition = position;
            }
        }
    }

    void PerformRotation_Y(Vector3 position)
    {
        if (GestureManager.Instance.IsRotating == 2 && Manipulator.Instance.tappedBrick != null)
        {

            Vector3 handVectorProject = Vector3.Project((position - manipulationPreviousPosition), Manipulator.Instance.tappedBrick.transform.forward);
            rotationFactor_Y = RotationSensitivity * handVectorProject.magnitude;

            if (rotationFactor_Y > rotationThreshold)
            {
                gameObject.GetComponentInParent<Transform>().RotateAround(transform.position, Manipulator.Instance.tappedBrick.transform.up, unitRotation * Mathf.Cos(Vector3.Angle(handVectorProject, Manipulator.Instance.tappedBrick.transform.forward)));    // rotate the handlers
                Manipulator.Instance.tappedBrick.transform.RotateAround(transform.position, Manipulator.Instance.tappedBrick.transform.up, unitRotation * Mathf.Cos(Vector3.Angle(handVectorProject, Manipulator.Instance.tappedBrick.transform.forward)));
                manipulationPreviousPosition = position;
            }
        }
    }

    void PerformRotation_Z(Vector3 position)
    {
        if (GestureManager.Instance.IsRotating == 3 && Manipulator.Instance.tappedBrick != null)
        {

            Vector3 handVectorProject = Vector3.Project((position - manipulationPreviousPosition), Manipulator.Instance.tappedBrick.transform.right);
            rotationFactor_Z = RotationSensitivity * handVectorProject.magnitude;

            if (rotationFactor_Z > rotationThreshold)
            {
                gameObject.GetComponentInParent<Transform>().RotateAround(transform.position, Manipulator.Instance.tappedBrick.transform.forward, unitRotation * Mathf.Cos(Vector3.Angle(handVectorProject, Manipulator.Instance.tappedBrick.transform.right)));
                Manipulator.Instance.tappedBrick.transform.RotateAround(transform.position, Manipulator.Instance.tappedBrick.transform.forward, unitRotation * Mathf.Cos(Vector3.Angle(handVectorProject, Manipulator.Instance.tappedBrick.transform.right)));
                manipulationPreviousPosition = position;
            }
        }
    }

    void PerformTranslation_X(Vector3 position)
    {
        if (GestureManager.Instance.IsManipulating == 1 && Manipulator.Instance.tappedBrick != null)
        {
            Vector3 handMove = position - manipulationPreviousPosition;
            Vector3 brickMove = Vector3.Project(handMove, Manipulator.Instance.tappedBrick.transform.right);

            if (brickMove.magnitude > magneticFieldRadius || !inMagneticField)
            {
                gameObject.GetComponentInParent<Transform>().position += brickMove;
                Manipulator.Instance.tappedBrick.transform.position += brickMove;
                manipulationPreviousPosition = position;
            }     
        }
    }

    void PerformTranslation_Y(Vector3 position)
    {
        if (GestureManager.Instance.IsManipulating == 2 && Manipulator.Instance.tappedBrick != null)
        {
            Vector3 handMove = position - manipulationPreviousPosition;
            Vector3 brickMove = Vector3.Project(handMove, Manipulator.Instance.tappedBrick.transform.up);

            if (brickMove.magnitude > magneticFieldRadius || !inMagneticField)
            {
                gameObject.GetComponentInParent<Transform>().position += brickMove;
                Manipulator.Instance.tappedBrick.transform.position += brickMove;
                manipulationPreviousPosition = position;
            }
        }
    }

    void PerformTranslation_Z(Vector3 position)
    {
        if (GestureManager.Instance.IsManipulating == 3 && Manipulator.Instance.tappedBrick != null)
        {
            Vector3 handMove = position - manipulationPreviousPosition;
            Vector3 brickMove = Vector3.Project(handMove, Manipulator.Instance.tappedBrick.transform.forward);

            if (brickMove.magnitude > magneticFieldRadius || !inMagneticField)
            {
                gameObject.GetComponentInParent<Transform>().position += brickMove;
                Manipulator.Instance.tappedBrick.transform.position += brickMove;
                manipulationPreviousPosition = position;
            }
            
        }
    }

    void PerformManipulationStart(Vector3 position)
    {
        manipulationInitialPosition = position;
        manipulationPreviousPosition = manipulationInitialPosition;
    }

    // [TO DO] Needs Improvements!!
    void MagneticAlignment(List<GameObject> detected)
    {
        if (detected.Count > 0)
        {
            gameObject.GetComponentInParent<Transform>().rotation = detected[0].transform.rotation;
            Manipulator.Instance.tappedBrick.transform.rotation = detected[0].transform.rotation;
            
            switch (AlignToAxis(Manipulator.Instance.tappedBrick, detected[0]))
            {
                case 0: // x+
                    Manipulator.Instance.tappedBrick.transform.position = detected[0].transform.position + Manipulator.Instance.tappedBrick.transform.right * 0.05f;
                    break;
                case 1: // x-
                    Manipulator.Instance.tappedBrick.transform.position = detected[0].transform.position - Manipulator.Instance.tappedBrick.transform.right * 0.05f;
                    break;
                case 2: // y+
                    Manipulator.Instance.tappedBrick.transform.position = detected[0].transform.position + Manipulator.Instance.tappedBrick.transform.up * 0.05f;
                    break;
                case 3: // y-
                    Manipulator.Instance.tappedBrick.transform.position = detected[0].transform.position - Manipulator.Instance.tappedBrick.transform.up * 0.05f;
                    break;
                case 4: // z+
                    Manipulator.Instance.tappedBrick.transform.position = detected[0].transform.position + Manipulator.Instance.tappedBrick.transform.forward * 0.05f;
                    break;
                case 5: // z-
                    Manipulator.Instance.tappedBrick.transform.position = detected[0].transform.position - Manipulator.Instance.tappedBrick.transform.forward * 0.05f;
                    break;
                default:
                    break;
            }

            gameObject.GetComponentInParent<Transform>().position = Manipulator.Instance.tappedBrick.transform.position;
            inMagneticField = true;
            Manipulator.Instance.tappedBrick.SendMessageUpwards("MagneticClip");
        }
        else
        {
            inMagneticField = false;
        }
    }

    private int AlignToAxis(GameObject active, GameObject target)
    {
        float[] cosToAxis = new float[3];
        Vector3 distanceVec = active.transform.position - target.transform.position;
        cosToAxis[0] = Mathf.Sin(Vector3.Angle(distanceVec, target.transform.right));
        cosToAxis[1] = Mathf.Sin(Vector3.Angle(distanceVec, target.transform.up));
        cosToAxis[2] = Mathf.Sin(Vector3.Angle(distanceVec, target.transform.forward));

        if(cosToAxis[0] <= cosToAxis[1] && cosToAxis[0] <= cosToAxis[2])
        {
            if(Vector3.Angle(distanceVec, target.transform.right) > 90f)
            {
                return 1;
            }
            else
            {
                return 0;
            }
            
        }
        else if (cosToAxis[1] <= cosToAxis[0] && cosToAxis[1] <= cosToAxis[2])
        {
            if (Vector3.Angle(distanceVec, target.transform.up) > 90f)
            {
                return 3;
            }
            else
            {
                return 2;
            }
        }
        else if (cosToAxis[2] <= cosToAxis[0] && cosToAxis[2] <= cosToAxis[1])
        {
            if (Vector3.Angle(distanceVec, target.transform.forward) > 90)
            {
                return 5;
            }
            else
            {
                return 4;
            }
        }

        return -1;
    }
}