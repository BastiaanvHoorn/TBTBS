using UnityEngine;
using System.Collections;

public class Camera_controller : MonoBehaviour {

    // Use this for initialization
    public new Rigidbody rigidbody;
    public new Camera camera;
    [Range(0.1F, 20.0F)]
    public float zoom_speed;
    [Range(0.01F, 1.0F)]
    public float move_speed;
    [Range(0.01F, 1.0F)]
    public float rotation_speed;
    private Vector3 direction;
    private Vector3 old_mouse_pos;
    private Vector3 rotate_vector;
    
    private bool left { get { return Input.GetKey(KeyCode.LeftArrow); } }
    private bool right { get { return Input.GetKey(KeyCode.RightArrow); } }
    private bool up { get { return Input.GetKey(KeyCode.UpArrow); } }
    private bool down { get { return Input.GetKey(KeyCode.DownArrow); } }

    void Start() {
    }

    // Update is called once per frame
    void Update() {
        move_camera();
        zoom_camera();
    }

    private void zoom_camera()
    {
        //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        camera.transform.localPosition += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel")) * zoom_speed;
        
        //Debug.Log(camera.transform.position);
    }
    private void move_camera()
    {
        if(Input.GetMouseButton(1))
        {
            if (Input.GetMouseButtonDown(1))
            {
                old_mouse_pos = Input.mousePosition;
                rotate_vector= transform.rotation.eulerAngles;
            }
            else
            { 

                float diff = old_mouse_pos.x - Input.mousePosition.x;
                this.transform.rotation = Quaternion.Euler(rotate_vector.x, rotate_vector.y - diff* rotation_speed *.2f, rotate_vector.z);
            }
        }
        if (Input.GetMouseButton(2))
        {
            //In the frame that the middle mouse button is pressed don't actually move the camera
            //Only retrieve the mouse position so we can move according to that position next frame
            if (Input.GetMouseButtonDown(2))
            {
                old_mouse_pos = Input.mousePosition;
            }
            else
            {
                Vector3 diff = old_mouse_pos - Input.mousePosition;
                this.gameObject.transform.position += transform.right * diff.x * move_speed *.2f;
                gameObject.transform.position += Quaternion.AngleAxis(-90f, Vector3.up) * transform.right * diff.y * move_speed * .2f;
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
            else if (up && down)
            {
                direction = new Vector3();
            }
            else if (up)
            {
                direction = new Vector3(0, 0, 1);
            }
            else if (down)
            {
                direction = new Vector3(0, 0, -1);
            }
            //All key-combinations that should do nothing
            else if (!left && !right && !up && !down ||
                left && right && up && down ||
                left && right && !up && !down ||
                !left && !right && up && down)
            {
                direction = new Vector3();
            }
            rigidbody.velocity = direction * move_speed;
        }
    }
    /// <summary>
    /// Returns the a vector that points the right way according to combination of the up and the down key
    /// </summary>
    /// <param name="right">if the camera should move right(1) or left(-1)</param>
    /// <returns></returns>
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
