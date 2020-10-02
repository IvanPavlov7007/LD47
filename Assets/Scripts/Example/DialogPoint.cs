using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPoint : MonoBehaviour
{
    public Dialog whichDialogToSay;
    private void OnTriggerEnter(Collider other)
    {
        DialogManager.instance.ShowDialog(whichDialogToSay);
    }
}
