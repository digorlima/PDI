using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Doozy.Engine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class OpenFile : MonoBehaviour
{
    public GameObject content;
    public GameObject viewport;
    public GameObject imagePreset;

    public Effect effect;

    public float imageSize;

    public void Open()
    {
        string path = EditorUtility.OpenFilePanel("Open a image", "", "png;*jpg;*jpeg;*pgm;*");
        string extension = "";
        extension = extension + path[path.Length - 3];
        extension = extension + path[path.Length - 2];
        extension = extension + path[path.Length - 1];

        Texture2D tex;
        
        if (extension == "pgm")
        {
            tex = readPgm(path);
        }
        else
        {
            var fileContent = File.ReadAllBytes(path);
                    
            tex = new Texture2D(0, 0); 
            tex.LoadImage(fileContent);
        }
        
        
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

    Texture2D readPgm(string path)
    {
        string text = System.IO.File.ReadAllText(path);
        text = text.Remove(0, 4);

        string width = "", height = "";
        int i = 0;
        for (i = 0; text[i] != ' ' ; i++)
        {
            width = width + text[i];
        }
        text = text.Remove(0, i+1);
        for (i = 0; text[i] != '\r'; i++)
        {
            height = height + text[i];
        }
        text = text.Remove(0, i+2);
        
        text = text.Remove(0, 5);
        
        text = Regex.Replace(text, @"\r\n", " ");

        Texture2D texture = new Texture2D(int.Parse(width), int.Parse(width));
        
        string strPixel = "";
        for(int row = int.Parse(width) - 1; row > 0; row--) {
            for (int column = 0; column < texture.height; column++) {
                strPixel = "";
                for (i = 0; text[i] != ' '; i++)
                {
                    strPixel += text[i];
                }

                //Debug.Log(row + " " + column);

                text = text.Remove(0, i + 1);
                
                int pixel = int.Parse(strPixel);

                float value = pixel / 255.0f;
                
                texture.SetPixel(column, row, new Color(value, value, value));
            }
        }

        return texture;
    }
}
