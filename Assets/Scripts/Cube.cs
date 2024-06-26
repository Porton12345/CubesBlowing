using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Transform))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;

    private float _currentChanceOfDivision = 100f;
    private int _minRandom = 2;
    private int _maxRandom = 6;
    private float _currentExplosionRadius = 30f;    
    private float _upwardsModifier = 3f;
    private float _maxChanceOfDivision = 100f;
    private float _chanceOfDivision;
    private Color _randomColor;
    private int _sizeKoefficient = 2;
    private int _force�oefficient = 2;
    private int _chanceKoefficient = 2;
    private float _radiusKoefficient = 1.5f;
    private float _currentExplosionForce = 500f;

    public void BlowCubeInChance(Cube cubePrefab)
    {
        _chanceOfDivision = Random.Range(0, _maxChanceOfDivision);
        Debug.Log("���� ������� " + _chanceOfDivision + " ������� ���� ������� " + _currentChanceOfDivision);

        if (_chanceOfDivision <= _currentChanceOfDivision)
        {
            int cubesCount = Random.Range(_minRandom, _maxRandom);                        
            Vector3 cubeCenterPosition = cubePrefab.transform.position;
            Vector3 currentSize = gameObject.transform.localScale;
            cubePrefab.transform.localScale = new Vector3(currentSize.x / _sizeKoefficient, currentSize.y / _sizeKoefficient, currentSize.z / _sizeKoefficient);
            _currentChanceOfDivision /= _chanceKoefficient;           
            CreateNewCubes(cubesCount, cubeCenterPosition);            
        }
        else
        {
            _currentExplosionForce *= _force�oefficient;
            _currentExplosionRadius *= _radiusKoefficient;
            Vector3 cubeCenterPosition = _cubePrefab.transform.position;
            Collider[] blowColliders = Physics.OverlapSphere(cubeCenterPosition, _currentExplosionRadius);
            TossNewCubes(blowColliders, cubeCenterPosition, _currentExplosionForce, _currentExplosionRadius);
        }

        Destroy(gameObject);
    }

    private void CreateNewCubes(int cubesCount, Vector3 cubeCenterPosition)
    {
        for (int i = 0; i < cubesCount; i++)
        {
            _randomColor = new Color(Random.value, Random.value, Random.value);
            Cube newCube = Instantiate(_cubePrefab, cubeCenterPosition, Quaternion.identity);
            newCube.GetComponent<Renderer>().material.color = _randomColor;                   
            newCube._currentChanceOfDivision = _currentChanceOfDivision;
        }
    }

    private void TossNewCubes(Collider[] colliders, Vector3 cubeCenterPosition, float force, float currentExplosionRadius)
    {
        foreach (Collider hit in colliders)
        {
            Rigidbody rigidBody = hit.GetComponent<Rigidbody>();

            if (rigidBody != null)
            {
                rigidBody.AddExplosionForce(force, cubeCenterPosition, currentExplosionRadius, _upwardsModifier, ForceMode.Force);
            }
        }
    }
}
