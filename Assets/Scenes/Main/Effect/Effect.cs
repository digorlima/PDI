using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    enum Effects { ADD, SUB, MULT, DIV };

    // Variáveis Públicas ------
    public GameObject allImages;
    public GameObject imagePlaceHolder;
    // -------------------------

    // Variáveis Privadas -------------------------
    private int effect;
    private bool state = false;
    private bool buffer = false;
    private int size = -1;
    private List<Image> images = new List<Image>();

    private Image m_image;
    // --------------------------------------------

    // Funções da Unity -----------
    private void Update() {
        if (images.Count == size) {
            SetState(false);
            DoEffect();
        }

        if (buffer != state) {
            Select();
        } 

        buffer = state;
    }
    // ----------------------------

    // Minhas Funções ----------------------------------------------------
    private void Select() {
        Component[] toggles = allImages.GetComponentsInChildren<Toggle>();

        foreach (Toggle joint in toggles) {
            joint.interactable = state;
            joint.isOn = false;
        }
    }

    public void AddImage(Image image) {
        images.Add(image);
    }

    public void DoEffect() {
        switch (effect) {
            case (int)Effects.ADD:
                Add();
                break; 
            
            case (int)Effects.SUB:
                Sub();
                break;
            
            case (int)Effects.MULT:
                Mult();
                break;
            
            case (int)Effects.DIV:
                Div();
                break;
        }
    }

    // -----------------
    private void Add() {
        Image a = images[0];
        Image b = images[1];

        float[] sum = new float[3];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        var texSum = new Texture2D(texA.width, texA.height);

        for(int row = 0; row < texA.width; row++) {
            for (int column = 0; column < texA.height; column++) {
                for (int channel = 0; channel < 3; channel++) {
                    sum[channel] = texA.GetPixel(row, column)[channel] + 
                                   texB.GetPixel(row, column)[channel];
                }

                texSum.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        texSum.Apply();
        
        imagePlaceHolder.GetComponent<Image>().sprite = Sprite.Create(texSum, new Rect(0, 0, texSum.width, texSum.height), new Vector2(0.5f, 0.5f), 100.0f);
        imagePlaceHolder.SetActive(true);
        
        SetSize(-1);
        images.Clear();
    }
    
    private void Sub() {
        Image a = images[0];
        Image b = images[1];

        float[] sum = new float[3];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        var texSum = new Texture2D(texA.width, texA.height);

        for(int row = 0; row < texA.width; row++) {
            for (int column = 0; column < texA.height; column++) {
                for (int channel = 0; channel < 3; channel++) {
                    sum[channel] = texA.GetPixel(row, column)[channel] -
                                   texB.GetPixel(row, column)[channel];
                }

                texSum.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        texSum.Apply();
        
        imagePlaceHolder.GetComponent<Image>().sprite = Sprite.Create(texSum, new Rect(0, 0, texSum.width, texSum.height), new Vector2(0.5f, 0.5f), 100.0f);
        imagePlaceHolder.SetActive(true);
        
        SetSize(-1);
        images.Clear();
    }
    
    private void Mult() {
        Image a = images[0];
        Image b = images[1];

        float[] sum = new float[3];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        var texSum = new Texture2D(texA.width, texA.height);

        for(int row = 0; row < texA.width; row++) {
            for (int column = 0; column < texA.height; column++) {
                for (int channel = 0; channel < 3; channel++) {
                    sum[channel] = texA.GetPixel(row, column)[channel] * 
                                   texB.GetPixel(row, column)[channel];
                }

                texSum.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        texSum.Apply();
        
        imagePlaceHolder.GetComponent<Image>().sprite = Sprite.Create(texSum, new Rect(0, 0, texSum.width, texSum.height), new Vector2(0.5f, 0.5f), 100.0f);
        imagePlaceHolder.SetActive(true);
        
        SetSize(-1);
        images.Clear();
    }
    
    private void Div() {
        Image a = images[0];
        Image b = images[1];

        float[] sum = new float[3];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        var texSum = new Texture2D(texA.width, texA.height);

        for(int row = 0; row < texA.width; row++) {
            for (int column = 0; column < texA.height; column++) {
                for (int channel = 0; channel < 3; channel++) {
                    sum[channel] = texA.GetPixel(row, column)[channel] / 
                                   texB.GetPixel(row, column)[channel];
                }

                texSum.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        texSum.Apply();
        
        imagePlaceHolder.GetComponent<Image>().sprite = Sprite.Create(texSum, new Rect(0, 0, texSum.width, texSum.height), new Vector2(0.5f, 0.5f), 100.0f);
        imagePlaceHolder.SetActive(true);
        
        SetSize(-1);
        images.Clear();
    }
    // -----------------

    // Gets and Sets ------------------------------------------
    public bool GetState() { return state; }
    public void SetState(bool state) { this.state = state; }
    
    public int GetSize() { return size; }
    public void SetSize(int size) { this.size = size; }

    public int GetEffect() { return effect; }
    public void SetEffect(int effect) { this.effect = effect; }
    // --------------------------------------------------------

    // -------------------------------------------------------------------
}
