using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class OpenFile : MonoBehaviour
{
    public GameObject content;
    public GameObject viewport;
    public GameObject destroy;
    private GameObject newObject;

    public void Apply()
    {
        Texture2D tex = new Texture2D(0, 0);
        string path = EditorUtility.OpenFilePanel("Open a image", "", "png;*jpg;*jpeg");
        var fileContent = File.ReadAllBytes(path);
        tex.LoadImage(fileContent);
        tex.Apply();

        float big;
        if (tex.width > tex.height)
        {
            big = tex.width;
        }
        else
        {
            big = tex.height;
        }
        float scale = 200 / big;

        newObject = new GameObject();
        
        newObject.AddComponent<Image>().sprite = Sprite.Create(tex, new Rect(0,0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        ((RectTransform)newObject.transform).sizeDelta = new Vector2 (tex.width, tex.height);
        newObject.transform.localScale = new Vector3(scale, scale);
        newObject.transform.SetParent(content.transform, false);

        if (viewport.GetComponent<Mask>() == null)
        {
            viewport.AddComponent<Mask>().showMaskGraphic = false;
            viewport.AddComponent<Image>();
        }

        GameObject newDestroy = Instantiate(destroy);
        newDestroy.transform.SetParent(newObject.transform, false);
        ((RectTransform)newDestroy.transform).anchorMin = new Vector2(0, 1);
        ((RectTransform)newDestroy.transform).anchorMax = new Vector2(0, 1);
        ((RectTransform)newDestroy.transform).pivot = new Vector2(0, 1);
    }
}
