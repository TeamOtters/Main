using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryHolder : MonoBehaviour
{       
    private Camera m_camera;
    private Rigidbody m_character;

    public struct Boundary
    {
        public float Down, Up, Left, Right;

        public Boundary(float down, float up, float left, float right)
        {
            Down = down; Up = up; Left = left; Right = right;
        }
    }

    public Boundary playerBoundary;
    public Boundary ballBoundary;

    // Use this for initialization
    void Start()
    {
        m_camera = Camera.main;
        m_character = GameController.Instance.player.GetComponentInChildren<Rigidbody>();

        var dist = (m_character.transform.position - m_camera.transform.position).z;

        playerBoundary = new Boundary(m_camera.ViewportToWorldPoint(new Vector3(0, 0, dist)).y,  //down
                                      m_camera.ViewportToWorldPoint(new Vector3(0, 1, dist)).y,  //up
                                      m_camera.ViewportToWorldPoint(new Vector3(0, 0, dist)).x,  //left
                                      m_camera.ViewportToWorldPoint(new Vector3(1, 0, dist)).x); //right  

        ballBoundary = new Boundary  (m_camera.ViewportToWorldPoint(new Vector3(0, 0, dist)).y,  //down
                                      m_camera.ViewportToWorldPoint(new Vector3(0, 1, dist)).y,  //up
                                      m_camera.ViewportToWorldPoint(new Vector3(0, 0, dist)).x,  //left
                                      m_camera.ViewportToWorldPoint(new Vector3(1, 0, dist)).x); //right  

        Debug.Log("Boundary Index 0: " + playerBoundary.Down + " should be Down");
        Debug.Log("Boundary Index 1: " + playerBoundary.Up + " should be Up");
        Debug.Log("Boundary Index 2: " + playerBoundary.Left + " should be Left");
        Debug.Log("Boundary Index 3: " + playerBoundary.Right + " should be Right");

    }

    void Update()
    {
        var dist = (m_character.transform.position - m_camera.transform.position).z;

        playerBoundary.Down = m_camera.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;  //down
        playerBoundary.Up = m_camera.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;  //up
        playerBoundary.Left = m_camera.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;  //left
        playerBoundary.Right = m_camera.ViewportToWorldPoint(new Vector3(1, 0, dist)).x; //right  

    }
}
