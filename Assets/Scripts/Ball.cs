using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] PaddleAgent paddleAgent;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag.Equals(Tags.Ground_TAG))
        {
            paddleAgent.BallGrounded();
        }
    }
}
