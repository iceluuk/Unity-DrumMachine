using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    [System.Serializable]
    public class Note{
        public float[] notesInStep;
    }

    [SerializeField]
    public Note[] noteSequence;
    private int currentStep = 0;
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

        Note step = noteSequence[currentStep];
        float[] newMesages = new float[step.notesInStep.Length];
        int noteIndex = 0;
        foreach(float note in step.notesInStep){
            newMesages[noteIndex] = note;
            noteIndex++;
        }

        floatMesages = newMesages;

        currentStep = (currentStep + 1) % noteSequence.Length;

        SendOSCMessage();
        hasBeenActivated = true;
    }

    public void Reset(){
        hasBeenActivated = false;
    }
}
