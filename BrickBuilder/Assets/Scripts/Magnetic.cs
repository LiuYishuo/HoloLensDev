using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Magnetic behaviour of an object with such property
/// maintain a list of object within it's magnetic field \in [0,6]
/// </summary>
public class Magnetic : MonoBehaviour {

    public float magneticFieldRadius = 100f;
    public List<GameObject> GameObjectInMagneticField = new List<GameObject>();
    private RaycastHit[] ObjectsInMagneticField = new RaycastHit[raycastDirections];
    //LayerMask targetingLayer = 8; // customized layer: "Magnetic Raycast"

    private static int raycastDirections = 6;

	void FixedUpdate () {

        ScanMagneticField();

        //SortByDistance(GameObjectInMagneticField);
	}

    private void ScanMagneticField()
    {
        int targetingLayer = LayerMask.NameToLayer("Magnetic"); // customized layer: "Magnetic Raycast"

        bool Hit = Physics.Raycast(transform.position, transform.right, out ObjectsInMagneticField[0], magneticFieldRadius, targetingLayer);
        if (Hit)
        {
            GameObjectInMagneticField.Add(ObjectsInMagneticField[0].transform.gameObject);
        }

        //if (Physics.Raycast(gameObject.transform.position, gameObject.transform.right, out ObjectsInMagneticField[0], magneticFieldRadius, targetingLayer))
        //{
        //    GameObjectInMagneticField.Add(ObjectsInMagneticField[0].transform.gameObject);
        //}

        if (Physics.Raycast(gameObject.transform.position, - gameObject.transform.right, out ObjectsInMagneticField[1], magneticFieldRadius, targetingLayer))
        {
            GameObjectInMagneticField.Add(ObjectsInMagneticField[1].transform.gameObject);
        }

        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.up, out ObjectsInMagneticField[2], magneticFieldRadius, targetingLayer))
        {
            GameObjectInMagneticField.Add(ObjectsInMagneticField[2].transform.gameObject);
        }

        if (Physics.Raycast(gameObject.transform.position, - gameObject.transform.up, out ObjectsInMagneticField[3], magneticFieldRadius, targetingLayer))
        {
            GameObjectInMagneticField.Add(ObjectsInMagneticField[3].transform.gameObject);
        }

        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out ObjectsInMagneticField[4], magneticFieldRadius, targetingLayer))
        {
            GameObjectInMagneticField.Add(ObjectsInMagneticField[4].transform.gameObject);
        }

        if (Physics.Raycast(gameObject.transform.position, - gameObject.transform.forward, out ObjectsInMagneticField[5], magneticFieldRadius, targetingLayer))
        {
            GameObjectInMagneticField.Add(ObjectsInMagneticField[5].transform.gameObject);
        }
    }

    // sort the list by distance to current object
    private void SortByDistance(List<GameObject> list)
    {
        if (list.Count > 0)
        {
            list.Sort(delegate (GameObject go_1, GameObject go_2) {
                return Vector3.Distance(transform.position, go_1.transform.position).CompareTo((Vector3.Distance(transform.position, go_2.transform.position)));
            });
        }     
    }


}
