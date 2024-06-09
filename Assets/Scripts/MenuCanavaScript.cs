using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanavaScript : MonoBehaviour
{
    public GameObject MenuCanvaOpen;
    public GameObject MenuCanvaClosed;
    void Start()
    {
        MenuCanvaOpen.SetActive(false);
        MenuCanvaClosed.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuCanvaOpen.SetActive(!MenuCanvaOpen.activeSelf);
            MenuCanvaClosed.SetActive(!MenuCanvaOpen.activeSelf);
        }
    }
}
