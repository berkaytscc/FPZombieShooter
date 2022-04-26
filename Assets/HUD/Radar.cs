using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadarObject
{
    public Image icon { get; set; }
    public GameObject owner { get; set; }
}

public class Radar : MonoBehaviour
{
    public Transform playerPosition;
    public float mapScale = 2.0f;

    public static List<RadarObject> radObject = new List<RadarObject>();
    public static void RegisterRadarObject(GameObject o, Image i)
    {
        Image image = Instantiate(i);
        radObject.Add(new RadarObject() { owner = o, icon = image });
    }

    public static void RemoveRadarObject(GameObject o)
    {
        List<RadarObject> newList = new List<RadarObject>();
        for (int i = 0; i < radObject.Count; i++)
        {
            if (radObject[i].owner == o)
            {
                Destroy(radObject[i].icon);
                continue;
            }
            else
            {
                newList.Add(radObject[i]);
            }
        }

        radObject.RemoveRange(0, radObject.Count);
        radObject.AddRange(newList);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPosition == null) return;
        foreach (RadarObject ro in radObject)
        {
            Vector3 radPos = ro.owner.transform.position - playerPosition.position;
            float distToObject = Vector3.Distance(playerPosition.position, ro.owner.transform.position) * mapScale;

            float deltay = Mathf.Atan2(radPos.x, radPos.z) * Mathf.Rad2Deg - 270 - playerPosition.eulerAngles.y;
            radPos.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
            radPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);

            ro.icon.transform.SetParent(this.transform);
            RectTransform rt = this.GetComponent<RectTransform>();
            ro.icon.transform.position = new Vector3(radPos.x + rt.pivot.x, radPos.z + rt.pivot.y, 0) + this.transform.position;
        }
    }
}
