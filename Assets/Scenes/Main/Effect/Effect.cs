using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    enum Effects { ADD, SUB };

    // Variáveis Públicas ------
    public GameObject allImages;
    // -------------------------

    // Variáveis Privadas -------------------------
    private int effect;
    private bool state = false;
    private bool buffer = false;
    private int size = -1;
    private List<Image> images = new List<Image>();
    // --------------------------------------------

    private void Start() {
        buffer = state;
    }

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

    // Minhas Funções ----------------------------------------------------
    private void Select() {
        Component[] toggles = allImages.GetComponentsInChildren<Toggle>();

        foreach (Toggle joint in toggles) {
            joint.interactable = state;
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
        }
    }

    // -----------------
    private void Add() {
        
        
        foreach(Image joint in images) {
            joint.sprite.texture
        }
    }
    // -----------------

    // Gets and Sets -----------------
    public bool GetState() {
        return state;
    }

    public void SetState(bool state) {
        this.state = state;
    }
    
    public int GetSize() {
        return size;
    }

    public void SetSize(int size) {
        this.size = size;
    }

    public int GetEffect() {
        return effect;
    }

    public void SetEffect(int effect) {
        this.effect = effect;
    }
    // --------------------------------

    // -------------------------------------------------------------------
}
