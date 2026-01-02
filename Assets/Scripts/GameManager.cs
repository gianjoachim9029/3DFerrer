using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject levelClearText;
    public GameObject tryAgainText;
    public GameObject respawnButton;
    public GameObject nextLevelButton;

    [Header("Player")]
    public GameObject player;

    [Header("Fade Effect")]
    public FadeManager fadeManager;

    private Vector3 startPosition;
    private StarterAssetsInputs _playerInputs; // ✅ To control the player's cursor logic

    void Start()
    {
        // 1. Hide all UI elements at the start
        if (levelClearText != null) levelClearText.SetActive(false);
        if (tryAgainText != null) tryAgainText.SetActive(false);
        if (respawnButton != null) respawnButton.SetActive(false);
        if (nextLevelButton != null) nextLevelButton.SetActive(false);

        // 2. Setup Player references
        if (player != null)
        {
            startPosition = player.transform.position;
            // ✅ Find the StarterAssetsInputs script so we can disable the cursor lock later
            _playerInputs = player.GetComponent<StarterAssetsInputs>();
        }

        // 3. Lock the cursor for gameplay
        LockCursor(true);
    }

    // Called when player hits the FinishLine
    public void ShowLevelClear()
    {
        StartCoroutine(LevelClearSequence());
    }

    private IEnumerator LevelClearSequence()
    {
        // Fade to black
        if (fadeManager != null)
            yield return fadeManager.FadeIn();

        // Show "Level Cleared" text and "Next Level" button
        if (levelClearText != null) levelClearText.SetActive(true);
        if (nextLevelButton != null) nextLevelButton.SetActive(true);

        // Stop the game and unlock cursor so player can click
        Time.timeScale = 0f;
        LockCursor(false); // ✅ This now forces the player script to release the mouse
    }

    // Called when player hits the DeathZone
    public void ShowTryAgain()
    {
        if (tryAgainText != null) tryAgainText.SetActive(true);
        if (respawnButton != null) respawnButton.SetActive(true);
        
        Time.timeScale = 0f;
        LockCursor(false);
    }

    // Called by Respawn Button
    public void RespawnPlayer()
    {
        StartCoroutine(RespawnSequence());
    }

    private IEnumerator RespawnSequence()
    {
        Debug.Log("RespawnPlayer called");
        if (fadeManager != null)
            yield return fadeManager.FadeIn();

        Time.timeScale = 1f;

        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        if (fadeManager != null)
            yield return fadeManager.FadeOut();

        LockCursor(true);
    }

    // Called by Next Level Button
    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // Unpause time
        
        // Load the next scene in the list (Current Index + 1)
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Check if the next level actually exists to prevent errors
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Loading Main Menu.");
            SceneManager.LoadScene(0); // Go back to first scene if finished
        }
    }

    // ✅ FIXED FUNCTION: Stops StarterAssets from fighting the cursor
    private void LockCursor(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            // Tell Player script: "It's okay to lock the cursor again"
            if (_playerInputs != null)
            {
                _playerInputs.cursorLocked = true;
                _playerInputs.cursorInputForLook = true;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Tell Player script: "STOP locking the cursor!"
            if (_playerInputs != null)
            {
                _playerInputs.cursorLocked = false;
                _playerInputs.cursorInputForLook = false; 
            }
        }
    }
}