using System;
using UnityEngine;
using UnityEngine.UI;

public class FlashController : MonoBehaviour
{
    [SerializeField] private GameEventChannel eventChannel;
    private Image flashImage;
    private bool IsFlashed;
    private void Start()
    {
        flashImage = GetComponent<Image>();
        eventChannel.OnEvidenceShot += Flash;
    }

    private void Flash(object sender, EventArgs e)
    {
        flashImage.color = Color.white;
        IsFlashed = true;
    }
    
    private void Update()
    {
        if (IsFlashed)
        {
            Color color = flashImage.color;
            color.a -= 0.02f;
            flashImage.color = color;

            if (flashImage.color.a <= 0.01f)
            {
                IsFlashed = false;
            }
        }    
    }
}
