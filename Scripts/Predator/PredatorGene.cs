using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorGenome
{
    public Gene speed;
    public Gene maxAge;
    public Gene energyEfficiency;
    public Gene sightRange;

    public PredatorGenome(PredatorV2b p)
    {
        PredatorGenome g1 = p.motherGenome;
        PredatorGenome g2 = p.fatherGenome;

        if (Random.Range(0f, 1f) > 0.5f)
        {
            this.speed = new Gene(g1.speed);
        }
        else
        {
            this.speed = new Gene(g2.speed);
        }

        if (Random.Range(0f, 1f) > 0.5f)
        {
            this.maxAge = new Gene(g1.maxAge);
        }
        else
        {
            this.maxAge = new Gene(g2.maxAge);
        }

        if (Random.Range(0f, 1f) > 0.5f)
        {
            this.energyEfficiency = new Gene(g1.energyEfficiency);
        }
        else
        {
            this.energyEfficiency = new Gene(g2.energyEfficiency);
        }

        if (Random.Range(0f, 1f) > 0.5f)
        {
            this.sightRange = new Gene(g1.sightRange);
        }
        else
        {
            this.sightRange = new Gene(g2.sightRange);
        }

    }

    public PredatorGenome()
    {
        this.speed = new Gene();
        this.maxAge = new Gene();
        this.energyEfficiency = new Gene();
        this.sightRange = new Gene();
    }
}

public class Gene
{
    private float e = 2.71828f;
    public float value;
    public float dominance;

    public Gene(Gene g)
    {
        value = g.value;
        value = Mutate(value);
        dominance = g.dominance;
        dominance = Mutate(dominance);
    }

    private float Mutate(float x)
    {
        if (Random.Range(0f, 1f) < 0.05f)
        {
            if (Random.Range(0f, 1f) >= 0.5f)
            {
                return x + Random.Range(0.02f, 0.08f);
            }
            else
            {
                return x - Random.Range(0.02f, 0.08f);
            }
        }
        return x;

    }

    public Gene()
    {
        this.value = Random.Range(0.2f, 0.3f);
        this.dominance = Random.Range(0.2f, 0.8f);
    }

    public float GetValue(Gene other)
    {
        if (this.value > other.value)
        {
            return ((this.value + other.value) / 2) * Sigmoid(this.dominance / other.dominance);
        }
        return ((this.value + other.value) / 2) * Sigmoid(other.dominance / this.dominance);
    }

    public float Sigmoid(float x)
    {
        return 0.4f / (1 + Mathf.Pow(e, 0.5f * -x)) + 0.8f;
    }



}