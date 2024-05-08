using System;
using UnityEngine;

public class CubeBlower : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
      
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(raycast, out raycastHit))
            {
                Transform objectHit = raycastHit.transform;

                if (objectHit == _cubePrefab.transform)
                    _cubePrefab.BlowCubeInChance(_cubePrefab);
            }
        }
    }    
}
