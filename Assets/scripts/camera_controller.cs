using UnityEngine;
using System.Collections;

public class camera_controller : MonoBehaviour {

    // Use this for initialization
    public new Rigidbody rigidbody;
    public Transform camera;
    private Vector3 direction;
    private Vector3 old_mouse_pos;
    
    private bool left { get { return Input.GetKey(KeyCode.LeftArrow); } }
    private bool right { get { return Input.GetKey(KeyCode.RightArrow); } }
    private bool up { get { return Input.GetKey(KeyCode.UpArrow); } }
    private bool down { get { return Input.GetKey(KeyCode.DownArrow); } }

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButton(2))
        {
            rigidbody.velocity = new Vector3();
            if (Input.GetMouseButtonDown(2))
            {
                old_mouse_pos = Input.mousePosition;
            }
            else
            {
                Vector3 diff = old_mouse_pos - Input.mousePosition;
                this.gameObject.transform.position += new Vector3(diff.x, 0, diff.y)/25;
                old_mouse_pos = Input.mousePosition;
            }
        }
        else
        {

            if (left && !right)
            {
                direction = up_down_check(-1);
            }
            else if (!left && right)
            {
                direction = up_down_check(1);
            }
            else if (up)
            {
                direction = new Vector3(0, 0, 1);
            }
            else if (down)
            {
                direction = new Vector3(0, 0, -1);
            }
            else if (!left && !right && !up && !down ||
                left && right && up && down ||
                left && right && !up && !down ||
                !left && !right && up && down)
            {
                direction = new Vector3();
            }
            rigidbody.velocity = direction * 15;
        }
    }

    private Vector3 up_down_check(int right)
    {
        if (right != -1 && right != 1)
        {
            Debug.LogError("wrong parameter input");
            return new Vector3();
        }
        else if (up && down || !up && !down)
        {
            return new Vector3(right, 0, 0);
        }
        else if (up && !down)
        {
            return new Vector3(right, 0, 1);
        }
        return new Vector3(right, 0, -1);
    }
}
