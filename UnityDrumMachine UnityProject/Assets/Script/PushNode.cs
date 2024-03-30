using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushNode : Node
{
    [SerializeField]
    private float circleRadius = 2f;
    [SerializeField]
    private float force = 1;
    private bool hasBeenActivated = false;

    private ParticleSystem effectParticleSystem;

    private void Start(){
        SetupTransmitter();

        effectParticleSystem = GetComponent<ParticleSystem>();
        effectParticleSystem.Stop();
    }
    
    private void Update() {
        DragNode();
    }

    public void Activate()
    {
        if(hasBeenActivated) return;
        SendOSCMessage();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleRadius);
        foreach (Collider2D collider in colliders)
        {
            Node otherNode = collider.GetComponent<Node>();
            if (otherNode != null && otherNode != this){
                Rigidbody2D nodeRB = collider.gameObject.GetComponent<Rigidbody2D>();
                Vector3 direction = collider.gameObject.transform.position.normalized * -1 * force;
                nodeRB.AddForce(new Vector2(direction.x, direction.y));
            }
        }

        effectParticleSystem.Play();
        hasBeenActivated = true;
    }

    public void Reset(){
        hasBeenActivated = false;
    }

     void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }
}
