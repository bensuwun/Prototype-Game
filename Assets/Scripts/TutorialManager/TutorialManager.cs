using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
    public GameObject tutorialPanel;
    public List<GameObject> tutorialPages;
    public Button prevBtn;
    public Button nextBtn;
    public Button startGameBtn;
    public GameObject customTyper;
    public GameObject modifiers;
    private int currentPageIndex;

    void Start() {
        currentPageIndex = 0;
        prevBtn.enabled = false;
        startGameBtn.enabled = false;

        startGameBtn.onClick.AddListener(StartGame);
        prevBtn.onClick.AddListener(SetPrevPage);
        nextBtn.onClick.AddListener(SetNextPage);
    }

    void SetNextPage () {
        tutorialPages[currentPageIndex].SetActive(false);
        currentPageIndex += 1;
        tutorialPages[currentPageIndex].SetActive(true);
        if (currentPageIndex == tutorialPages.Count - 1) {
            nextBtn.enabled = false;
            startGameBtn.enabled = true;
        }
            
        if (currentPageIndex != 0)
            prevBtn.enabled = true;
    }

    void SetPrevPage() {
        tutorialPages[currentPageIndex].SetActive(false);
        currentPageIndex -= 1;
        tutorialPages[currentPageIndex].SetActive(true);
        if (currentPageIndex == 0)
            prevBtn.enabled = false;
        if (currentPageIndex < tutorialPages.Count - 1)
            nextBtn.enabled = true;
    }

    void StartGame() {
        customTyper.SendMessage("BeginBattle");
        modifiers.SendMessage("BeginDebuffs");
    }
}