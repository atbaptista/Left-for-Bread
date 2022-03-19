using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCredits : MonoBehaviour
{
    public GameObject creditsText;
    private bool _isShown = false;

    public void ShowCreditText()
    {
        _isShown = !_isShown;
        creditsText.SetActive(_isShown);

        //deselect the button after clicking because it wasn't doing that before for some reason...
        GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
}
