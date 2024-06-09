using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button clientBtn;


    void Awake()
    {
        serverBtn.onClick.AddListener(() => {NetworkManager.Singleton.StartServer();});
            
        HostBtn.onClick.AddListener(() => {NetworkManager.Singleton.StartHost();});

        clientBtn.onClick.AddListener(() => {NetworkManager.Singleton.StartClient();});
    }
}
