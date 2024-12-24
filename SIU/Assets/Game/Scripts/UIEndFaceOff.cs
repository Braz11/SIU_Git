using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.PackageManager;
public class UIEndFaceOff : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;
    [SerializeField] private TMP_Text teamWinText;
    [SerializeField] private Button buttonOption1;
    [SerializeField] private Button buttonOption2;
    [SerializeField] private CTeam.PlayerPositions positionForOption1;
    [SerializeField] private CTeam.PlayerPositions positionForOption2;

    private void Awake() {
        EventsManager.OnShowEndFaceoffScreen += OnShowEndFaceoffScreen;
        buttonOption1.onClick.AddListener(Option1);
        buttonOption2.onClick.AddListener(Option2);
    }
    private void OnDestroy() {
        EventsManager.OnShowEndFaceoffScreen -= OnShowEndFaceoffScreen;
        buttonOption1.onClick.RemoveListener(Option1);
        buttonOption2.onClick.RemoveListener(Option2);
    }

    private void OnShowEndFaceoffScreen(EndFaceoffData data)
    {
        EventsManager.OnShowScreen?.Invoke(myPanel.name);
        if (data.teamNumber == 1) {
            teamWinText.text = "<color=blue>BLUE</color> wins!";
        } else {
            teamWinText.text = "<color=red>RED</color> wins!";
        }
        
    }
   private void Option1()
   {
        EventsManager.OnDefineNewMatchup?.Invoke(positionForOption1);
   }
   private void Option2()
   {
        EventsManager.OnDefineNewMatchup?.Invoke(positionForOption2);
   }
}
