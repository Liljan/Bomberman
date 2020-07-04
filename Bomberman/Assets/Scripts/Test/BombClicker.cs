using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombClicker : MonoBehaviour
{
    public Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("1"))
            LevelEvents.Instance().InvokeSpawnOrthogonalBomb(MousePositionWorld());
        else if (Input.GetKeyDown("2"))
            LevelEvents.Instance().InvokeSpawnDiagonalBomb(MousePositionWorld());
    }

   private Vector3 MousePositionWorld()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 0.0f;

        return camera.ScreenToWorldPoint(pos);
    }
}
