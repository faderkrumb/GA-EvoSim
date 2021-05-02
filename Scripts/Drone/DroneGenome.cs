using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneGenome
{
    public Gene speed;
    public Gene maxAge;
    public Gene energyEfficiency;
    public Gene sightRange;

    public DroneGenome(DroneV2 d)
    {
        DroneGenome d1 = d.motherGenome;
        DroneGenome d2 = d.fatherGenome;


        if (Random.Range(0f, 1f) > 0.5f)
        {
            this.speed = new Gene(d1.speed);
        }
        else
        {
            this.speed = new Gene(d2.speed);
        }

        if (Random.Range(0f, 1f) > 0.5f)
        {
            this.maxAge = new Gene(d1.maxAge);
        }
        else
        {
            this.maxAge = new Gene(d2.maxAge);
        }

        if (Random.Range(0f, 1f) > 0.5f)
        {
            this.energyEfficiency = new Gene(d1.energyEfficiency);
        }
        else
        {
            this.energyEfficiency = new Gene(d2.energyEfficiency);
        }

        if (Random.Range(0f, 1f) > 0.5f)
        {
            this.sightRange = new Gene(d1.sightRange);
        }
        else
        {
            this.sightRange = new Gene(d2.sightRange);
        }
    }

    public DroneGenome()
    {
        this.speed = new Gene();
        this.maxAge = new Gene();
        this.energyEfficiency = new Gene();
        this.sightRange = new Gene();
    }
}


