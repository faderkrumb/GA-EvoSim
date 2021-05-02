using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private float age = 0;
    Rigidbody RB;
    SphereCollider SC;

    public void InsantiateFood(float bounds, Color bruh)
    {
        float posX = Random.Range(-bounds, bounds);
        float posZ = Random.Range(-bounds, bounds);


        gameObject.transform.position = new Vector3(posX, 1, posZ);
        gameObject.GetComponent<Renderer>().material.color = bruh;
        gameObject.tag = "Food";

        RB = gameObject.AddComponent<Rigidbody>();
        RB.isKinematic = true;
        RB.useGravity = false;

        SC = gameObject.AddComponent<SphereCollider>();
        SC.isTrigger = true;

    }

    private void Update()
    {
        AgeFood();
    }

    private void AgeFood()
    {
        age += Time.fixedDeltaTime;

        if (age > 300)
        {
            KillFood();
        }
    }

    public void KillFood()
    {
        FoodManager.foodList.Remove(gameObject);
        Destroy(gameObject);
    }
}

