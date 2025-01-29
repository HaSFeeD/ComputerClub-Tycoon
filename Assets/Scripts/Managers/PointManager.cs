<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
=======
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager instance;
<<<<<<< HEAD
    public List<Phase> _points;
=======
    public GameObject spawnPoint;
    public GameObject registrationPoint;
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
    public GameObject finishPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
<<<<<<< HEAD

=======
>>>>>>> 27866b6 (Refactored Some Code and add new Features)
