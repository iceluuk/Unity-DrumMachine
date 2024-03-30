using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class RhythmNode : Node
{
    public float growthRate = 2f;
    public bool isActive = false;
    public Transform circleTransform;
    private float circleRadius = 0;
    private ParticleSystem effectParticleSystem;

    private void Start() {
        effectParticleSystem = GetComponent<ParticleSystem>();
        if(!isActive) effectParticleSystem.Stop();

        SetupTransmitter();
    }

    void Update()
    {
        DragNode();
        
        if (isActive)
        {
            circleRadius += growthRate * Time.deltaTime;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleRadius);
            foreach (Collider2D collider in colliders)
            {
                Node otherNode = collider.GetComponent<Node>();
                if (otherNode != null && otherNode != this)
                {
                    OneshotNode oneshotNode = otherNode as OneshotNode;
                    if (oneshotNode != null)
                    {
                        oneshotNode.Activate();
                    }
                    PushNode pushNode = otherNode as PushNode;
                    if (pushNode != null)
                    {
                        pushNode.Activate();
                    }

                    RhythmNode rhythmNode = otherNode as RhythmNode;
                    if (rhythmNode != null)
                    {
                        StopActivation();
                        rhythmNode.StartActivation();

                        OneshotNode[] osNodes =  FindObjectsOfType<OneshotNode>();
                        foreach(OneshotNode oneshotNodeToReset in osNodes){
                            oneshotNodeToReset.Invoke("Reset", 0.1f);
                        } 

                        PushNode[] pNodes =  FindObjectsOfType<PushNode>();
                        foreach(PushNode pushNodeToReset in pNodes){
                            pushNodeToReset.Invoke("Reset", 0.1f);
                        } 
                    }
                }

            }
            var particleShape = effectParticleSystem.shape;
            particleShape.radius = circleRadius;

            circleTransform.localScale = new Vector3(circleRadius * 2, circleRadius* 2, 0);
        }
    }

    public void StartActivation()
    {
        isActive = true;
        effectParticleSystem.Play();

        SendOSCMessage();
    }

    public void StopActivation()
    {
        isActive = false;
        effectParticleSystem.Stop();
        circleRadius = 0;
        circleTransform.localScale = Vector3.zero;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }
}


