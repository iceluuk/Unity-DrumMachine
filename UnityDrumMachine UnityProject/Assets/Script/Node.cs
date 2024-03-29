using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class Node : MonoBehaviour
{
    public string oscAddress = "/trigger";
    public float[] floatMesages = {1f};

    OSCTransmitter transmitter;

    public void SetupTransmitter()
    {
        transmitter = gameObject.AddComponent<OSCTransmitter>();

        transmitter.RemoteHost = "127.0.0.1";
        transmitter.RemotePort = 57121;
    }

    public void SendOSCMessage()
    {
        var message = new OSCMessage(oscAddress);

        foreach(float floatMessage in floatMesages){
            message.AddValue(OSCValue.Float(floatMessage));
        }

        transmitter.Send(message);
    }
}
