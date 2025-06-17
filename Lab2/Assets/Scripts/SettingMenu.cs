using UnityEngine;
using UnityEngine.UI;
public class SettingMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button soundButton;
    public Button musicButton;

    private bool isSoundOn = true;
    private bool isMusicOn = true;
    void Start()
    {
        // Đọc giá trị từ PlayerPrefs
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        if (AudioController.instance != null)
        {
            AudioController.instance.SetSound(isSoundOn);
            AudioController.instance.SetMusic(isMusicOn);
        }

        // Thêm listeners cho các nút
        soundButton.onClick.AddListener(ToggleSound);
        musicButton.onClick.AddListener(ToggleMusic);

        // Cập nhật trạng thái UI ban đầu
        UpdateButtonStates();
    }

    void UpdateButtonStates()
    {
        // Cập nhật màu sắc hoặc trạng thái của các nút dựa trên giá trị hiện tại
        soundButton.GetComponent<Image>().color = isSoundOn ? Color.green : Color.red;
        musicButton.GetComponent<Image>().color = isMusicOn ? Color.green : Color.red;

    }

    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        if (AudioController.instance != null)
        {
            AudioController.instance.SetSound(isSoundOn);
        }
        UpdateButtonStates();
        Debug.Log("Sound: " + (isSoundOn ? "On" : "Off"));
    }

    void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        if (AudioController.instance != null)
        {
            AudioController.instance.SetMusic(isMusicOn);
        }
        UpdateButtonStates();
        Debug.Log("Music: " + (isMusicOn ? "On" : "Off"));
    }

}
