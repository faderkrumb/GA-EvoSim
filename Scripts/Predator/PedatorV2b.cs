using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorV2b : MonoBehaviour
{
    private float timeMult;

    private GameObject statObject;
    private Statistics stats;

    private PredatorManagerV2 predatorManager;
    private GameObject droneManagerGO;
    private DroneManagerV2 droneManager;

    [HideInInspector]
    private Vector3 destination = new Vector3(0, 1, 0);
    private Vector3 v3Force = new Vector3(0, 0, 0);

    [SerializeField]
    private GameObject dClose;
    private GameObject targetMate;


    public PredatorGenome motherGenome;
    public PredatorGenome fatherGenome;

    //Functional values
    [HideInInspector]
    public bool female;
    public float age, hunger, horny;
    private float hornyLim = 120;

    //Genomic values
    [HideInInspector]
    public float maxAge, speed, energyEfficiency, sightRange;



    public void SetGenome(PredatorGenome motherGenom, PredatorGenome fatherGenome)
    {
        this.motherGenome = motherGenom;
        this.fatherGenome = fatherGenome;
    }

    public void InstantiatePredator(float timeMult, GameObject statObject, PredatorManagerV2 predatorManager, DroneManagerV2 droneManager)
    {
        this.timeMult = timeMult;
        this.predatorManager = predatorManager;
        this.droneManager = droneManager;
        this.statObject = statObject;
        stats = statObject.GetComponent<Statistics>();
        if (Random.Range(0f, 1f) > 0.5f)
        {
            female = true;
        }
        CalculateValues();
        InvokeRepeating("FindDrone", 0f, 10f / timeMult);
        InvokeRepeating("FindMate", 5f / timeMult, 3f / timeMult);
    }

    private void CalculateValues()
    {
        speed = motherGenome.speed.GetValue(fatherGenome.speed) / 10 * timeMult;
        maxAge = motherGenome.maxAge.GetValue(fatherGenome.maxAge) * 600 / timeMult;
        energyEfficiency = motherGenome.energyEfficiency.GetValue(fatherGenome.energyEfficiency);
        sightRange = motherGenome.sightRange.GetValue(fatherGenome.sightRange) * 100;
        SetRandomDestination();

    }

    void Start()
    {

    }

    void Update()
    {
        TimeStuff();
        Walk();
        AttackDrone();
        if (dClose != null)
        {
            dClose.GetComponent<Renderer>().material.color = new Color(255, 0, 255);
        }
    }
    private void TimeStuff()
    {
        /*
        horny += Time.deltaTime * timeMult;
        age += Time.deltaTime;
        hunger += Time.deltaTime * timeMult * (speed * 10 / timeMult) / energyEfficiency;*/

        horny += 1f / 100f * 2f * timeMult;
        age += 1f / 100f;
        hunger += 1f / 100f * timeMult * (speed * 10f / timeMult) / energyEfficiency;

        if (age > maxAge)
        {
            Kill();
            stats.PredatorsKilledByAge += 1;
        }
        if (hunger > 70)
        {
            //Debug.Log("Hunger > 100");
            Kill();
            stats.PredatorsStarved += 1;
        }
    }


    private void Walk()
    {

        if (horny > hornyLim && (horny > hunger * 1.5f || (dClose == null && horny > 100)) && !female)
        {
            if (CheckMateInRange())
            {
                ChaseTargetMate();
                MateWithTargetMate();
                gameObject.transform.position += v3Force * speed;
                return;
            }
        }
        if (horny > hornyLim && horny > hunger * 1.5f && female)
        {
            if (targetMate != null)
            {
                if (CheckMateInRange())
                {
                    ChaseTargetMate();
                    targetMate.GetComponent<PredatorV2b>().targetMate = gameObject;
                    gameObject.transform.position += v3Force * speed;
                    return;
                }
            }
        }
        if (dCloseInSight())
        {
            //Debug.Log("drone in range");
            chaseDClose();
            gameObject.transform.position += v3Force * speed;
            return;
        }
        else //Debug.Log("drone not in range");

        //Mate();

        if (DestinationReached())
        {
            SetRandomDestination();
            //Debug.Log("Random destination");
        }



        gameObject.transform.position += v3Force * speed;

    }


    //movement

    private void CalcV3()
    {
        Vector3 temp = destination - gameObject.transform.position;


        v3Force.x = temp.x;
        v3Force.z = temp.z;
        v3Force.Normalize();
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

    private void FindDrone()
    {
        if (!dCloseInSight())
        {
            float minDist = 1e9F;
            if (Random.Range(0f, 1f) < 0.5f)
            {
                foreach (GameObject d in droneManager.droneList)
                {
                    float dist = Vector3.Distance(gameObject.transform.position, d.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        dClose = d;
                        if (dist < sightRange)
                        {
                            return;
                        }
                    }
                }
            }
            else
            {
                for (int i = droneManager.droneList.Count - 1; i >= 0; i--)
                {
                    GameObject d = droneManager.droneList[i];
                    float dist = Vector3.Distance(gameObject.transform.position, d.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        dClose = d;
                        if (dist < sightRange)
                        {
                            return;
                        }
                    }
                }
            }

        }
    }

    private bool dCloseInSight()
    {
        if (dClose != null)
        {
            if (Vector3.Distance(gameObject.transform.position, dClose.transform.position) <= sightRange)
            {
                return true;
            }
        }
        return false;

    }

    private void chaseDClose()
    {
        destination = dClose.transform.position;
        destination.y = 1;

        CalcV3();
    }

    private void SetRandomDestination()
    {
        float x = Random.Range(-30f, 30f);
        float z = Random.Range(-30f, 30f);

        destination.x = gameObject.transform.position.x + x;
        destination.z = gameObject.transform.position.z + z;
        destination.y = 1;
        Vector3 temp = destination - gameObject.transform.position;


        v3Force.x = temp.x;
        v3Force.z = temp.z;
        v3Force.Normalize();
    }

    private void AttackDrone()
    {
        if (dClose != null)
        {
            if (Vector3.Distance(gameObject.transform.position, dClose.transform.position) < 2f)
            {
                //Debug.Log("AttackDrone");
                dClose.GetComponent<DroneV2>().Kill();
                hunger -= 30f;
                stats.DronesKilledByPredators += 1;
                //FindClosestDrone();
                if (!dCloseInSight())
                {
                    Debug.Log("No drone in range, random destination");
                    //SetRandomDestination();
                }
            }
        }

    }

    //reproduction


    private bool CheckMateInRange()
    {
        if (targetMate != null)
        {
            if (Vector3.Distance(gameObject.transform.position, targetMate.transform.position) <= sightRange * 3)
            {
                targetMate.GetComponent<PredatorV2b>().targetMate = gameObject;
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

    private void Sex(GameObject father)
    {
        int children = Random.Range(1, 4);
        for (int i = 0; i < children; i++)
        {
            predatorManager.SpawnPredator(gameObject, father);
        }
        horny -= horny;
        targetMate = null;
    }

    private void MateWithTargetMate()
    {
        if (targetMate != null)
        {
            if (Vector3.Distance(gameObject.transform.position, targetMate.transform.position) < 2f)
            {
                targetMate.GetComponent<PredatorV2b>().horny -= 0;
                targetMate.GetComponent<PredatorV2b>().targetMate = null;
                Sex(targetMate);
            }
        }

    }

    private void FindMate()
    {
        if (horny > hornyLim && female)
        {
            float minDist = 1e9f;
            foreach (GameObject p in predatorManager.predatorList)
            {
                if (p == gameObject)
                {
                    continue;
                }
                PredatorV2b predator = p.GetComponent<PredatorV2b>();
                if (!predator.female && predator.horny > hornyLim)
                {
                    float pDist = Vector3.Distance(gameObject.transform.position, p.transform.position);
                    if (pDist < minDist)
                    {
                        minDist = pDist;
                        targetMate = p;
                    }
                }
            }
        }
    }

    private void Kill()
    {
        //Debug.Log("kill called");
        predatorManager.predatorList.Remove(gameObject);
        Destroy(gameObject);
        stats.PredatorsKilledTotal += 1;
    }

}

