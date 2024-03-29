using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneshotNode : Node
{
    private bool hasBeenActivated = false;

    private ParticleSystem effectParticleSystem;

    private void Start() {
        SetupTransmitter();

        effectParticleSystem = GetComponent<ParticleSystem>();
        effectParticleSystem.Stop();
    }

    public void Activate()
    {
        if(hasBeenActivated) return;

        effectParticleSystem.Play();
        SendOSCMessage();
        hasBeenActivated = true;
    }

    public void Reset(){
        hasBeenActivated = false;
    }
}
