using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject StatsGO;
    private Statistics Stats;

    public float TimeMultiplier = 1;
    public int PredatorInitPopulation;
    public int DroneInitPopulation;
    public int FoodInitAmount;
    public float FoodSpawnRate;
    public int Bounds;


    private GameObject predatorManagerGO;
    private GameObject droneManagerGO;
    private GameObject foodManagerGO;

    private PredatorManagerV2 predatorManager;
    private DroneManagerV2 droneManager;
    private FoodManager foodManager;




    void Start()
    {
        StatsGO = new GameObject();
        StatsGO.name = "Statistics";
        Stats = StatsGO.AddComponent<Statistics>();
        Stats.InstantiateStatistics(TimeMultiplier);

        predatorManagerGO = new GameObject();
        predatorManagerGO.name = "predatorManager";
        predatorManager = predatorManagerGO.AddComponent<PredatorManagerV2>();
        predatorManager.InstantiateManager(TimeMultiplier, StatsGO, PredatorInitPopulation, Bounds);


        droneManagerGO = new GameObject();
        droneManagerGO.name = "DroneManager";
        droneManager = droneManagerGO.AddComponent<DroneManagerV2>();
        droneManager.InstantiateManager(TimeMultiplier, StatsGO, DroneInitPopulation, Bounds);

        Stats.SetManagers(predatorManager, droneManager);
        predatorManager.SetDroneManager(droneManager);
        droneManager.SetPredatorManager(predatorManager);

        foodManagerGO = new GameObject();
        foodManager = foodManagerGO.AddComponent<FoodManager>();
        foodManager.SetValues(TimeMultiplier, FoodInitAmount, FoodSpawnRate, Bounds);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
