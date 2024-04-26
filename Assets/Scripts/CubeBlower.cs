using UnityEngine;

[RequireComponent(typeof(Cube))]
public class CubeBlower : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab; 
    
    private int _minRandom = 2;
    private int _maxRandom = 6;
    private float _explosionRadius = 50f;
    private float _explosionForce = 1000f;
    private float _upwardsModifier = 3f;
    private float _maxChanceOfDivision = 100f;
    private float _chanceOfDivision;     
    private Color _randomColor;

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
                    _chanceOfDivision = Random.Range(0, _maxChanceOfDivision);
                    Debug.Log("Шанс деления " + _chanceOfDivision + " Текущий шанс деления " + _cubePrefab.currentChanceOfDivision);

                    if (_chanceOfDivision <= _cubePrefab.currentChanceOfDivision)
                    {
                        int cubesCount = Random.Range(_minRandom, _maxRandom);

                        Transform cubeTransform = GetComponent<Transform>();
                        Vector3 cubeCenterPosition = cubeTransform.position;

                        Vector3 currentSize = gameObject.transform.localScale;
                        _cubePrefab.transform.localScale = new Vector3(currentSize.x / 2, currentSize.y / 2, currentSize.z / 2);
                        Collider[] colliders = new Collider[cubesCount];

                        CreateNewCubes(colliders, cubesCount, cubeCenterPosition);
                        TossNewCubes(colliders, cubeCenterPosition);
                    }

                    Destroy(gameObject);
                }
            }
        }
    }

    private void CreateNewCubes(Collider[] colliders, int cubesCount, Vector3 cubeCenterPosition)
    {
        for (int i = 0; i < cubesCount; i++)
        {
            _randomColor = new Color(Random.value, Random.value, Random.value, 1.0f);
            Cube newCube = Instantiate(_cubePrefab, cubeCenterPosition, Quaternion.identity);            
            newCube.GetComponent<Renderer>().material.color = _randomColor;
            colliders[i] = newCube.GetComponent<Collider>();
            newCube.currentChanceOfDivision /= 2f;
        }
    }

    private void TossNewCubes(Collider[] colliders, Vector3 cubeCenterPosition)
    {
        foreach (Collider hit in colliders)
        {
            Rigidbody rigidBody = hit.GetComponent<Rigidbody>();

            if (rigidBody != null)
            {
                rigidBody.AddExplosionForce(_explosionForce, cubeCenterPosition, _explosionRadius, _upwardsModifier, ForceMode.Force);
            }
        }
    }
}
