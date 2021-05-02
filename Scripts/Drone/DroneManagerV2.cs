using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManagerV2 : MonoBehaviour
{
    private GameObject stats;
    private PredatorManagerV2 predatorManager;

    public List<GameObject> droneList = new List<GameObject>();

    private float timeMult;
    private int initSize;
    private int bounds;

    private static Color GREEN = new Color(0, 255, 0);

    void Start()
    {

    }

    void Update()
    {

    }

    public void InstantiateManager(float timeMult, GameObject stats, int initSize, int bounds)
    {
        this.stats = stats;
        this.timeMult = timeMult;
        this.initSize = initSize;
        this.bounds = bounds;
        InitSpawn();
    }

    public void SetPredatorManager(PredatorManagerV2 predatorManager)
    {
        this.predatorManager = predatorManager;
        InitSpawn();
    }


    private void InitSpawn()
    {
        for (int i = 0; i < initSize; i++)
        {
            GameObject drone = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            drone.transform.localScale = new Vector3(1, 1, 1);
            drone.transform.position = new Vector3(Random.Range((float)-bounds, (float)bounds), 1, Random.Range((float)-bounds, (float)bounds));
            drone.GetComponent<Renderer>().material.color = GREEN;
            drone.AddComponent<DroneV2>().SetGenome(new DroneGenome(), new DroneGenome());
            drone.GetComponent<DroneV2>().InstantiateDrone(timeMult, stats, gameObject.GetComponent<DroneManagerV2>(), predatorManager);
            droneList.Add(drone);
        }
    }

    public void SpawnDrone(GameObject mother, GameObject father)
    {
        GameObject drone = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        drone.transform.localScale = new Vector3(1, 1, 1);
        drone.transform.position = mother.transform.position;
        //        predator.transform.position = new Vector3(Random.Range(-20, 20), 1, Random.Range(-20, 20));
        drone.GetComponent<Renderer>().material.color = GREEN;
        drone.AddComponent<DroneV2>().SetGenome(new DroneGenome(mother.GetComponent<DroneV2>()), new DroneGenome(father.GetComponent<DroneV2>()));
        drone.GetComponent<DroneV2>().InstantiateDrone(timeMult, stats, gameObject.GetComponent<DroneManagerV2>(), predatorManager);
        droneList.Add(drone);
    }
}
