using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameText : MonoBehaviour
{
    private TextMeshProUGUI nameText;

    private void Start()
    {
        nameText = GetComponent<TextMeshProUGUI>();

        if (AuthManager.User != null)
        {
            nameText.text = $"Hi! {AuthManager.User.Email}";
        }
        else
        {
            nameText.text = "ERROR : AuthManager.User == null";
        }
    }
}
