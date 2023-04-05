using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using shared;
using UnityEngine;

public class NetworkCheck : MonoBehaviour
{
    private TcpClient _client;
    private TcpMessageChannel _channel;
    
    [SerializeField] private int port = 55555;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Connect();
        }
    }


    public void Connect()
    {
        _channel = new TcpMessageChannel();
        try
        {
            _channel.Connect("192.168.137.1", port);
            HostJoinRequest hostJoinRequest = new HostJoinRequest();
            _channel.SendMessage(hostJoinRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
