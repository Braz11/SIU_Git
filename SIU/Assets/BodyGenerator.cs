using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartInfo
{
    public Color hairColor;
    public Color beardColor;
    public Color skinColor;

    public int hairIndex = -1;
    public int beardIndex = -1;
}

public class BodyGenerator : MonoBehaviour
{
    [SerializeField] List<Color> hairColors;
    [SerializeField] List<Color> beardColors;
    [SerializeField] List<Color> skinColors;

    [SerializeField] List<Sprite> hairPrefabs;
    [SerializeField] List<Sprite> beardPrefabs;

    private void Awake() {
        EventsManager.OnCreateBody += OnCreateBody;
        EventsManager.OnAssembleBody += OnAssembleBody;
    }

    private void OnDestroy() {
        EventsManager.OnCreateBody -= OnCreateBody;
        EventsManager.OnAssembleBody -= OnAssembleBody;
    }

    private void OnCreateBody(CTeam.Player player) {
        player.bodyPartInfo = new BodyPartInfo();
        player.bodyPartInfo.hairColor = hairColors[Random.Range(0, hairColors.Count)];
        player.bodyPartInfo.skinColor = skinColors[Random.Range(0, skinColors.Count)];
        player.bodyPartInfo.beardColor = beardColors[Random.Range(0, beardColors.Count)];
        
        player.bodyPartInfo.hairIndex = Random.Range(0, hairPrefabs.Count);
        player.bodyPartInfo.beardIndex = Random.Range(0, beardPrefabs.Count);
    }

    private void OnAssembleBody(Image hair, Image beard, BodyPartInfo bodyPartInfo) {
        hair.color = bodyPartInfo.hairColor;
        if(bodyPartInfo.hairIndex > 1)
            hair.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(3000 , 3000);
        else
            hair.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(2048 , 2048);
            
        hair.sprite = hairPrefabs[bodyPartInfo.hairIndex];

        beard.color = bodyPartInfo.beardColor;
        beard.sprite = beardPrefabs[bodyPartInfo.beardIndex];
    }
}
