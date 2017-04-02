using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssembleSubstitute : MonoBehaviour {
    public GameObject assembleBrick;
    private List<GameObject> assembles = new List<GameObject>();

    public void Replicate(GameObject activeBrick)
    {
        bool flag_occlusion = false;

        foreach (GameObject clone in assembles)
        {
            if(clone.transform.position == activeBrick.transform.position)
            {
                flag_occlusion = true;
            }
        }

        if (!flag_occlusion)
        {
            GameObject clone = (GameObject)Instantiate(assembleBrick, activeBrick.transform.position, activeBrick.transform.rotation);
            assembles.Add(clone);
            Calibrate();

            Destroy(activeBrick);
        }
    }

    private void Calibrate()
    {
        Vector3 movingVector = Vector3.zero;
        if(assembles.Count == 0)
        {
            return;
        }
        else if (assembles.Count == 1)
        {
            transform.rotation = assembles[0].transform.rotation;
            transform.position = assembles[0].transform.position;
            assembles[0].transform.SetParent(transform);
        }
        else
        {
            foreach (GameObject clone in assembles)
            {
                movingVector += (clone.transform.position - transform.position) / assembles.Count;
                clone.transform.SetParent(null);
            }
            transform.position += movingVector;

            foreach (GameObject clone in assembles)
            {
                clone.transform.SetParent(transform);
            }
        }
     
    }

    // Lay-off all clone bricks by Manipulator voice command
    public void Reset()
    {
        foreach (GameObject clone in assembles)
        {
            clone.transform.SetParent(null);
            Destroy(clone);
        }
        assembles.Clear();
        transform.position = Camera.main.transform.position - Camera.main.transform.up;
    }
}
