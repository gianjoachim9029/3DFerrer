using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject levelClearText;
    public GameObject tryAgainText;
    public GameObject respawnButton;

    [Header("Player")]
    public GameObject player;

    [Header("Fade Effect")]
    public FadeManager fadeManager; // Assign in Inspector

    private Vector3 startPosition;

    void Start()
    {
        if (levelClearText != null) levelClearText.SetActive(false);
        if (tryAgainText != null) tryAgainText.SetActive(false);
        if (respawnButton != null) respawnButton.SetActive(false);

        if (player != null)
            startPosition = player.transform.position;

        LockCursor(true);
    }

    public void ShowLevelClear()
    {
        StartCoroutine(LevelClearSequence());
    }

    private IEnumerator LevelClearSequence()
    {
        if (fadeManager != null)
            yield return fadeManager.FadeIn(); // fade to black

        if (levelClearText != null) levelClearText.SetActive(true);
        Time.timeScale = 0f;
        LockCursor(false);
    }

    public void ShowTryAgain()
    {
        if (tryAgainText != null) tryAgainText.SetActive(true);
        if (respawnButton != null) respawnButton.SetActive(true);
        Time.timeScale = 0f;
        LockCursor(false);
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnSequence());
    }

    private IEnumerator RespawnSequence()
    {
        if (fadeManager != null)
            yield return fadeManager.FadeIn(); // fade to black

        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        if (fadeManager != null)
            yield return fadeManager.FadeOut(); // fade back in

        LockCursor(true);
    }

    private void LockCursor(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
