using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public enum Effects
    {
        ADD, SUB, MULT, DIV, AND, OR, XOR,      
        PCOR,                                   
        CONTRAST, NEGATIVE,                     
        HORIZONTALFLIP, VERTICALFLIP, FLIPBOTH,  
        BORDERDETECTION,                           
        AVERAGE3, AVERAGE5, MEDIAN3, MEDIAN5, MODE3, MODE5, MINIMUM3, MINIMUM5, MAXIMUM3, MAXIMUM5
    }

    // Variáveis Públicas -------------
    public GameObject allImages;
    public GameObject imagePlaceHolder;
    public Slider borderDetection;
    // --------------------------------

    // Variáveis Privadas -------------------------
    private int effect;
    private bool state = false;
    private bool buffer = false;
    private int size = -1;
    private List<Image> images = new List<Image>();
    private Texture2D texture;
    private Image m_image;

    private Image m_image1;
    // --------------------------------------------

    // Funções da Unity -----------
    private void Start()
    {
        m_image1 = imagePlaceHolder.GetComponent<Image>();
    }

    public void Update()
    {
        if (images.Count == size)
        {
            SetState(false);
            DoEffect();
        }

        if (buffer != state)
        {
            Select();
        }

        buffer = state;
    }
    // ----------------------------

    // Minhas Funções ----------------------------------------------------
    public void Select()
    {
        foreach (Toggle joint in allImages.GetComponentsInChildren<Toggle>())
        {
            joint.interactable = state;
            joint.isOn = false;
        }
    }

    public void AddImage(Image image)
    {
        images.Add(image);
    }

    public void DoEffect()
    {
        switch (effect)
        {
            case (int)Effects.ADD:
                texture = Arithmetic.Add(images[0], images[1]);
                break;

            case (int)Effects.SUB:
                texture = Arithmetic.Sub(images[0], images[1]);
                break;

            case (int)Effects.MULT:
                texture = Arithmetic.Mult(images[0], images[1]);
                break;

            case (int)Effects.DIV:
                texture = Arithmetic.Div(images[0], images[1]);
                break;

            case (int)Effects.AND:
                texture = Arithmetic.And(images[0], images[1]);
                break;

            case (int)Effects.OR:
                texture = Arithmetic.Or(images[0], images[1]);
                break;

            case (int)Effects.XOR:
                texture = Arithmetic.Xor(images[0], images[1]);
                break;

            case (int)Effects.PCOR:
                PCor();
                break;

            case (int)Effects.CONTRAST:
                Contrast();
                break;

            case (int)Effects.NEGATIVE:
                Negative();
                break;

            case (int)Effects.HORIZONTALFLIP:
                Flip(0);
                break;

            case (int)Effects.VERTICALFLIP:
                Flip(1);
                break;

            case (int)Effects.FLIPBOTH:
                Flip(2);
                break;

            case (int)Effects.BORDERDETECTION:
                BorderDetection();
                break;

            case (int)Effects.AVERAGE3:
                Average(3);
                break;

            case (int)Effects.AVERAGE5:
                Average(5);
                break;
            
            case (int)Effects.MEDIAN3:
                Median(3);
                break;

            case (int)Effects.MEDIAN5:
                Median(5);
                break;
            
            case (int)Effects.MODE3:
                Mode(3);
                break;

            case (int)Effects.MODE5:
                Mode(5);
                break;
            
            case (int)Effects.MINIMUM3:
                Minimum(3);
                break;

            case (int)Effects.MINIMUM5:
                Minimum(5);
                break;
            
            case (int)Effects.MAXIMUM3:
                Maximum(3);
                break;

            case (int)Effects.MAXIMUM5:
                Maximum(5);
                break;
        }
    }

    public void Clean()
    {
        SetSize(-1);
        images.Clear();
    }

    public void Apply()
    {
        texture.Apply();

        m_image1.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        imagePlaceHolder.SetActive(true);
    }

    public void PCor()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        float[] red = { 0, 0, 1.0f };
        float[] orange = { 0, 0, 5f, 1.0f };
        float[] yellow = { 0, 1.0f, 1.0f };
        float[] green = { 0, 1.0f, 1.0f };
        float[] blue = { 0, 0, 1.0f };
        float[] purple = { 0.5098f, 0, 0.2941f };
        float[] violet = { 1.0f, 0, 0.5608f };

        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float x = texA.GetPixel(column, row)[0];
                if (x > 0 && x < 0.1f)
                {
                    texture.SetPixel(column, row, new Color(red[0], red[1], red[2]));
                }
                else if (x > 0.1429f && x < 0.1429f * 2)
                {
                    texture.SetPixel(column, row, new Color(orange[0], orange[1], orange[2]));
                }
                else if (x > 0.1429f * 2 && x < 0.1429f * 3)
                {
                    texture.SetPixel(column, row, new Color(yellow[0], yellow[1], yellow[2]));
                }
                else if (x > 0.1429f * 3 && x < 0.1429f * 4)
                {
                    texture.SetPixel(column, row, new Color(green[0], green[1], green[2]));
                }
                else if (x > 0.1429f * 4 && x < 0.1429f * 5)
                {
                    texture.SetPixel(column, row, new Color(purple[0], purple[1], purple[2]));
                }
                else if (x > 0.1429f * 5 && x < 0.1429f * 6)
                {
                    texture.SetPixel(column, row, new Color(violet[0], violet[1], violet[2]));
                }
                else if (x > 0.1429f * 6 && x < 0.1429f * 7)
                {
                    texture.SetPixel(column, row, new Color(blue[0], blue[1], blue[2]));
                }
            }
        }

        Apply();
        Clean();
    }

    public void Contrast()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        float hi = 0;

        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                for (int channel = 0; channel < 3; channel++)
                {
                    float pixel = texA.GetPixel(column, row)[channel];

                    if (pixel > hi)
                    {
                        hi = pixel;
                    }
                }
            }
        }

        float x = 1.0f / hi;

        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float value = texA.GetPixel(column, row)[0] * x;

                texture.SetPixel(column, row, new Color(value, value, value));
            }
        }

        Apply();
        Clean();
    }

    public void Negative()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float value = 0.0f + Math.Abs(texA.GetPixel(column, row)[0] - 1.0f);

                texture.SetPixel(column, row, new Color(value, value, value));
            }
        }

        Apply();
        Clean();
    }

    public void HorizontalFlip(Texture2D texA, Texture2D texture)
    {
        for (int row = 0; row < texture.height; row++)
        {
            for (int column = 0, column2 = texture.width - 1; column < texture.height; column++, column2--)
            {
                var pixel = texA.GetPixel(column, row);
                texture.SetPixel(column, row, texA.GetPixel(column2, row));
                texture.SetPixel(column2, row, pixel);
            }
        }
    }

    public void VerticalFlip(Texture2D texA, Texture2D texture)
    {
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0, row2 = texture.height - 1; row < texture.height / 2; row++, row2--)
            {
                var pixel = texA.GetPixel(column, row);
                texture.SetPixel(column, row, texA.GetPixel(column, row2));
                texture.SetPixel(column, row2, pixel);
            }
        }
    }

    public void Flip(int option)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        switch (option)
        {
            case 0:
                HorizontalFlip(texA, texture);
                break;
            case 1:
                VerticalFlip(texA, texture);
                break;
            case 2:
                HorizontalFlip(texA, texture);
                VerticalFlip(texture, texture);
                break;
        }

        Apply();
        Clean();
    }

    public void BorderDetection()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        float previous = -1.0f;

        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                //float sum = 0.0f;

                float pixel = texA.GetPixel(column, row)[0];

                if (pixel > previous + borderDetection.value || pixel < previous - borderDetection.value)
                {
                    texture.SetPixel(column, row, new Color(0.5f, 0.5f, 0.5f));
                }
                else
                {
                    texture.SetPixel(column, row, new Color(0, 0, 0));
                }

                previous = pixel;
            }
        }

        for (int row = 0; row < texture.width; row++)
        {
            for (int column = 0; column < texture.height; column++)
            {
                //float sum = 0.0f;

                float pixel = texA.GetPixel(column, row)[0];

                if (pixel > previous + borderDetection.value || pixel < previous - borderDetection.value)
                {
                    float value = texture.GetPixel(column, row)[0] + 0.5f;
                    texture.SetPixel(column, row, new Color(value, value, value));
                }
                else
                { 
                    float value = texture.GetPixel(column, row)[0];
                    texture.SetPixel(column, row, new Color(value, value, value));
                }

                previous = pixel;
            }
        }

        Apply();
    }

    public void Average(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        float previous = -1.0f;

        int start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {

                float sum = 0.0f;

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        sum += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                float value = sum / (type * type);
                texture.SetPixel(column, row, new Color(value, value, value));
            }
        }

        Apply();
        Clean();
    }
    
    public void Median(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        float previous = -1.0f;

        int start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {
                List<float> sum = new List<float>();

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        sum.Add(texA.GetPixel(averageCol + column, averageRow + row)[0]);
                    }
                }

                sum.Sort();

                float value = sum[start * 2];

                texture.SetPixel(column, row, new Color(value, value, value));
            }
        }

        Apply();
        Clean();
    }
    
    public void Mode(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        float previous = -1.0f;

        int start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {
                float sum = 0.0f;
                float[] arr = new float[256];

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        sum += texA.GetPixel(averageCol + column, averageRow + row)[0];
                        int i = (int)(texA.GetPixel(averageCol + column, averageRow + row)[0] * 255);
                        arr[i]++;
                    }
                }

                float maior = arr[0];
                float value = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    if(arr[i] > maior){
                        maior = arr[i];
                        value = i;
                    }
                }

                value /= 255;

                if(maior == 1){
                    value = sum / (type * type);
                }

                texture.SetPixel(column, row, new Color(value, value, value));
            }
        }

        Apply();
        Clean();
    }
    
    public void Minimum(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {

                float minimum = 1.0f;

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        if (minimum > texA.GetPixel(averageCol + column, averageRow + row)[0])
                        {
                            minimum = texA.GetPixel(averageCol + column, averageRow + row)[0];
                        }
                    }
                }
                
                texture.SetPixel(column, row, new Color(minimum, minimum, minimum));
            }
        }

        Apply();
        Clean();
    }
    
    public void Maximum(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {

                float maximum = 0.0f;

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        if (maximum < texA.GetPixel(averageCol + column, averageRow + row)[0])
                        {
                            maximum = texA.GetPixel(averageCol + column, averageRow + row)[0];
                        }
                    }
                }
                
                texture.SetPixel(column, row, new Color(maximum, maximum, maximum));
            }
        }

        Apply();
        Clean();
    }
    
    public void Kuwahara(int type)
    {
        Image a = images[0];
     
        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start = 2;
     
        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {
                float average = 0.0f;
                float sum = 0.0f;
                float va
                     
                for (int averageRow = 0; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = 0; averageCol >= -start; averageCol--)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                average /= Mathf.Pow(type, 2);
                
                for (int averageRow = 0; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = 0; averageCol >= -start; averageCol--)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;

                texture.SetPixel(column, row, new Color(maximum, maximum, maximum));
            }
        }
     
        Apply();
        Clean();
    }

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
