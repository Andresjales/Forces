using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forces : MonoBehaviour
{
    Vector3 displacement;

    [Header("World Limits")]
    [SerializeField] float xBorder;
    [SerializeField] float yBorder;

    [Header("Vectors")]
    [SerializeField] Vector3 velocity;
    Vector3 acceleration;

    [Header("Forces")]
    [SerializeField] float mass;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float frictionCoefficient;
    [SerializeField] [Range(0, 1)] float speedLose = 0.9f;
    Vector3 weight;

    [Header("Fluids")]
    [SerializeField] bool fluid;
    [SerializeField] float dragCoefficient;
    float frontalArea;

    [Header("Gravity Atraction")]
    [SerializeField] bool atraction;
    [SerializeField] float g;
    [SerializeField] GameObject otherStar;
    Forces m2;
    Vector3 starDistance;

    private void Awake()
    {
        weight = new Vector3(0, mass * gravity, 0);
        frontalArea = transform.localScale.x;
        starDistance = otherStar.transform.position - transform.position;
        m2 = otherStar.GetComponent<Forces>();
    }

    void Update()
    {
        if(atraction)
        {
            GravityAtraction();
        }
        else
        {
            ApplyForce(-weight);
            if (fluid)
            {
                FluidResistance();
            }
            else
            {
                CalculateFriction();
            }

            CheckCollisions();
        }
        Moving();

        acceleration = Vector3.zero;
    }

    public void Moving()
    {
        velocity += acceleration * Time.deltaTime;
        displacement = velocity * Time.deltaTime;
        transform.position = transform.position + displacement;

        acceleration.Draw(transform.position, Color.green);
        velocity.Draw(transform.position, Color.red);
        transform.position.Draw(Color.blue);
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    public void CalculateFriction()
    {
        if (transform.position.y <= 0)
        {
            Vector3 friction = -frictionCoefficient * weight.magnitude * velocity.normalized;
            ApplyForce(friction);
        }
    }

    public void FluidResistance()
    {
        if(transform.position.y <= 0)
        {
            Vector3 fluidRes = -0.5f * Mathf.Pow(velocity.magnitude, 2) * frontalArea * dragCoefficient * velocity.normalized;
            ApplyForce(fluidRes);
        }
    }

    public void GravityAtraction()
    {
        Vector3 gAtraction = (g * mass * m2.mass * starDistance.normalized) / Mathf.Pow(starDistance.magnitude, 2);
        ApplyForce(gAtraction);
    }

    private void CheckCollisions()
    {
        if(transform.position.x >= xBorder || transform.position.x <= -xBorder)
        {
            if(transform.position.x >= xBorder)
            {
                transform.position = new Vector3(xBorder, transform.position.y, 0);
            }
            else if(transform.position.x <= -xBorder)
            {
                transform.position = new Vector3(-xBorder, transform.position.y, 0);
            }

            velocity.x *= -1 * speedLose;
        }
        else if(transform.position.y >= yBorder || transform.position.y <= -yBorder)
        {
            if (transform.position.y >= yBorder)
            {
                transform.position = new Vector3(transform.position.x, yBorder, 0);
            }
            else if (transform.position.y <= -yBorder)
            {
                transform.position = new Vector3(transform.position.x, -yBorder, 0);
            }

            velocity.y *= -1 * speedLose;
        }
    }
}
