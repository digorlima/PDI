using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Doozy.Engine.UI;

public class OpenFile : MonoBehaviour
{
    public GameObject content;
    public GameObject viewport;
    public GameObject imagePreset;

    public Effect effect;

    public float imageSize;

    public void Open()
    {
        string path = EditorUtility.OpenFilePanel("Open a image", "", "png;*jpg;*jpeg;*");
        var fileContent = File.ReadAllBytes(path);
        
        Texture2D tex = new Texture2D(0, 0);
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
        float scale = imageSize / big;

        GameObject newObject = Instantiate(imagePreset);
        newObject.transform.SetParent(content.transform, false);

        GameObject image = newObject.transform.GetChild(0).gameObject;

        image.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        ((RectTransform)newObject.transform).sizeDelta = new Vector2 (tex.width * scale, tex.height * scale);
        newObject.transform.localScale = new Vector3(1, 1);

        image.GetComponent<Toggle>().interactable = effect.GetState();
        image.GetComponent<UIToggle>().OnClick.OnToggleOn.Event.AddListener(delegate { ValueChanged(image.GetComponent<Image>()); });

        if (viewport.GetComponent<Mask>() == null)
        {
            viewport.AddComponent<Mask>().showMaskGraphic = false;
            viewport.AddComponent<Image>();
        }
    }

    void ValueChanged(Image image) {
        effect.AddImage(image);
    }
}
