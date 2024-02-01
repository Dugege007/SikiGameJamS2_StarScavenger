using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Range(1, 10)]
    public float Interval = 2;
    public float TimeToSpawn = 0;
    public float Y_Force = 0f;
    public float X_Force = 0f;

    public List<GameObject> Prefab;

    Transform SpawnPoint;

    private void Start()
    {
        SpawnPoint = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        TimeToSpawn = TimeToSpawn + Time.deltaTime;

        if (TimeToSpawn >= Interval)
        {
            TimeToSpawn = 0;
            Spawn();
        }
    }

    // Spawn game object and set new constant force;
    void Spawn()
    {
        var RandomPrefab = Random.Range(0, Prefab.Capacity);
        var RandomOffset = Random.Range(-1f, 1f);

        Vector3 Offset = new Vector3(0, RandomOffset);
        GameObject NewGameObject = Instantiate(Prefab[RandomPrefab], SpawnPoint.position + Offset, Quaternion.identity);
        NewGameObject.GetComponent<ConstantForce2D>().force = new Vector2(X_Force, Y_Force);
    }
}
