using UnityEngine;
using UnityEngine.UI;

public class HeadItem : MonoBehaviour
{
    [SerializeField] private Image hair;
    [SerializeField] private Image beard;

    public void Initalize(BodyPartInfo info) {
        EventsManager.OnAssembleBody?.Invoke(hair, beard, info);
    }
}