using UnityEngine;

public class TrailController : MonoBehaviour {

    public Transform[] trailPrefabs;
    public ItemEnum trail;
    
    // If a trail is equiped, display it
	void Start () {
        if (ApplicationController.ac.playerData.equipedItems.ContainsKey(ItemTypeEnum.trail))
        {
            trail = ApplicationController.ac.playerData.equipedItems[ItemTypeEnum.trail];
            int index = GetTrailIndex(trail);
            if (index < trailPrefabs.Length)
                Instantiate(trailPrefabs[index], this.transform.position, trailPrefabs[index].rotation,this.transform);
        }
	}

    int GetTrailIndex(ItemEnum item)
    {
        int index = 999;
        switch (item)
        {
            case ItemEnum.trail_white:
                index = 0;
                break;
            case ItemEnum.trail_cyan:
                index = 1;
                break;
            case ItemEnum.trail_red:
                index = 2;
                break;
            case ItemEnum.trail_yellow:
                index = 3;
                break;
            case ItemEnum.trail_black:
                index = 4;
                break;
            case ItemEnum.trail_rainbow:
                index = 5;
                break;
        }
        return index;
    }
	
}
