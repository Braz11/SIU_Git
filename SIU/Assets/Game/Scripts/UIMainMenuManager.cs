using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject myPanel;
    [SerializeField] TMP_InputField nameTextField;
    [SerializeField] TextMeshProUGUI warningText;
    [SerializeField] Button startGameBtn;
    [SerializeField] Button addPlayerBtn;
    [SerializeField] GameObject playerBoxPrefab;
    [SerializeField] Transform content;

    private void Awake() {
        EventsManager.OnWarningText += ShowWarningText;
        EventsManager.OnAddedPlayer += OnAddedPlayer;

        addPlayerBtn.onClick.AddListener(() => {
            EventsManager.OnTryAddPayer?.Invoke(nameTextField.text);
            nameTextField.text = "";
        });

        startGameBtn.onClick.AddListener(() => {
            EventsManager.OnClickedStartGame?.Invoke();
        });

        nameTextField.onSubmit.AddListener((string name) => {
            EventsManager.OnTryAddPayer?.Invoke(name);
            nameTextField.text = "";
        });
    }

    private void OnDestroy() {
        EventsManager.OnWarningText -= ShowWarningText;
        EventsManager.OnAddedPlayer -= OnAddedPlayer;
    }

    private void Start() {
        EventsManager.OnShowScreen?.Invoke(myPanel.name);
    }

    private void ShowWarningText(string text) {
        warningText.text = text;
    }

    private void OnAddedPlayer(string name) {
        GameObject playerBox = Instantiate(playerBoxPrefab, content);
        playerBox.GetComponent<PlayerBox>().SetName(name);
    }
}
