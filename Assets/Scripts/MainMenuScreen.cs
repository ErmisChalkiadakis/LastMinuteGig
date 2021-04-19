using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    private static int SHOW_HASH = Animator.StringToHash("Show");

    [SerializeField] private Animator animator;
    [SerializeField] private MainMenuButton protocolOneButton;
    [SerializeField] private MainMenuButton restartFromTutorialButton;

    protected void Awake()
    {
        protocolOneButton.ButtonSelectedEvent += OnProtocolOneButtonSelectedEvent;
        restartFromTutorialButton.ButtonSelectedEvent += OnRestartFromTutorialButtonSelectedEvent;

        int playerPlayedTutorial = PlayerPrefs.GetInt("PlayedTutorial");

        restartFromTutorialButton.gameObject.SetActive(playerPlayedTutorial > 0);
    }

    protected void OnDestroy()
    {
        protocolOneButton.ButtonSelectedEvent -= OnProtocolOneButtonSelectedEvent;
        restartFromTutorialButton.ButtonSelectedEvent -= OnRestartFromTutorialButtonSelectedEvent;
    }

    private void OnProtocolOneButtonSelectedEvent()
    {
        StartCoroutine(HideAndLoadNextScene());
    }

    private void OnRestartFromTutorialButtonSelectedEvent()
    {
        StartCoroutine(HideAndLoadTutorialScene());
    }

    private IEnumerator HideAndLoadNextScene()
    {
        animator.SetBool(SHOW_HASH, false);

        yield return new WaitForSeconds(.5f);

        int playerPlayedTutorial = PlayerPrefs.GetInt("PlayedTutorial");
        if (playerPlayedTutorial == 0)
        {
            SceneManager.LoadScene("SustainedProtocol Tutorial");
        }
        else
        {
            SceneManager.LoadScene("SustainedProtocol");
        }
    }

    private IEnumerator HideAndLoadTutorialScene()
    {
        animator.SetBool(SHOW_HASH, false);

        yield return new WaitForSeconds(.5f);

        SceneManager.LoadScene("SustainedProtocol Tutorial");
    }
}
