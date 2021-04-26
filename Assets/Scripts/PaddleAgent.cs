using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public static class Tags
{
    public static readonly string BALL_TAG = "Ball";
    public static readonly string PADDLE_TAG = "Paddle";
    public static readonly string Ground_TAG = "Ground";
    public static readonly string Boundary_TAG = "Boundary";
}


public class PaddleAgent : Agent
{
    [SerializeField] Rigidbody ball;
    [SerializeField] float speed = 1f;
    [SerializeField] float InitialForce = 5f;
    [SerializeField] MeshRenderer meshRender;
    [SerializeField] Color winMat;
    [SerializeField] Color loseMat;

    Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = initialPosition;
        ball.transform.localPosition = new Vector3(Random.Range(-5, 5), 45, Random.Range(-5, 5));
        ball.angularVelocity = Vector3.zero;
        ball.velocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //target agent observation
        sensor.AddObservation(ball.transform.localPosition);
        sensor.AddObservation(ball.position);
        sensor.AddObservation(ball.velocity);
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        transform.localPosition += new Vector3(vectorAction[0], 0, vectorAction[1]) * Time.deltaTime * speed;
        if(ball.transform.position.y < -2)
        {
            //AddReward(+5);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals(Tags.BALL_TAG))
        {
            meshRender.material.color = winMat;
            AddReward(1);
            EndEpisode();
        }
    }

    public void BallGrounded()
    {
        meshRender.material.color = loseMat;
        AddReward(-0.1f);
        //EndEpisode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(Tags.Boundary_TAG))
        {
            meshRender.material.color = loseMat;
            AddReward(-1);
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }
}
