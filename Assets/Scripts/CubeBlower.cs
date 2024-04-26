using UnityEngine;

public class CubeBlower : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;    

    private void Update()
    {
        BlowCubes();
    }

    private void BlowCubes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.gameObject == gameObject)
                {
                    _cubePrefab.BlowCubeInChance(_cubePrefab);                   
                }
            }
        }
    }    
}
