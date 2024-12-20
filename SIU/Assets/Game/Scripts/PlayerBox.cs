using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTextField;
    [SerializeField] Button removeBtn;
    [SerializeField] Animator animator;

    public void RemovePlayer() {
        EventsManager.OnRemovedPlayer?.Invoke(nameTextField.text);
        animator.SetTrigger("Disappear");
        Destroy(gameObject, 0.2f);
    }

    public void SetName(string name)
    {
        nameTextField.text = name;
    }
}
