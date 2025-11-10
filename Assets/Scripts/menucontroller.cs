using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;     // Reference to Main Menu
    public GameObject optionsPanel;      // Reference to Options Menu

    // Called when "Options" is clicked
    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // Called when "Back" is clicked in the Options menu
    public void BackToMainMenu()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // Optional: Called when "Quit" is clicked
    public void QuitGame()
    {
        Debug.Log("Quit pressed!");
        Application.Quit();
    }
}
