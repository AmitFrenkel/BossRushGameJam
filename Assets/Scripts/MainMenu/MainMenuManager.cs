using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private VideoClip loopVideo;
    [SerializeField] private Sprite[] loadingSprites;
    [SerializeField] private Image loadingImage;
    [SerializeField] private VideoPlayer videoPlayer,opening;
    [SerializeField] private GameObject openingRawImage;
    [SerializeField] private Animator buttons;

    private bool loop = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer.frame >= (long) (videoPlayer.frameCount-3) && loop)
        {
            videoPlayer.clip = loopVideo;
            videoPlayer.isLooping = true;
            buttons.Play("Buttons");
            loop = false;
        }
    }

    public void ChangeCanvas(GameObject first, GameObject second)
    {
        first.SetActive(false);
        second.SetActive(true);
    }

    public void StartToLoadScene(int sceneIndex)
    {
        opening.gameObject.SetActive(true);
        openingRawImage.SetActive(true);
        StartCoroutine(OpeningEnumerator(sceneIndex));
    }

    private IEnumerator OpeningEnumerator(int sceneIndex)
    {
        while (opening.frame <= (long) (opening.frameCount-3))
        {
            yield return null;
        }
        loadingImage.gameObject.SetActive(true);
        StartCoroutine(LoadAsync(sceneIndex));
    }
    private IEnumerator LoadAsync(int sceneIndex)
    {
        StartCoroutine(PlayLoading());
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncOperation.isDone)
        {
            asyncOperation.allowSceneActivation = false;
            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private IEnumerator PlayLoading()
    {
        for (int i = 0; i < loadingSprites.Length; i++)
        {
            loadingImage.sprite = loadingSprites[i];
            if (i+1 == loadingSprites.Length)
            {
                i = -1;
            }
            yield return new WaitForSeconds(0.04f);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
