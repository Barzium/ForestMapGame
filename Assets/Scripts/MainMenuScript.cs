using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] int travelerSceneNum;
    [SerializeField] int guideSceneNum;
    public void TravelerButton() {
        StartCoroutine(LoadYourAsyncScene(travelerSceneNum));
    }

    public void GuideButton() {
        StartCoroutine(LoadYourAsyncScene(guideSceneNum));

    }
    private void GoToScene(int scene) {

    }

    IEnumerator LoadYourAsyncScene(int scene) {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
