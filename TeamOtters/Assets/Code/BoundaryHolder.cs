using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryHolder : MonoBehaviour
{

    public Boundary playerBoundary;

    private GameController m_gameController;
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

    // Use this for initialization
    void Start()
    {
        m_gameController = GetComponentInParent<GameController>();
        m_camera = Camera.main;
        m_character = m_gameController.player.GetComponentInChildren<Rigidbody>();

        var dist = (m_character.transform.position - Camera.main.transform.position).z;

        playerBoundary = new Boundary(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y,  //down
                                      Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y,  //up
                                      Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x,  //left
                                      Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x); //right  

        //Debug.Log("Boundary Index 0: " + playerBoundary.Down + " should be Down");
        //Debug.Log("Boundary Index 1: " + playerBoundary.Up + " should be Up");
        //Debug.Log("Boundary Index 2: " + playerBoundary.Left + " should be Left");
        //Debug.Log("Boundary Index 3: " + playerBoundary.Right + " should be Right");

    }

    /*void Update()
    {

        // Update playerbounds with any camera movements

    }*/
}
