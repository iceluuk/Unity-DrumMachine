using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    public float[] noteSequence = {-20, -22, -24, -21};
    private int currentNote = 0;
    private bool hasBeenActivated = false;

    private ParticleSystem effectParticleSystem;

    private void Start() {
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

        effectParticleSystem.Play();

        floatMesages[0] = noteSequence[currentNote];
        currentNote = (currentNote + 1) % noteSequence.Length;

        SendOSCMessage();
        hasBeenActivated = true;
    }

    public void Reset(){
        hasBeenActivated = false;
    }
}
