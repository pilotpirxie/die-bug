using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image _image;
    
    [SerializeField] private bool fadeOut = false;

    private void Start()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
    }

    private void FixedUpdate()
    {
        if (fadeOut)
        {
            
            if (_image.color.a > 0)
            {
                _image.color = new Color(0, 0, 0, _image.color.a - 0.01f);
            }
        }
        else
        {
            if (_image.color.a < 255)
            {
                _image.color = new Color(0, 0, 0, _image.color.a + 0.01f);
            }
        }
    }
}