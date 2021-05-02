using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    public float timeMult;

    private PredatorManagerV2 predatorManager;
    private DroneManagerV2 droneManager;

    public int DronesAlive;
    public int PredatorsAlive;
    public int Food;

    public int DronesKilledTotal = 0;
    public int DronesKilledByAge = 0;
    public int DronesStarved = 0;
    public int DronesKilledByPredators = 0;

    public float DroneAverageSpeed;
    public float DroneAverageMaxAge;
    public float DroneAverageSightRange;
    public float DroneAverageEnergyEfficiency;

    public float FemalePredators = 0;
    public float MalePredators = 0;

    public int PredatorsKilledTotal = 0;
    public int PredatorsKilledByAge = 0;
    public int PredatorsStarved = 0;

    public float PredatorAverageSpeed;
    public float PredatorAverageMaxAge;
    public float PredatorAverageSightRange;
    public float PredatorAverageEnergyEfficiency;

    //DataWriter dataWriter;

    string file;
    string[] data;
    public void InstantiateStatistics(float timeMult)
    {
        this.timeMult = timeMult;
    }

    public void SetManagers(PredatorManagerV2 predatorManager, DroneManagerV2 droneManager)
    {
        this.predatorManager = predatorManager;
        this.droneManager = droneManager;
        //dataWriter = new DataWriter(timeMult);
        InvokeRepeating("CalcAll", 0f, 10f / timeMult);
        //InvokeRepeating("WriteData", 0, 10f / timeMult);

    }

    private void Start()
    {

    }

    private void WriteData()
    {
        //dataWriter.WriteToFile(CreateDataArray());
    }

    private string[] CreateDataArray()
    {
        string[] data = new string[18];

        data[0] = DronesAlive.ToString();
        data[1] = DronesKilledTotal.ToString();
        data[2] = DronesKilledByAge.ToString();
        data[3] = DronesStarved.ToString();
        data[4] = DronesKilledByPredators.ToString();
        data[5] = DroneAverageSpeed.ToString();
        data[6] = DroneAverageMaxAge.ToString();
        data[7] = DroneAverageSightRange.ToString();
        data[8] = DroneAverageEnergyEfficiency.ToString();

        data[9] = PredatorsAlive.ToString();
        data[10] = PredatorsKilledTotal.ToString();
        data[11] = PredatorsKilledByAge.ToString();
        data[12] = PredatorsStarved.ToString();
        data[13] = PredatorAverageSpeed.ToString();
        data[14] = PredatorAverageMaxAge.ToString();
        data[15] = PredatorAverageSightRange.ToString();
        data[16] = PredatorAverageEnergyEfficiency.ToString();

        data[17] = Food.ToString();

        return data;
    }

    private void Update()
    {

    }

    private void CalcAll()
    {
        DronesAlive = droneManager.droneList.Count;
        PredatorsAlive = predatorManager.predatorList.Count;
        PSpeed();
        PSightRange();
        PMaxAge();
        PEnergyEfficiency();
        DSpeed();
        DSightRange();
        DMaxAge();
        DEnergyEfficiency();
    }

    private void PSpeed()
    {
        float sum = 0;
        foreach (GameObject p in predatorManager.predatorList)
        {
            sum += p.GetComponent<PredatorV2b>().speed / timeMult;
        }
        sum /= PredatorsAlive;
        PredatorAverageSpeed = sum;
    }

    private void PMaxAge()
    {
        float sum = 0;
        foreach (GameObject p in predatorManager.predatorList)
        {
            sum += p.GetComponent<PredatorV2b>().maxAge * timeMult;
        }
        sum /= PredatorsAlive;
        PredatorAverageMaxAge = sum;
    }

    private void PEnergyEfficiency()
    {
        float sum = 0;
        foreach (GameObject p in predatorManager.predatorList)
        {
            sum += p.GetComponent<PredatorV2b>().energyEfficiency;
        }
        sum /= PredatorsAlive;
        PredatorAverageEnergyEfficiency = sum;
    }
    private void PSightRange()
    {
        float sum = 0;
        foreach (GameObject p in predatorManager.predatorList)
        {
            sum += p.GetComponent<PredatorV2b>().sightRange;
        }
        sum /= PredatorsAlive;
        PredatorAverageSightRange = sum;
    }

    private void DSpeed()
    {
        float sum = 0;
        foreach (GameObject d in droneManager.droneList)
        {
            sum += d.GetComponent<DroneV2>().speed / timeMult;
        }
        sum /= DronesAlive;
        DroneAverageSpeed = sum;
    }

    private void DMaxAge()
    {
        float sum = 0;
        foreach (GameObject d in droneManager.droneList)
        {
            sum += d.GetComponent<DroneV2>().maxAge * timeMult;
        }
        sum /= DronesAlive;
        DroneAverageMaxAge = sum;
    }

    private void DEnergyEfficiency()
    {
        float sum = 0;
        foreach (GameObject d in droneManager.droneList)
        {
            sum += d.GetComponent<DroneV2>().energyEfficiency;
        }
        sum /= DronesAlive;
        DroneAverageEnergyEfficiency = sum;
    }
    private void DSightRange()
    {
        float sum = 0;
        foreach (GameObject d in droneManager.droneList)
        {
            sum += d.GetComponent<DroneV2>().sightRange;
        }
        sum /= DronesAlive;
        DroneAverageSightRange = sum;
    }
}
