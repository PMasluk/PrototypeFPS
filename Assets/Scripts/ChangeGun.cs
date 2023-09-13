using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGun : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> guns = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DisactiveAllGuns();
            guns[0].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DisactiveAllGuns();
            guns[1].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DisactiveAllGuns();
            guns[2].SetActive(true);
        }
    }

    private void DisactiveAllGuns()
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(false);
        }
    }
}
