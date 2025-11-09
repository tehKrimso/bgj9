using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotViewControls : MonoBehaviour
{
    public GameObject EmptyPot;
    public GameObject FullPot;

    public void SetFullPot()
    {
        EmptyPot.SetActive(false);
        FullPot.SetActive(true);
    }

    public void SetEmptyPot()
    {
        EmptyPot.SetActive(true);
        FullPot.SetActive(false);
    }

    public void DisablePots()
    {
        EmptyPot.SetActive(false);
        FullPot.SetActive(false);
    }
}
