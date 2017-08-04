using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class SaveScene : MonoBehaviour {

    public Toggle[] sceneToggles;
    public string[] sceneFilPath;
    public GameObject toggleToModifyParent;

    private Toggle[] tempToggleList;
    private Slider tempSlider;
    private setOn tempSetOn;
    private SceneData sceneData;

    public void Start()
    {
        tempToggleList = toggleToModifyParent.GetComponentsInChildren<Toggle>();

        for(int i = 0; i < sceneFilPath.Length; i++)
        {
            if (!File.Exists(Application.persistentDataPath + "/" + sceneFilPath[i]+".json"))
            {
                //Debug.Log("create persistent file from ressource file : " + sceneFilPath[i]);
                TextAsset file = Resources.Load(sceneFilPath[i]) as TextAsset;
                string content = file.ToString();
                //Debug.Log("Content of the file resource and future persistent file : " + content);

                string filePath = Application.persistentDataPath + "/" + sceneFilPath[i]+".json";
                File.WriteAllText(filePath, content);
            }
       }
    }

    public void loadScene()
    {
        for(int i = 0; i< sceneToggles.Length; i++)
        {
            if(sceneToggles[i].isOn)
            {
                string filePath = Application.persistentDataPath +"/"+ sceneFilPath[i]+".json";
                //Debug.Log("this is the path to search file : " + filePath);

                if (File.Exists(filePath))
                {
                    string dataAsJson = File.ReadAllText(filePath);
                    //Debug.Log("print the json parsed : " + dataAsJson);
                    sceneData = JsonUtility.FromJson<SceneData>(dataAsJson);
                    //Debug.Log("sceneDataLeng : " + sceneData.intensity.Length);

                    for (int j = 0; j < tempToggleList.Length; j++)
                    {
                        tempSlider = tempToggleList[j].GetComponentInChildren<Slider>();
                        tempSetOn = tempToggleList[j].GetComponent<setOn>();
                        tempSlider.value = sceneData.intensity[j];
                        tempSetOn.sendValue(tempSlider.value);
                    }
                }
                else
                {
                    Debug.LogError("Cannot load scene data");
                }
            }
        }
    }

    public void saveScene()
    {

        for (int j = 0; j < tempToggleList.Length; j++)
        {
            tempSlider = tempToggleList[j].GetComponentInChildren<Slider>();
            sceneData.intensity[j] = (int)tempSlider.value;
        }

        for (int i = 0; i< sceneToggles.Length; i++)
        {
            if (sceneToggles[i].isOn)
            {
                string dataAsJson = JsonUtility.ToJson(sceneData);
                string filePath = Path.Combine(Application.persistentDataPath, sceneFilPath[i]+ ".json");
                File.WriteAllText(filePath, dataAsJson);
            }
        }
    }

    [Serializable]
    class SceneData
    {
        public int[] intensity;
    }
}
