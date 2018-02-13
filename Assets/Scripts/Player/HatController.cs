using UnityEngine;

public class HatController : MonoBehaviour {

    public Transform[] hatPrefabs;
    public ItemEnum hat;
    
    // If a hat is equiped, display it
	void Start () {
        if (ApplicationController.ac.playerData.equipedItems.ContainsKey(ItemTypeEnum.hat))
        {
            hat = ApplicationController.ac.playerData.equipedItems[ItemTypeEnum.hat];
            int index = GetHatIndex(hat);
            if (index < hatPrefabs.Length)
                Instantiate(hatPrefabs[index], this.transform.position, hatPrefabs[index].rotation,this.transform);
        }
	}

    int GetHatIndex(ItemEnum item)
    {
        int index = 999;
        switch (item)
        {
            case ItemEnum.hat_top:
                index = 0;
                break;
            case ItemEnum.hat_cowboy:
                index = 1;
                break;
            case ItemEnum.hat_academic:
                index = 2;
                break;
        }
        return index;
    }
	
}
