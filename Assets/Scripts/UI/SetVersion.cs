using System;
using TMPro;
using UnityEngine;

public class SetVersion : MonoBehaviour
{

    private void Start()
    {
        GetComponent<TMP_Text>().text = Application.version;
    }
}
