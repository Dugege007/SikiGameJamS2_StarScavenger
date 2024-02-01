using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipSpawner : MonoBehaviour
{
    public List<Transform> SpawnPoint;
    public List<GameObject> SpaceshipPrefab;
    public List<Material> ColorVatiation;
    public float TimeFromLastSpawn = 0;
    [Range(1, 10)]
    public float Interval = 3;

    public bool RandomShip;
    public bool OneSpawner;

    [SerializeField] int prefabNumber = 0;
    int PrefabMaxCount;

    void Start()
    {
        PrefabMaxCount = SpaceshipPrefab.Count;
    }

    void FixedUpdate()
    {
        TimeFromLastSpawn = TimeFromLastSpawn + Time.deltaTime;

        if (TimeFromLastSpawn >= Interval)
        {
            TimeFromLastSpawn = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        // Set random prefab from list
        if (RandomShip)
        {   prefabNumber = Random.Range(0, PrefabMaxCount); }

        // Spawn SpaceShip, set color variation for sprite and preset animation for engine
        GameObject SpaceShip_0 = Instantiate(SpaceshipPrefab[prefabNumber], SpawnPoint[0].transform.position, gameObject.transform.rotation);
        SpaceShip_0.GetComponent<Renderer>().material = ColorVatiation[0];
        SpaceShip_0.GetComponent<EngineAnimationPreset>().Preset = 1;

        // Spawn SpaceShip on all spawn point
        if (!OneSpawner)
        {
            GameObject SpaceShip_1 = Instantiate(SpaceshipPrefab[prefabNumber], SpawnPoint[1].transform.position, Quaternion.identity);
            SpaceShip_1.GetComponent<Renderer>().material = ColorVatiation[1];
            SpaceShip_1.GetComponent<EngineAnimationPreset>().Preset = 2;

            GameObject SpaceShip_2 = Instantiate(SpaceshipPrefab[prefabNumber], SpawnPoint[2].transform.position, Quaternion.identity);
            SpaceShip_2.GetComponent<Renderer>().material = ColorVatiation[2];
            SpaceShip_2.GetComponent<EngineAnimationPreset>().Preset = 3;

            GameObject SpaceShip_3 = Instantiate(SpaceshipPrefab[prefabNumber], SpawnPoint[3].transform.position, Quaternion.identity);
            SpaceShip_3.GetComponent<Renderer>().material = ColorVatiation[3];
            SpaceShip_3.GetComponent<EngineAnimationPreset>().Preset = 4;
        }

        prefabNumber++;

        // Reset prefab number to zero
        if (prefabNumber == PrefabMaxCount)
        {
            prefabNumber = 0;
        }
    }
}
