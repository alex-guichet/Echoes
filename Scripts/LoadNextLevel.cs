using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextLevel : MonoBehaviour
{
    private Button _button;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        
        _button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        });
    }

}
