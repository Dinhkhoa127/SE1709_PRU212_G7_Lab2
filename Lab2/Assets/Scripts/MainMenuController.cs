﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ShowInstructions()
    {
        // Tạo scene mới tên là "Instructions" hoặc bật panel
        SceneManager.LoadScene("InstructionScreen");
    }
    public void ShowLeaderboard()
    {
        // Tạo scene mới tên là "Leaderboard" hoặc bật panel
        SceneManager.LoadScene("LeaderBoardScreen");
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit(); // Chỉ hoạt động khi build ra file
    }
    public void BackToMainMenu()
    {
        // Quay lại menu chính
        SceneManager.LoadScene("MainMenuScreen");
    }
    public void ContinuePlay()
    {
        // Quay lại menu chính
        SceneManager.LoadScene("LeaderBoardScreen");
    }
    public void PlayLevel1()
    {
        SaveCurrentLevel("GameScreen1");
        SceneManager.LoadScene("GameScreen1");
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }
    public void PlayLevel3()
    {
        SaveCurrentLevel("GameScreen3");
        SceneManager.LoadScene("GameScreen3");
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }
    public void PlayLevel2()
    {
        SaveCurrentLevel("GameScreen2");
        SceneManager.LoadScene("GameScreen2");
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }
    public void LoadRandomMap()
    {
        // Tên các scene map của bạn
        string[] mapNames = { "GameScreen1", "GameScreen2", "GameScreen3" };
        int randomIndex = Random.Range(0, mapNames.Length); // Random từ 0 đến 2
        SaveCurrentLevel(mapNames[randomIndex]);
        SceneManager.LoadScene(mapNames[randomIndex]);
    }

    // Lưu lại tên màn chơi hiện tại
    public static void SaveCurrentLevel(string sceneName)
    {
        PlayerPrefs.SetString("LastPlayedLevel", sceneName);
        PlayerPrefs.Save();
    }

    // Hàm replay lại màn vừa chơi
    public void ReplayLastLevel()
    {
        string lastLevel = PlayerPrefs.GetString("LastPlayedLevel", "GameScreen1");
        SceneManager.LoadScene(lastLevel);
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }
}
