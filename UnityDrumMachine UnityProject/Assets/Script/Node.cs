using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class Node : MonoBehaviour
{
    public string oscAddress = "/trigger";
    public float[] floatMesages = { 1f };

    OSCTransmitter transmitter;
    Camera mainCamera;

    private bool isDragging = false;
    private Vector3 offset;

    public void SetupTransmitter()
    {
        transmitter = gameObject.AddComponent<OSCTransmitter>();
        mainCamera = FindObjectOfType<Camera>();

        transmitter.RemoteHost = "127.0.0.1";
        transmitter.RemotePort = 57120;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    public void DragNode()
    {
        if (isDragging)
        {
            Vector3 targetPos = GetMouseWorldPos() + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
        }
    }

    public void SendOSCMessage()
    {
        Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, mainCamera.nearClipPlane));
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, mainCamera.nearClipPlane));

        float pan = Mathf.InverseLerp(leftEdge.x, rightEdge.x, transform.position.x);

        pan = Mathf.Lerp(-1f, 1f, pan);

        var message = new OSCMessage(oscAddress);

        message.AddValue(OSCValue.Float(pan));

        foreach (float floatMessage in floatMesages)
        {
            message.AddValue(OSCValue.Float(floatMessage));
        }

        transmitter.Send(message);
    }
}
