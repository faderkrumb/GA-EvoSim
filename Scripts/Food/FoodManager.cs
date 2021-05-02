using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public float timeMult;

    public static List<GameObject> foodList = new List<GameObject>();

    public float spawnRate;
    public float initSize;
    public float bounds;

    public Color bönbär = new Color(255, 255, 0);

    void Start()
    {

    }

    public void SetValues(float timeMult, float initSize, float spawnRate, float bounds)
    {
        this.timeMult = timeMult;
        this.initSize = initSize;
        this.spawnRate = spawnRate;
        this.bounds = bounds;
        for (int i = 0; i < initSize; i++)
        {
            SpawnFood();
        }
        InvokeRepeating("FoodSpawner", 1f, 1f / timeMult);
    }


    private void FoodSpawner()
    {
        for (int i = 0; i < 1000 * spawnRate; i++)
        {
            SpawnFood();
        }
    }

    public static void KillFood(GameObject food)
    {
        food.GetComponent<Food>().KillFood();
    }

    private void SpawnFood()
    {
        GameObject food = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        food.AddComponent<Food>().InsantiateFood(bounds, bönbär);
    }
}
