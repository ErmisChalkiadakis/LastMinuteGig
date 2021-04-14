using TMPro;
using UnityEngine;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI usernameDisplayTextMesh;

    protected void Awake()
    {
        usernameDisplayTextMesh.text = PlayerPrefs.GetString("Username") + PlayerPrefs.GetString("UsernameSuffix");
    }
}
