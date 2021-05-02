using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorManagerV2 : MonoBehaviour
{
    private GameObject stats;
    private GameObject droneManagerGO;
    private DroneManagerV2 droneManager;

    public List<GameObject> predatorList = new List<GameObject>();



    private float timeMult;
    private int initSize;
    private int bounds;

    private static Color RED = new Color(255, 0, 0);

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
    }

    public void SetDroneManager(DroneManagerV2 droneManager)
    {
        this.droneManager = droneManager;
        InitSpawn();
    }

    private void InitSpawn()
    {
        for (int i = 0; i < initSize; i++)
        {
            GameObject predator = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            predator.transform.localScale = new Vector3(1, 1, 1);
            predator.transform.position = new Vector3(Random.Range((float)-bounds, (float)bounds), 1, Random.Range((float)-bounds, (float)bounds));
            predator.GetComponent<Renderer>().material.color = RED;
            predator.AddComponent<PredatorV2b>().SetGenome(new PredatorGenome(), new PredatorGenome());
            predator.GetComponent<PredatorV2b>().InstantiatePredator(timeMult, stats, gameObject.GetComponent<PredatorManagerV2>(), droneManager);
            predatorList.Add(predator);
        }
    }

    public void SpawnPredator(GameObject mother, GameObject father)
    {
        GameObject predator = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        predator.transform.localScale = new Vector3(1, 1, 1);
        predator.transform.position = mother.transform.position;
        //        predator.transform.position = new Vector3(Random.Range(-20, 20), 1, Random.Range(-20, 20));
        predator.GetComponent<Renderer>().material.color = RED;
        predator.AddComponent<PredatorV2b>().SetGenome(new PredatorGenome(mother.GetComponent<PredatorV2b>()), new PredatorGenome(father.GetComponent<PredatorV2b>()));
        predator.GetComponent<PredatorV2b>().InstantiatePredator(timeMult, stats, gameObject.GetComponent<PredatorManagerV2>(), droneManager);
        predatorList.Add(predator);
    }
}
