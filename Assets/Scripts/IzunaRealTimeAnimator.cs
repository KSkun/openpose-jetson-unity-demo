using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using static OpenPoseUtils;

public class IzunaRealTimeAnimator : MonoBehaviour
{
    public GameObject lHand1, lHand2, rHand1, rHand2;
    public GameObject lLeg1, lLeg2, rLeg1, rLeg2;
    public GameObject head;

    public string address = "127.0.0.1";
    public int port = 23456;

    private Socket _socket;
    private PoseFilter _filter;

    public void Start()
    {
        var ipe = new IPEndPoint(IPAddress.Parse(address), port);
        _socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _socket.ReceiveTimeout = 1;
        _socket.Connect(ipe);
        if (!_socket.Connected)
        {
            Debug.LogError($"remote {address}:{port} is not available");
            Application.Quit();
        }

        _filter = new PoseFilter(3);
    }

    public void Update()
    {
        var buffer = new byte[144];
        try
        {
            var receiveBytes = _socket.Receive(buffer);
            if (receiveBytes == 0)
            {
                return;
            }
        }
        catch (SocketException)
        {
            return;
        }

        var keypoints = new Vector2[18];
        var reader = new BinaryReader(new MemoryStream(buffer));
        for (var i = 0; i < 18; i++)
        {
            keypoints[i].Set(reader.ReadInt32(), reader.ReadInt32());
        }

        keypoints = _filter.Filter(keypoints);

        if (keypoints[2].x >= 0 && keypoints[3].x >= 0 && keypoints[4].x >= 0 &&
            keypoints[5].x >= 0 && keypoints[6].x >= 0 && keypoints[7].x >= 0)
        {
            var lrShoulder = keypoints[2] - keypoints[5];

            var leftHand1 = keypoints[6] - keypoints[5];
            var leftHand2 = keypoints[7] - keypoints[6];
            var angleLHand1 = -180 - Vec2Angle(lrShoulder, leftHand1);
            var angleLHand2 = -Vec2Angle(leftHand1, leftHand2);

            var rightHand1 = keypoints[3] - keypoints[2];
            var rightHand2 = keypoints[4] - keypoints[3];
            var angleRHand1 = -Vec2Angle(-lrShoulder, rightHand1);
            var angleRHand2 = -Vec2Angle(rightHand1, rightHand2);

            lHand1.transform.localRotation = Quaternion.Euler(0, 0, angleLHand1);
            lHand2.transform.localRotation = Quaternion.Euler(0, 0, angleLHand2);
            rHand1.transform.localRotation = Quaternion.Euler(0, 0, angleRHand1);
            rHand2.transform.localRotation = Quaternion.Euler(0, 0, angleRHand2);
        }

        if (keypoints[8].x >= 0 && keypoints[9].x >= 0 && keypoints[10].x >= 0 &&
            keypoints[11].x >= 0 && keypoints[12].x >= 0 && keypoints[13].x >= 0)
        {
            var lrHip = keypoints[8] - keypoints[11];

            var leftLeg1 = keypoints[12] - keypoints[11];
            var leftLeg2 = keypoints[13] - keypoints[12];
            var angleLLeg1 = -180 - Vec2Angle(lrHip, leftLeg1);
            var angleLLeg2 = -Vec2Angle(leftLeg1, leftLeg2);

            var rightLeg1 = keypoints[9] - keypoints[8];
            var rightLeg2 = keypoints[10] - keypoints[9];
            var angleRLeg1 = -Vec2Angle(-lrHip, rightLeg1);
            var angleRLeg2 = -Vec2Angle(rightLeg1, rightLeg2);

            lLeg1.transform.localRotation = Quaternion.Euler(0, 0, angleLLeg1);
            lLeg2.transform.localRotation = Quaternion.Euler(0, 0, angleLLeg2);
            rLeg1.transform.localRotation = Quaternion.Euler(0, 0, angleRLeg1);
            rLeg2.transform.localRotation = Quaternion.Euler(0, 0, angleRLeg2);
        }

        if (keypoints[0].x >= 0 && keypoints[1].x >= 0 && keypoints[2].x >= 0 && keypoints[5].x >= 0)
        {
            var lrShoulder = keypoints[2] - keypoints[5];
            var neckNose = keypoints[0] - keypoints[1];
            var angleHead = 90 - Vec2Angle(lrShoulder, neckNose);

            head.transform.localRotation = Quaternion.Euler(0, 0, angleHead);
        }
    }
}