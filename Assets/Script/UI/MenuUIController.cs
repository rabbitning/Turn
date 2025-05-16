using System.Collections;
using UnityEngine;

public class MenuUIController : UIController
{
    [SerializeField] bool _unFocusedAutoPause = true;
    [SerializeField] GameObject _pausePanel = null;
    [SerializeField] GameObject _chipsMenuPanel = null;
    [SerializeField] GameObject _winPanel = null;

    protected override void Start()
    {
        _pausePanel.SetActive(false);
        _chipsMenuPanel.SetActive(false);
        if (_winPanel != null) _winPanel.SetActive(false);
    }

    void Update()
    {
        HandleAutoPause();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // if (Input.GetKeyDown(KeyCode.Tab))
        // {
        //     ToggleChipsMenu();
        // }
    }

    public void TogglePause()
    {
        Time.timeScale = _pausePanel.activeSelf ? 1 : 0;
        _pausePanel.SetActive(!_pausePanel.activeSelf);
    }

    void HandleAutoPause()
    {
        if (!Application.isFocused && !_pausePanel.activeSelf && _unFocusedAutoPause)
        {
            Time.timeScale = 0;
            _pausePanel.SetActive(true);
        }
    }

    public void ToggleChipsMenu()
    {
        if (Time.timeScale == 0) return;

        _chipsMenuPanel.SetActive(!_chipsMenuPanel.activeSelf);

        // if (!_chipsMenuPanel.activeSelf)
        // {
        //     _chipsMenuPanel.GetComponent<ChipsMenuManager>().ApplyChips();
        // }
    }

    public void Win()
    {
        // Time.timeScale = 0;
        StartCoroutine(WaitForWinPanel());
    }

    IEnumerator WaitForWinPanel()
    {
        yield return new WaitForSeconds(0.8f);
        if (_winPanel != null) _winPanel.SetActive(true);
    }
}
