using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelIntro : MonoBehaviour
{
    public TypewriterDialogue typewriter;
    public GameObject dialoguePanel;
    [TextArea] public string introText;

    private static HashSet<string> shownScenes = new HashSet<string>();

    void Start()
    {
        dialoguePanel.SetActive(false);

        string scene = SceneManager.GetActiveScene().name;

        // If this scene's dialogue was already shown in this session → skip
        if (shownScenes.Contains(scene))
            return;

        // First time in this scene → show dialogue
        shownScenes.Add(scene);
        typewriter.PlayDialogue(introText);
    }
}
