using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinisher : MonoBehaviour {

    [SerializeField]
    GameObject goldCharacter;
    [SerializeField]
    GameObject plantCharacter;

    void Update()
    {
        if(goldCharacter.transform.position.x >= transform.position.x && goldCharacter.activeSelf)
            StartCoroutine("WaitForReset");

        else if (plantCharacter.transform.position.x >= transform.position.x && goldCharacter.activeSelf)
            StartCoroutine("WaitForEnd");
    }

    IEnumerator WaitForReset()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        plantCharacter.SetActive(true);
        Camera.main.GetComponent<CameraMovement>().ResetCamera();
        goldCharacter.SetActive(false);
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        //HAY QUE CONFIGURAR LOS BUILD SETTINGS PARA LAS ESCENAS
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1, LoadSceneMode.Additive);
    }
}
