using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Snake : MonoBehaviour {
    // VARIABLES //
	//Current movement direction by default moves to the right
	Vector2 dir = Vector2.right;
	//Did the snake eat something?
	bool ate = false; 
	public GameObject tailPrefab;
    //Keep track of tail
    List<Transform> tail = new List<Transform>();


    //======================================================================//

	// Use this for initialization
	void Start () {
		//Move the snake 300ms
		InvokeRepeating ("Move", 0.3f, 0.3f);
	}

    //======================================================================//

    // Update is called once per frame
    void Update () {
		if (Input.acceleration.x > 0) {
			dir = Vector2.right;
		} else if (Input.acceleration.z < 0) {
			dir = -Vector2.up; // -up = down
		} else if (Input.acceleration.x < 0) {
			dir = -Vector2.right; // -right = left
		} else if (Input.acceleration.z > 0) {
			dir = Vector2.up;
		}
    
	}

    //======================================================================//

    void Move(){
		//Save current position
		Vector2 v = transform.position;

		//Do movement stuff.
		transform.Translate(dir);

		if (ate)
        {
            //Load prefab into the world
            GameObject g = GetG(v);

            //Keep track of it in our tail list
            tail.Insert(0, g.transform);

            //Reset the flag
            ate = false;
        }
        //Is there a tail?
        else if (tail.Count > 0) {
			//Move last tail element to where head was
			tail.Last ().position = v;

			//Add to front of list, remove from the back
			tail.Insert (0, tail.Last ());
			tail.RemoveAt (tail.Count-1);
		}
	}

    private GameObject GetG(Vector2 v)
    {
        return Instantiate(tailPrefab, v, Quaternion.identity);
    }

    //======================================================================//

    void OnTriggerEnter2D(Collider2D coll) {
		if (coll.name.StartsWith ("FoodPrefab")) {
			//get longer in next move call
			ate = true;

			//remove the food
			Destroy (coll.gameObject);
		}
		/*
		else {
			//You Lose screen...
		}
		*/
	}
}
