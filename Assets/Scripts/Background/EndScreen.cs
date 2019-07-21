using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Button continueBtn;

    void Start()
    {
        continueBtn.onClick.AddListener(LoadBack);
    }

    void LoadBack()
    {
        //Load the game save data
        SceneManager.LoadSceneAsync(2);
    }
}
