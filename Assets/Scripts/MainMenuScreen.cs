using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    private static int SHOW_HASH = Animator.StringToHash("Show");

    [SerializeField] private Animator animator;
    [SerializeField] private MainMenuButton protocolOneButton;

    protected void Awake()
    {
        protocolOneButton.ButtonSelectedEvent += OnProtocolOneButtonSelectedEvent;
    }

    protected void OnDestroy()
    {
        protocolOneButton.ButtonSelectedEvent -= OnProtocolOneButtonSelectedEvent;
    }

    private void OnProtocolOneButtonSelectedEvent()
    {
        StartCoroutine(HideAndLoadNextScene());
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
}
