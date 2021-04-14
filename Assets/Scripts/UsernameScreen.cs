using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Random = System.Random;

public class UsernameScreen : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private Button confirmButton;

    private Random random = new Random();

    protected void Awake()
    {
        if (PlayerPrefs.HasKey("Username"))
        {
            ShowIntroScreen();
            return;
        }

        usernameField.Select();
        confirmButton.onClick.AddListener(OnConfirmButtonClickedEvent);
    }

    protected void Update()
    {
        if (string.IsNullOrEmpty(usernameField.text))
        {
            confirmButton.interactable = false;
        }
        else
        {
            confirmButton.interactable = true;
        }
    }

    private void OnConfirmButtonClickedEvent()
    {
        PlayerPrefs.SetString("Username", usernameField.text);
        PlayerPrefs.SetString("UsernameSuffix", GenerateRandomSuffix());

        ShowIntroScreen();
    }

    private void ShowIntroScreen()
    {
        SceneManager.LoadScene("Intro");
    }

    private string GenerateRandomSuffix()
    {
        string suffix = "#";

        for (int i = 0; i < 4; i++)
        {
            int randomInteger = random.Next(0, 10);
            suffix += randomInteger;
        }

        return suffix;
    }
}
