using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroductionScreen : MonoBehaviour
{
    [SerializeField] private AnimatorStateObserver animatorStateObserver;

    protected void Awake()
    {
        animatorStateObserver.AnimatorStateEnteredEvent += OnAnimatorStateEnteredEvent;
    }

    private void OnAnimatorStateEnteredEvent(string stateTag)
    {
        if (stateTag == "IdleOut")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
