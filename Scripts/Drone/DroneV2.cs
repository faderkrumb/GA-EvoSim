using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneV2 : MonoBehaviour
{
    public float timeMult;

    DroneManagerV2 droneManager;
    PredatorManagerV2 predatorManager;
    public GameObject stats;
    public Vector3 destination = new Vector3(0, 1, 0);
    private Vector3 v3Force = new Vector3(0, 0, 0);

    private GameObject fClose;
    private GameObject pClose;
    public GameObject targetMate;
    private Rigidbody RB;
    private SphereCollider scannerSC;


    public DroneGenome motherGenome;
    public DroneGenome fatherGenome;


    //Functional values
    public bool female;
    public float age;
    public float horny;
    public float hunger;


    //Genomic values
    public float maxAge;
    public float speed;
    public float energyEfficiency;
    public float sightRange;

    public void SetGenome(DroneGenome motherGenome, DroneGenome fatherGenome)
    {
        this.motherGenome = motherGenome;
        this.fatherGenome = fatherGenome;
    }

    public void InstantiateDrone(float timeMult, GameObject stats, DroneManagerV2 droneManager, PredatorManagerV2 predatorManager)
    {
        this.timeMult = timeMult;
        this.stats = stats;
        this.droneManager = droneManager;
        this.predatorManager = predatorManager;
        if (Random.Range(0f, 1f) > 0.5f)
        {
            female = true;
        }
        CalculateValues();

        scannerSC = gameObject.AddComponent<SphereCollider>();
        scannerSC.isTrigger = true;
        scannerSC.radius = sightRange;

        //InvokeRepeating("FindClosestDrone", 0f, 10f);
        SetRandomDestination();
        InvokeRepeating("FindMate", 5f / timeMult, 15f / timeMult);
    }

    private void CalculateValues()
    {
        speed = motherGenome.speed.GetValue(fatherGenome.speed) / 10 * timeMult;
        maxAge = motherGenome.maxAge.GetValue(fatherGenome.maxAge) * 500 / timeMult;
        energyEfficiency = motherGenome.energyEfficiency.GetValue(fatherGenome.energyEfficiency);
        sightRange = motherGenome.sightRange.GetValue(fatherGenome.sightRange) * 100;

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        TimeStuff();
    }

    private void TimeStuff()
    {
        /*horny += Time.deltaTime * 2 * timeMult;
        age += Time.deltaTime;
        hunger += Time.deltaTime * timeMult * (speed * 10 / timeMult) / energyEfficiency;*/

        horny += 1f / 100f * 2f * timeMult;
        age += 1f / 100f;
        hunger += 1f / 100f * timeMult * (speed * 10f / timeMult) / energyEfficiency;

        if (age > maxAge)
        {
            Kill();
            stats.GetComponent<Statistics>().DronesKilledByAge += 1;
        }
        if (hunger > 70)
        {
            Kill();
            stats.GetComponent<Statistics>().DronesStarved += 1;
        }
    }

    private void CalcV3()
    {
        Vector3 temp = destination - gameObject.transform.position;


        v3Force.x = temp.x;
        v3Force.z = temp.z;
        v3Force.Normalize();
    }

    private void Walk()
    {
        if (horny > 70 && horny > hunger * 1.5 && !female)
        {
            if (CheckMateInRange())
            {
                ChaseTargetMate();
                MateWithTarGetMate();
                gameObject.transform.position += v3Force * speed;
                return;
            }
        }
        if (horny > 70 && horny > hunger * 1.5 && female)
        {
            if (targetMate != null)
            {
                if (CheckMateInRange())
                {
                    ChaseTargetMate();
                    targetMate.GetComponent<DroneV2>().targetMate = gameObject;
                    gameObject.transform.position += v3Force * speed;
                }

            }
        }
        if (fClose != null && hunger > 25)
        {
            destination = fClose.transform.position;
            CalcV3();
            EatFood();
        }
        if (DestinationReached())
        {
            SetRandomDestination();
        }
        gameObject.transform.position += v3Force * speed;
    }

    private void EatFood()
    {
        if (Vector3.Distance(gameObject.transform.position, fClose.transform.position) < 3)
        {
            fClose.GetComponent<Food>().KillFood();
            hunger = 0;
        }
    }

    private bool DestinationReached()
    {
        float posX = gameObject.transform.position.x;
        float posZ = gameObject.transform.position.z;
        float destX = destination.x;
        float destZ = destination.z;
        if (Mathf.Abs(posX - destX) < 3)
        {
            if (Mathf.Abs(posZ - destZ) < 3)
            {
                //Debug.Log("Destination Reached");
                return true;
            }

        }

        return false;
    }

    private void SetRandomDestination()
    {
        float x = Random.Range(-20, 20);
        float z = Random.Range(-20, 20);

        destination.x = gameObject.transform.position.x + x;
        destination.z = gameObject.transform.position.z + z;
        destination.y = 1;
        CalcV3();
    }


    //reproduction
    private bool CheckMateInRange()
    {
        if (targetMate != null)
        {
            if (Vector3.Distance(gameObject.transform.position, targetMate.transform.position) <= sightRange)
            {
                targetMate.GetComponent<DroneV2>().targetMate = gameObject;
                return true;
            }
        }
        return false;
    }

    private void ChaseTargetMate()
    {
        if (targetMate != null)
        {
            destination = targetMate.transform.position;
            destination.y = 1;

            CalcV3();
        }

    }

    public void Sex(GameObject father)
    {
        int children = Random.Range(1, 4);
        for (int i = 0; i < children; i++)
        {
            droneManager.SpawnDrone(gameObject, father);
        }
        horny -= horny;
        targetMate = null;
    }

    private void MateWithTarGetMate()
    {
        if (targetMate != null)
        {
            if (Vector3.Distance(gameObject.transform.position, targetMate.transform.position) < 2f)
            {
                horny = 0;
                targetMate.GetComponent<DroneV2>().Sex(gameObject);
                Debug.Log("MateCalled");
            }
        }

    }

    private void FindMate()
    {
        if (horny > 70 && !female)
        {
            //Debug.Log("FindMate");
            float minDist = 1e9f;
            foreach (GameObject d in droneManager.droneList)
            {
                if (d == gameObject)
                {
                    continue;
                }
                DroneV2 drone = d.GetComponent<DroneV2>();
                if (drone.female && drone.horny > 70)
                {
                    float dDist = Vector3.Distance(gameObject.transform.position, d.transform.position);
                    if (dDist < minDist)
                    {
                        minDist = dDist;
                        targetMate = d;
                    }
                }
                /*if (d.GetComponent<DroneV2>().horny > 70)
                {
                    float dDist = Vector3.Distance(gameObject.transform.position, d.transform.position);
                    if (dDist < minDist)
                    {
                        minDist = dDist;
                        targetMate = d;
                    }
                }*/

            }
        }
        if (targetMate != null)
        {
            //Debug.Log("Mate Found");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            if (fClose == null)
            {
                fClose = other.gameObject;
            }
        }
    }

    public void Kill()
    {
        droneManager.droneList.Remove(gameObject);
        Destroy(gameObject);
        stats.GetComponent<Statistics>().DronesKilledTotal += 1;
    }

}
