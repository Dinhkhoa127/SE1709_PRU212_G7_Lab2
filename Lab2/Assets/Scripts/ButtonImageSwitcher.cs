using UnityEngine;

public class ButtonImageSwitcher : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject imageNormal;
    public GameObject imageActive;
    public bool keepState = false; // Chọn true nếu muốn giữ trạng thái

    void Start()
    {
        if (keepState)
        {
            bool isActive = PlayerPrefs.GetInt(gameObject.name + "_Active", 1) == 1;
            imageNormal.SetActive(!isActive);
            imageActive.SetActive(isActive);
        }
        else
        {
            // Mặc định trạng thái nào đó, ví dụ là bật
            imageNormal.SetActive(true);
            imageActive.SetActive(false);
        }
    }

    public void ToggleImage()
    {
        bool isActive = !imageActive.activeSelf;
        imageNormal.SetActive(!isActive);
        imageActive.SetActive(isActive);

        if (keepState)
        {
            PlayerPrefs.SetInt(gameObject.name + "_Active", isActive ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
