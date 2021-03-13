using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToastManager : MonoBehaviour
{
    public static ToastManager instance;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField]
    private Text toastText;
    public static void Toast(string contents)
    {
        instance.toastText.gameObject.SetActive(false);
        instance.toastText.text = contents;
        instance.toastText.gameObject.SetActive(true);
    }
}
