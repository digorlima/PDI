using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class Effect : MonoBehaviour
{
    public enum Effects
    {
        ADD, SUB, MULT, DIV, AND, OR, XOR,
        PCOR,
        CONTRAST, NEGATIVE,
        HORIZONTALFLIP, VERTICALFLIP, FLIPBOTH,
        BORDERDETECTION,
        AVERAGE, MEDIAN, MODE, MINIMUM, MAXIMUM,
        KUWAHARA, TOMITA, NAGAO, SOMBOONKAEW,
        M1, M2, M3, H1, H2
    }

    [Header("Objetos do editor")]
    public GameObject allImages;
    public GameObject imagePlaceHolder;

    [Header("Objetos para efeitos")]
    public Slider borderSlider;
    public TMP_InputField matInput;

    private int effect;
    private bool state = false;
    private bool buffer = false;
    private bool forceDo = true;
    private int size = -1;
    private List<Image> images = new List<Image>();
    private Texture2D texture;
    private Image m_image1;

    // Funções da Unity -----------
    private void Start()
    {
        m_image1 = imagePlaceHolder.GetComponent<Image>();
    }

    public void Update()
    {
        if (images.Count == size && forceDo)
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

            case (int)Effects.AND:
                And();
                break;

            case (int)Effects.OR:
                Or();
                break;

            case (int)Effects.XOR:
                Xor();
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

            case (int)Effects.AVERAGE:
                Average(GetMat(3));
                break;

            case (int)Effects.MEDIAN:
                Median(GetMat(3));
                break;

            case (int)Effects.MODE:
                Mode(GetMat(3));
                break;

            case (int)Effects.MINIMUM:
                Minimum(GetMat(3));
                break;

            case (int)Effects.MAXIMUM:
                Maximum(GetMat(3));
                break;

            case (int)Effects.KUWAHARA:
                Kuwahara(GetMat(5));
                break;

            case (int)Effects.TOMITA:
                Tomita(GetMat(5));
                break;

            case (int)Effects.NAGAO:
                Nagao(GetMat(5));
                break;

            case (int)Effects.SOMBOONKAEW:
                Somboonkaew(GetMat(5));
                break;

            case (int)Effects.M1:
                M1(GetMat(3));
                break;

            case (int)Effects.M2:
                M2(GetMat(3));
                break;

            case (int)Effects.M3:
                M3(GetMat(3));
                break;

            case (int)Effects.H1:
                H1(GetMat(3));
                break;

            case (int)Effects.H2:
                H2(GetMat(3));
                break;
        }

        Apply();
    }

    public void Clean()
    {
        SetSize(-1);
        images.Clear();
        forceDo = true;

        if(matInput.gameObject.active){
            matInput.text = "";
        }
    }

    public void Apply()
    {
        texture.Apply();

        m_image1.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        imagePlaceHolder.SetActive(true);
    }

    public int GetMat(int standard)
    {
        if (matInput.text == "")
        {
            matInput.text = standard + "";
            return standard;
        }
        else
        {
            int value = Convert.ToInt32(matInput.text, 10);

            if (value % 2 == 0)
            {
                value--;
                matInput.text = value + "";
            }
            else if (value < 0){
                value = standard;
                matInput.text = value + "";
            }

            return value;
        }
    }

    public void Add()
    {
        Image a = images[0];
        Image b = images[1];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        texture = new Texture2D(texA.width, texA.height);

        for (int row = 0; row < texA.width; row++)
        {
            for (int column = 0; column < texA.height; column++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);
                    int pixel = pixelA + pixelB;

                    sum[channel] = pixel / 255.0f;
                }

                texture.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }

        Clean();
    }

    public void Sub()
    {
        Image a = images[0];
        Image b = images[1];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        texture = new Texture2D(texA.width, texA.height);

        for (int row = 0; row < texA.width; row++)
        {
            for (int column = 0; column < texA.height; column++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);
                    int pixel = pixelA - pixelB;

                    sum[channel] = pixel / 255.0f;
                }

                texture.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }

        Clean();
    }

    public void Mult()
    {
        Image a = images[0];
        Image b = images[1];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        texture = new Texture2D(texA.width, texA.height);

        for (int row = 0; row < texA.width; row++)
        {
            for (int column = 0; column < texA.height; column++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);
                    int pixel = pixelA * pixelB;

                    sum[channel] = pixel / 255.0f;
                }

                texture.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }

        Clean();
    }

    public void Div()
    {
        Image a = images[0];
        Image b = images[1];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        texture = new Texture2D(texA.width, texA.height);

        for (int row = 0; row < texA.width; row++)
        {
            for (int column = 0; column < texA.height; column++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);
                    int pixel = 0;

                    if (pixelB != 0)
                    {
                        pixel = pixelA / pixelB;
                    }

                    sum[channel] = pixel / 255.0f;
                }

                texture.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }

        Clean();
    }

    public void And()
    {
        Image a = images[0];
        Image b = images[1];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        texture = new Texture2D(texA.width, texA.height);

        for (int row = 0; row < texA.width; row++)
        {
            for (int column = 0; column < texA.height; column++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);
                    int pixel = pixelA & pixelB;

                    sum[channel] = pixel / 255.0f;
                }

                texture.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }

        Clean();
    }

    public void Or()
    {
        Image a = images[0];
        Image b = images[1];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        texture = new Texture2D(texA.width, texA.height);

        for (int row = 0; row < texA.width; row++)
        {
            for (int column = 0; column < texA.height; column++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);
                    int pixel = pixelA | pixelB;

                    sum[channel] = pixel / 255.0f;
                }

                texture.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }

        Clean();
    }

    public void Xor()
    {
        Image a = images[0];
        Image b = images[1];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        texture = new Texture2D(texA.width, texA.height);

        for (int row = 0; row < texA.width; row++)
        {
            for (int column = 0; column < texA.height; column++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);
                    int pixel = pixelA ^ pixelB;

                    sum[channel] = pixel / 255.0f;
                }

                texture.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }

        Clean();
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

                if (pixel > previous + borderSlider.value || pixel < previous - borderSlider.value)
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

                if (pixel > previous + borderSlider.value || pixel < previous - borderSlider.value)
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

        forceDo = false;
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

        forceDo = false;
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

        forceDo = false;
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
                    if (arr[i] > maior)
                    {
                        maior = arr[i];
                        value = i;
                    }
                }

                value /= 255;

                if (maior == 1)
                {
                    value = sum / (type * type);
                }

                texture.SetPixel(column, row, new Color(value, value, value));
            }
        }

        forceDo = false;
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

        forceDo = false;
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

        forceDo = false;
    }

    public void Kuwahara(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start;

        start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {
                float quantity = Mathf.Pow((start + 1), 2);
                float average = 0.0f;
                float sum = 0.0f;
                List<float> averages = new List<float>();
                List<float> variances = new List<float>();

                // -----------------------------------------------------------------------------------------------------------

                for (int averageRow = 0; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = 0; averageCol >= -start; averageCol--)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                average /= quantity;
                averages.Add(average);

                for (int averageRow = 0; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = 0; averageCol >= -start; averageCol--)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;

                variances.Add(Mathf.Sqrt((1.0f / quantity) * sum));
                sum = 0.0f;

                // ----------------------------------------------------------------------------------------------------------

                for (int averageRow = 0; averageRow <= start; averageRow++)
                {
                    for (int averageCol = 0; averageCol >= -start; averageCol--)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                average /= quantity;
                averages.Add(average);

                for (int averageRow = 0; averageRow <= start; averageRow++)
                {
                    for (int averageCol = 0; averageCol >= -start; averageCol--)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;

                variances.Add(Mathf.Sqrt((1.0f / quantity) * sum));
                sum = 0.0f;

                // ----------------------------------------------------------------------------------------------------------

                for (int averageRow = 0; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = 0; averageCol <= start; averageCol++)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                average /= quantity;
                averages.Add(average);

                for (int averageRow = 0; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = 0; averageCol <= start; averageCol++)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;

                variances.Add(Mathf.Sqrt((1.0f / quantity) * sum));
                sum = 0.0f;

                // ----------------------------------------------------------------------------------------------------------

                for (int averageRow = 0; averageRow <= start; averageRow++)
                {
                    for (int averageCol = 0; averageCol <= start; averageCol++)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                average /= quantity;
                averages.Add(average);

                for (int averageRow = 0; averageRow <= start; averageRow++)
                {
                    for (int averageCol = 0; averageCol <= start; averageCol++)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                variances.Add(Mathf.Sqrt((1.0f / quantity) * sum));

                // ----------------------------------------------------------------------------------------------------------

                float m = variances[0];
                int j = 0;

                for (int i = 1; i < variances.Count; i++)
                {
                    if (m > variances[i])
                    {
                        m = variances[i];
                        j = i;
                    }
                }

                texture.SetPixel(column, row, new Color(averages[j], averages[j], averages[j]));
            }
        }

        forceDo = false;
    }

    public void Tomita(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start;

        start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {
                float quantity = Mathf.Pow((start + 1), 2);
                float average = 0.0f;
                float sum = 0.0f;
                List<float> averages = new List<float>();
                List<float> variances = new List<float>();

                // -----------------------------------------------------------------------------------------------------------

                for (int averageRow = 0; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = 0; averageCol >= -start; averageCol--)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                average /= quantity;
                averages.Add(average);

                for (int averageRow = 0; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = 0; averageCol >= -start; averageCol--)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;
                variances.Add(Mathf.Sqrt((1.0f / quantity) * sum));
                sum = 0.0f;

                // ----------------------------------------------------------------------------------------------------------

                for (int averageRow = 0; averageRow <= start; averageRow++)
                {
                    for (int averageCol = 0; averageCol >= -start; averageCol--)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                average /= quantity;
                averages.Add(average);

                for (int averageRow = 0; averageRow <= start; averageRow++)
                {
                    for (int averageCol = 0; averageCol >= -start; averageCol--)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;
                variances.Add(Mathf.Sqrt((1.0f / quantity) * sum));
                sum = 0.0f;

                // ----------------------------------------------------------------------------------------------------------

                for (int averageRow = 0; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = 0; averageCol <= start; averageCol++)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                average /= quantity;
                averages.Add(average);

                for (int averageRow = 0; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = 0; averageCol <= start; averageCol++)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;
                variances.Add(Mathf.Sqrt((1.0f / quantity) * sum));
                sum = 0.0f;

                // ----------------------------------------------------------------------------------------------------------

                for (int averageRow = 0; averageRow <= start; averageRow++)
                {
                    for (int averageCol = 0; averageCol <= start; averageCol++)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                average /= quantity;
                averages.Add(average);

                for (int averageRow = 0; averageRow <= start; averageRow++)
                {
                    for (int averageCol = 0; averageCol <= start; averageCol++)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;
                variances.Add(Mathf.Sqrt((1.0f / quantity) * sum));
                sum = 0.0f;
                // ----------------------------------------------------------------------------------------------------------

                for (int averageRow = -start + 1; averageRow <= start - 1; averageRow++)
                {
                    for (int averageCol = -start + 1; averageCol <= start - 1; averageCol++)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                average /= quantity;
                averages.Add(average);

                for (int averageRow = -start + 1; averageRow <= start - 1; averageRow++)
                {
                    for (int averageCol = -start + 1; averageCol <= start - 1; averageCol++)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;
                variances.Add(Mathf.Sqrt((1.0f / quantity) * sum));
                sum = 0.0f;

                // ----------------------------------------------------------------------------------------------------------

                float m = variances[0];
                int j = 0;

                for (int i = 1; i < variances.Count; i++)
                {
                    if (m > variances[i])
                    {
                        m = variances[i];
                        j = i;
                    }
                }

                texture.SetPixel(column, row, new Color(averages[j], averages[j], averages[j]));
            }
        }

        forceDo = false;
    }

    public void Nagao(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start;

        start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {
                float quantity = Mathf.Pow((start + 1), 2);
                float average = 0.0f;
                float sum = 0.0f;
                List<float> averages = new List<float>();
                List<float> variances = new List<float>();
                float counter = 1;

                // -----------------------------------------------------------------------------------------------------------

                for (int averageRow = -start + 1; averageRow <= start - 1; averageRow++)
                {
                    for (int averageCol = -start + 1; averageCol <= start - 1; averageCol++)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                        counter++;
                    }
                }

                average /= counter;
                averages.Add(average);

                for (int averageRow = -start + 1; averageRow <= start - 1; averageRow++)
                {
                    for (int averageCol = -start + 1; averageCol <= start - 1; averageCol++)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;
                variances.Add(Mathf.Sqrt((1.0f / counter) * sum));
                sum = 0.0f;
                counter = 1;

                // ----------------------------------------------------------------------------------------------------------

                for (int averageRow = -1; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = -start + 1; averageCol <= start - 1; averageCol++)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                        counter++;
                    }
                }

                average /= counter;
                averages.Add(average);

                for (int averageRow = -1; averageRow >= -start; averageRow--)
                {
                    for (int averageCol = -start + 1; averageCol <= start - 1; averageCol++)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;
                variances.Add(Mathf.Sqrt((1.0f / counter) * sum));
                sum = 0.0f;
                counter = 1;

                // ----------------------------------------------------------------------------------------------------------

                for (int averageRow = -start + 1; averageRow <= start - 1; averageRow++)
                {
                    for (int averageCol = 1; averageCol <= start; averageCol++)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                        counter++;
                    }
                }

                average /= counter;
                averages.Add(average);

                for (int averageRow = -start + 1; averageRow <= start - 1; averageRow++)
                {
                    for (int averageCol = 1; averageCol <= start; averageCol++)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;
                variances.Add(Mathf.Sqrt((1.0f / counter) * sum));
                sum = 0.0f;
                counter = 1;

                // ----------------------------------------------------------------------------------------------------------

                for (int averageRow = 1; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start + 1; averageCol <= start - 1; averageCol++)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                        counter++;
                    }
                }

                average /= counter;
                averages.Add(average);

                for (int averageRow = 1; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start + 1; averageCol <= start - 1; averageCol++)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;
                variances.Add(Mathf.Sqrt((1.0f / counter) * sum));
                sum = 0.0f;
                counter = 1;

                // ----------------------------------------------------------------------------------------------------------

                for (int averageRow = -start + 1; averageRow <= start - 1; averageRow++)
                {
                    for (int averageCol = -1; averageCol >= -start; averageCol--)
                    {
                        average += texA.GetPixel(averageCol + column, averageRow + row)[0];
                        counter++;
                    }
                }

                average /= counter;
                averages.Add(average);

                for (int averageRow = -start + 1; averageRow <= start - 1; averageRow++)
                {
                    for (int averageCol = -1; averageCol >= -start; averageCol--)
                    {
                        sum += Mathf.Pow((texA.GetPixel(averageCol + column, averageRow + row)[0] - average), 2);
                    }
                }

                average = 0.0f;
                variances.Add(Mathf.Sqrt((1.0f / counter) * sum));
                sum = 0.0f;
                counter = 0;

                // ----------------------------------------------------------------------------------------------------------
                int rowFactor = 1;
                int colFactor = 1;

                int aux = 0;

                while (true)
                {
                    if (aux == 0)
                    {
                        rowFactor = 1;
                        colFactor = 1;
                    }
                    else if (aux == 1)
                    {
                        rowFactor = 1;
                        colFactor = -1;
                    }
                    else if (aux == 2)
                    {
                        rowFactor = -1;
                        colFactor = 1;
                    }
                    else if (aux == 3)
                    {
                        rowFactor = -1;
                        colFactor = -1;
                    }
                    else if (aux == 4)
                    {
                        break;
                    }

                    for (int i = 1; i <= start; i++)
                    {
                        average += texA.GetPixel(column - (colFactor * (i - 1)), row - (rowFactor * i))[0];
                        average += texA.GetPixel(column - (colFactor * i), row - (rowFactor * (i - 1)))[0];
                        average += texA.GetPixel(column - (colFactor * i), row - (rowFactor * i))[0];
                        counter += 3;
                    }

                    average /= counter;
                    averages.Add(average);

                    for (int i = 1; i <= start; i++)
                    {
                        sum += Mathf.Pow((texA.GetPixel(column - (colFactor * (i - 1)), row - (rowFactor * i))[0] - average), 2);
                        sum += Mathf.Pow((texA.GetPixel(column - (colFactor * i), row - (rowFactor * (i - 1)))[0] - average), 2);
                        sum += Mathf.Pow((texA.GetPixel(column - (colFactor * i), row - (rowFactor * i))[0] - average), 2);
                    }

                    average = 0.0f;
                    variances.Add(Mathf.Sqrt((1.0f / counter) * sum));
                    sum = 0.0f;
                    counter = 1;

                    aux++;
                }

                // ----------------------------------------------------------------------------------------------------------

                float m = variances[0];
                int j = 0;

                for (int i = 1; i < variances.Count; i++)
                {
                    if (m > variances[i])
                    {
                        m = variances[i];
                        j = i;
                    }
                }

                texture.SetPixel(column, row, new Color(averages[j], averages[j], averages[j]));
            }
        }

        forceDo = false;
    }

    public void Somboonkaew(int type)
    {

    }

    public void M1(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start;

        start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    float[] mat = new float[type * type];

                    int w = 0;
                    for (int i = -start; i <= start; i++)
                    {
                        for (int j = -start; j <= start; j++)
                        {
                            mat[w] = texA.GetPixel(column + i, row + j)[channel];
                            w++;
                        }
                    }

                    float x = mat[(int)Math.Floor(mat.Count() / 2.0f)];
                    float y = 0;

                    for (int i = 0; i < mat.Count(); i++)
                    {
                        y += mat[i];
                    }

                    y -= x;

                    sum[channel] = (x * ((type * type))) - y;
                }

                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }

        forceDo = false;
    }

    public void M2(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start;

        start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    float[] mat = new float[type * type];

                    int w = 0;
                    for (int i = -start; i <= start; i++)
                    {
                        for (int j = -start; j <= start; j++)
                        {
                            mat[w] = texA.GetPixel(column + i, row + j)[channel];
                            w++;
                        }
                    }

                    float x = mat[(int)Math.Floor(mat.Count() / 2.0f)];
                    float y = 0;

                    for (int i = 0; i < mat.Count(); i++)
                    {
                        if ((i + 1) % 2 == 0)
                        {
                            y += 2 * mat[i];
                        }
                        else
                        {
                            y += mat[i];
                        }
                    }

                    y -= x;

                    sum[channel] = (x * ((type * type) - 1)) - y;
                }

                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }

        forceDo = false;
    }

    public void M3(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start;

        start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    float[] mat = new float[type * type];

                    int w = 0;
                    for (int i = -start; i <= start; i++)
                    {
                        for (int j = -start; j <= start; j++)
                        {
                            mat[w] = texA.GetPixel(column + i, row + j)[channel];
                            w++;
                        }
                    }

                    float x = mat[(int)Math.Floor(mat.Count() / 2.0f)];
                    float y = 0;

                    for (int i = 0; i < mat.Count(); i++)
                    {
                        y += mat[i];
                    }

                    y -= x;

                    sum[channel] = (x * ((type * type) - 1)) - y;
                }

                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }

        forceDo = false;
    }

    public void H1(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start;

        start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    float[] mat = new float[type * type];

                    int w = 0;
                    for (int i = -start; i <= start; i++)
                    {
                        for (int j = -start; j <= start; j++)
                        {
                            mat[w] = texA.GetPixel(column + i, row + j)[channel];
                            w++;
                        }
                    }

                    float x = mat[(int)Math.Floor(mat.Count() / 2.0f)];
                    float y = 0;

                    for (int i = 0; i < mat.Count(); i++)
                    {
                        y += mat[i];
                    }

                    y -= x;

                    sum[channel] = (x * (type * type) - 1) - y;
                }

                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }

        forceDo = false;
    }

    public void H2(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start;

        start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column++)
        {
            for (int row = start; row < texture.height; row++)
            {
                float[] sum = new float[3];

                for (int channel = 0; channel < 3; channel++)
                {
                    float[] mat = new float[type * type];

                    int w = 0;
                    for (int i = -start; i <= start; i++)
                    {
                        for (int j = -start; j <= start; j++)
                        {
                            mat[w] = texA.GetPixel(column + i, row + j)[channel];
                            w++;
                        }
                    }

                    float x = mat[(int)Math.Floor(mat.Count() / 2.0f)];
                    float y = 0;

                    for (int i = 0; i < mat.Count(); i++)
                    {
                        y += mat[i];
                    }

                    y -= x;

                    sum[channel] = (x * ((type * type) - 1)) - y;
                }

                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }

        forceDo = false;
    }

    // Gets and Sets ------------------------------------------
    public bool GetState() { return state; }
    public void SetState(bool state) { this.state = state; }

    public int GetSize() { return size; }
    public void SetSize(int size) { this.size = size; }

    public int GetEffect() { return effect; }
    public void SetEffect(int effect) { this.effect = effect; }

    public bool GetForceDo() { return forceDo; }
    public void SetForceDo(bool forceDo) { this.forceDo = forceDo; }
    // --------------------------------------------------------

    // -------------------------------------------------------------------
}
