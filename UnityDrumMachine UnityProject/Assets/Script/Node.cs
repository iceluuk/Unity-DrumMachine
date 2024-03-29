using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class Node : MonoBehaviour
{
    public string oscAddress = "/trigger";
    public float[] floatMesages = {1f};

    OSCTransmitter transmitter;
    Camera mainCamera;

    public void SetupTransmitter()
    {
        transmitter = gameObject.AddComponent<OSCTransmitter>();
        mainCamera = FindObjectOfType<Camera>();

        transmitter.RemoteHost = "127.0.0.1";
        transmitter.RemotePort = 57121;
    }

    public void SendOSCMessage()
    {
        // Get camera edges in world space
        Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, mainCamera.nearClipPlane));
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, mainCamera.nearClipPlane));

        float pan = Mathf.InverseLerp(leftEdge.x, rightEdge.x, transform.position.x);

        pan = Mathf.Lerp(-1f, 1f, pan);
        print(pan);

        var message = new OSCMessage(oscAddress);

        message.AddValue(OSCValue.Float(pan));

        foreach(float floatMessage in floatMesages){
            message.AddValue(OSCValue.Float(floatMessage));
        }

        transmitter.Send(message);
    }
}
