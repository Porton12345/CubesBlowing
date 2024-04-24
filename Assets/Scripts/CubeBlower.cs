using UnityEngine;

public class CubeBlower : MonoBehaviour
{
    public ChanceOfDivision chance;

    private int minRandom = 2;
    private int maxRandom = 6;
    private float _explosionRadius = 50f;
    private float _explosionForce = 1000f;
    private float _upwardsModifier = 3f;
    private float _maxChanceOfDivision = 100f;
    private float _chanceOfDivision;
    private Color _randomColor;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(raycast, out raycastHit)) 
            {
                if (raycastHit.collider.gameObject == gameObject) 
                {             
                    _chanceOfDivision = Random.Range(0, _maxChanceOfDivision);
                    Debug.Log("Шанс деления " + _chanceOfDivision + " Текущий шанс деления " + chance.chanceOfDivision);

                    if (_chanceOfDivision <= chance.chanceOfDivision)
                    {                        
                        int cubesCount = Random.Range(minRandom, maxRandom);

                        Transform cubeTransform = GetComponent<Transform>();
                        Vector3 cubeCenterPosition = cubeTransform.position;

                        Vector3 currentSize = gameObject.transform.localScale;
                        gameObject.transform.localScale = new Vector3(currentSize.x / 2, currentSize.y / 2, currentSize.z / 2);

                        for (int i = 0; i < cubesCount; i++)
                        {
                            _randomColor = new Color(Random.value, Random.value, Random.value, 1.0f);
                            GameObject newCube = Instantiate(gameObject, cubeCenterPosition, Quaternion.identity);
                            newCube.GetComponent<Renderer>().material.color = _randomColor;
                            newCube.GetComponent<ChanceOfDivision>().chanceOfDivision = chance.chanceOfDivision/2;
                        }

                        Collider[] colliders = Physics.OverlapSphere(cubeCenterPosition, _explosionRadius);

                        foreach (Collider hit in colliders)
                        {
                            Rigidbody rigidBody = hit.GetComponent<Rigidbody>();

                            if (rigidBody != null)
                            {                                
                                rigidBody.AddExplosionForce(_explosionForce, cubeCenterPosition, _explosionRadius, _upwardsModifier, ForceMode.Force);
                            }
                        }
                    }                  
                      
                    Destroy(gameObject);                    
                }
            }            
        }        
    }
}
