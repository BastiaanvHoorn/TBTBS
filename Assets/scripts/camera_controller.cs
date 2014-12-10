using UnityEngine;
using System.Collections;

public class camera_controller : MonoBehaviour {

	// Use this for initialization
    public new Rigidbody rigidbody;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody.velocity = new Vector3(0, 0, 10);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rigidbody.velocity = new Vector3(0, 0, -10);
        }
        else
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0); ;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.velocity = new Vector3(-10, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.velocity = new Vector3(10, 0, 0);
        }
        else
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, rigidbody.velocity.z);
        }
	}
}
