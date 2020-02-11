using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.Utils.ColorModels;
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
        M1, M2, M3, H1, H2, HIGHBOOST,
        BITPLANE,
        GLOBALLIMIAR, AVERAGELIMIAR, MEDIANLIMIAR, AVGMINMAXLIMIAR,
        NIBLACK,
        ROTATE, TRANSFORM,
        POINTDETECTION, LINEDETECTION,
        CMYK,
        ROBERTS, CROSSROBERTS, PREWITTGX, PREWITTGY, SOBELGX, SOBELGY, 
        KRISH, ROBISON, FREICHEN, LAPLACIANO,
        BRIGHTLOG, BRIGHTEXP, BRIGHTSQRT, BRIGHTSQARE,
        YUV, HSB
    }

    [Header("Objetos do editor")]
    public GameObject allImages;
    public GameObject imagePlaceHolder;
    //public GameObject loading;

    [Header("Objetos para efeitos")]
    public Slider offsetSlider;
    public TMP_InputField matInput;
    public TMP_InputField highBoostInput;
    public TMP_InputField highBoostMatrix;
    public Slider bitPlaneSlider;    
    public TMP_InputField niBlackMatrix;
    public Slider niBlackSlider;
    public TMP_InputField XInput;
    public TMP_InputField YInput;
    public int optionSelector = 0;
    public Slider lineDetectionSlider;
    public Slider contrastMin;
    public Slider contrastMax;

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
                Contrast(GetConstrastMin(), GetConstrastMax());
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
            
            case (int)Effects.HIGHBOOST:
                HighBoost(GetHighBoostMat(3));
                break;
            
            case (int)Effects.BITPLANE:
                BitPlane(GetPlaneSlider(1));
                break;
            
            case (int)Effects.GLOBALLIMIAR:
                GlobalLimiar();
                break;
            
            case (int)Effects.AVERAGELIMIAR:
                AverageLimiar(GetMat(3));
                break;
            
            case (int)Effects.MEDIANLIMIAR:
                MedianLimiar(GetMat(3));
                break;
            
            case (int)Effects.AVGMINMAXLIMIAR:
                AvgMinMaxLimiar(GetMat(3));
                break;
            
            case (int)Effects.NIBLACK:
                NiBlack(GetNiBlackMat(15));
                break;
            
            case (int)Effects.TRANSFORM:
                Transform(GetXInput(), GetYInput());
                break;
            
            case (int)Effects.POINTDETECTION:
                PointDetection();
                break;
            
            case (int)Effects.LINEDETECTION:
                LineDetection(optionSelector);
                break;
            
            case (int)Effects.CMYK:
                CMYK(optionSelector);
                break;
            
            case (int)Effects.ROBERTS:
                Roberts();
                break;
            
            case (int)Effects.CROSSROBERTS:
                CrossRoberts();
                break;
            
            case (int)Effects.PREWITTGX:
                PrewittGx();
                break;
            
            case (int)Effects.PREWITTGY:
                PrewittGy();
                break;
            
            case (int)Effects.SOBELGX:
                SobelGx();
                break;
            
            case (int)Effects.SOBELGY:
                SobelGy();
                break;
            
            case (int)Effects.KRISH:
                Krish();
                break;
            
            case (int)Effects.ROBISON:
                Robison();
                break;
            
            case (int)Effects.FREICHEN:
                Freichen();
                break;
            
            case (int)Effects.LAPLACIANO:
                Laplaciano(optionSelector);
                break;
            
            case (int)Effects.BRIGHTLOG:
                BrightnessLog();
                break;
            
            case (int)Effects.BRIGHTEXP:
                BrightnessExp();
                break;
            
            case (int)Effects.BRIGHTSQRT:
                BrightnessSqrt();
                break;
            
            case (int)Effects.BRIGHTSQARE:
                BrightnessSquare();
                break;
            
            case (int)Effects.YUV:
                YUV(optionSelector);
                break;
            
            case (int)Effects.HSB:
                BrightnessSquare();
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
    
    public int GetHighBoostMat(int standard)
    {
        if (highBoostMatrix.text == "")
        {
            highBoostMatrix.text = standard + "";
            return standard;
        }
        else
        {
            int value = Convert.ToInt32(highBoostMatrix.text, 10);

            if (value % 2 == 0)
            {
                value--;
                highBoostMatrix.text = value + "";
            }
            else if (value < 0){
                value = standard;
                highBoostMatrix.text = value + "";
            }

            GetHighBoostInput(1);
            
            return value;
        }
    }
    
    public void GetHighBoostInput(int standard)
    {
        if (highBoostInput.text == "")
        {
            highBoostInput.text = standard + "";
        }
    }

    public int GetPlaneSlider(int standard)
    {
        if ((int) bitPlaneSlider.value == 0)
        {
            bitPlaneSlider.value = standard;
        }

        return (int)bitPlaneSlider.value;
    }

    public int GetNiBlackMat(int standard)
    {
        if (niBlackMatrix.text == "")
        {
            niBlackMatrix.text = standard + "";
            return standard;
        }

        int value = Convert.ToInt32(niBlackMatrix.text, 10);

        if (value % 2 == 0)
        {
            value--;
            niBlackMatrix.text = value + "";
        }
        else if (value < 0){
            value = standard;
            niBlackMatrix.text = value + "";
        }

        return value;
    }

    public int GetXInput()
    {
        if (XInput.text == "")
        {
            XInput.text = "0";
        }

        return Int32.Parse(XInput.text);
    }

    public int GetYInput()
    {
        if (YInput.text == "")
        {
            YInput.text = "0";
        }

        return Int32.Parse(YInput.text);
    }

    public void SetLineAngle(int lineAngle)
    {
        this.optionSelector = lineAngle;
    }

    public float GetConstrastMin()
    {
        if (contrastMin.value > contrastMax.value)
        {
            contrastMin.value = contrastMax.value;
        }

        return contrastMin.value;
    }

    public float GetConstrastMax()
    {
        return contrastMax.value;
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
                    float pixelA = texA.GetPixel(row, column)[channel];
                    float pixelB = texB.GetPixel(row, column)[channel];
                    float pixel = pixelA + pixelB;

                    sum[channel] = pixel;
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
                    float pixelA = texA.GetPixel(row, column)[channel];
                    float pixelB = texB.GetPixel(row, column)[channel];
                    float pixel = pixelA - pixelB;

                    sum[channel] = pixel;
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
                    float pixelA = texA.GetPixel(row, column)[channel];
                    float pixelB = texB.GetPixel(row, column)[channel];
                    float pixel = pixelA * pixelB;

                    sum[channel] = pixel;
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
                    float pixelA = texA.GetPixel(row, column)[channel];
                    float pixelB = texB.GetPixel(row, column)[channel];
                    float pixel = 0;

                    if (pixelB != 0.0f)
                    {
                        pixel = pixelA / pixelB;
                    }

                    sum[channel] = pixel;
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
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 10000);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 10000);
                    int pixel = pixelA & pixelB;

                    sum[channel] = pixel / 10000.0f;
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
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 10000);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 10000);
                    int pixel = pixelA | pixelB;

                    sum[channel] = pixel / 10000.0f;
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
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 10000);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 10000);
                    int pixel = pixelA ^ pixelB;

                    sum[channel] = pixel / 10000.0f;
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

    public void Contrast(float min, float max)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        float hi = 0;
        float lo = 1;

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
                    else if (pixel < lo)
                    {
                        lo = pixel;
                    }
                }
            }
        }
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                
                for (int channel = 0; channel < 3; channel++)
                {
                    float pixel  = texA.GetPixel(column, row)[channel];
                    
                    sum[channel] = ((max - min)/(hi - lo)) * (pixel - lo) + min;
                }

                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }

        forceDo = false;
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

                if (pixel > previous + offsetSlider.value || pixel < previous - offsetSlider.value)
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

                if (pixel > previous + offsetSlider.value || pixel < previous - offsetSlider.value)
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

                    x *= (type * type);
                    
                    sum[channel] = x - y;
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

                    float x = mat[(int)Math.Floor(mat.Count() / 2.0f)] * (type + type - 1);
                    float y = 0;

                    for (int i = 0; i < mat.Count(); i++)
                    {
                        if ((i + 1) % 2 == 0)
                        {
                            y += (type - 1) * mat[i];
                        }
                        else
                        {
                            x += mat[i];
                        }
                    }

                    sum[channel] = x - y;
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

                    float x = mat[(int)Math.Floor(mat.Count() / 2.0f)] * (type + type - 1);
                    float y = 0;

                    for (int i = 0; i < mat.Count(); i++)
                    {
                        if ((i + 1) % 2 == 0)
                        {
                            y += mat[i];
                        }
                    }

                    sum[channel] = x - y;
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

                    float x = mat[(int)Math.Floor(mat.Count() / 2.0f)] * (type + 1);
                    float y = 0;

                    for (int i = 0; i < mat.Count(); i++)
                    {
                        if ((i + 1) % 2 == 0)
                        {
                            y += mat[i];
                        }
                    }

                    sum[channel] = x - y;
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

                    x *= ((type * type) - 1); 
                    
                    sum[channel] = x - y;
                }

                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }

        forceDo = false;
    }
    
    public void HighBoost(int type)
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

                    x *= ((type * type) * Int32.Parse(highBoostInput.text)) - 1;
                    
                    sum[channel] = x - y;
                }

                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }

        forceDo = false;
    }

    public void BitPlane(int plane)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                int pixel = (int)(texA.GetPixel(column, row)[0] * 255);

                int valueInt = pixel & (int)Mathf.Pow(plane, 2);

                float value = valueInt / 255.0f;

                texture.SetPixel(column, row, new Color(value, value, value));
            }
        }

        forceDo = false;
    }

    public void GlobalLimiar()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                if (texA.GetPixel(column, row)[0] < offsetSlider.value)
                {
                    texture.SetPixel(column, row, new Color(0,0,0));
                }
                else
                {
                    texture.SetPixel(column, row, new Color(1,1,1));
                }
            }
        }

        forceDo = false;
    }
    
    public void AverageLimiar(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column+=3)
        {
            for (int row = start; row < texture.height; row+=3)
            {
                float sum = 0.0f;

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        sum += texA.GetPixel(averageCol + column, averageRow + row)[0];
                    }
                }

                float average = sum / (type * type);

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        if (texA.GetPixel(averageCol + column, averageRow + row)[0] < average)
                        {
                            texture.SetPixel(averageCol + column, averageRow + row, new Color(0,0,0));
                        }
                        else
                        {
                            texture.SetPixel(averageCol + column, averageRow + row, new Color(1,1,1));
                        }
                    }
                }
            }
        }

        forceDo = false;
    }
    
    public void MedianLimiar(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column+=3)
        {
            for (int row = start; row < texture.height; row+=3)
            {
                List<float> pixels = new List<float>();

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        pixels.Add(texA.GetPixel(averageCol + column, averageRow + row)[0]);
                    }
                }

                pixels.Sort();
                float median = pixels[(int)Math.Floor((type * type) / 2.0f)];

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        if (texA.GetPixel(averageCol + column, averageRow + row)[0] < median)
                        {
                            texture.SetPixel(averageCol + column, averageRow + row, new Color(0,0,0));
                        }
                        else
                        {
                            texture.SetPixel(averageCol + column, averageRow + row, new Color(1,1,1));
                        }
                    }
                }
            }
        }

        forceDo = false;
    }
    
    public void AvgMinMaxLimiar(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column+=type)
        {
            for (int row = start; row < texture.height; row+=type)
            {
                List<float> pixels = new List<float>();

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        pixels.Add(texA.GetPixel(averageCol + column, averageRow + row)[0]);
                    }
                }

                pixels.Sort();
                float average = (pixels[0] + pixels[pixels.Count - 1])/2;

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        if (texA.GetPixel(averageCol + column, averageRow + row)[0] < average)
                        {
                            texture.SetPixel(averageCol + column, averageRow + row, new Color(0,0,0));
                        }
                        else
                        {
                            texture.SetPixel(averageCol + column, averageRow + row, new Color(1,1,1));
                        }
                    }
                }
            }
        }

        forceDo = false;
    }
    
    public void NiBlack(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start = (int)Math.Floor(type / 2.0f);

        for (int column = start; column < texture.width; column+=type)
        {
            for (int row = start; row < texture.height; row+=type)
            {
                List<float> pixels = new List<float>();

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        pixels.Add(texA.GetPixel(averageCol + column, averageRow + row)[0]);
                    }
                }
                
                float average = pixels.Average();

                float sum = 0.0f;

                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        sum += Mathf.Pow(texA.GetPixel(column, row)[0] - average, 2);
                    }
                }

                float deviation = Mathf.Sqrt(sum / (type * type));

                float value = average + (niBlackSlider.value * deviation);
                
                for (int averageRow = -start; averageRow <= start; averageRow++)
                {
                    for (int averageCol = -start; averageCol <= start; averageCol++)
                    {
                        if (texA.GetPixel(averageCol + column, averageRow + row)[0] < value)
                        {
                            texture.SetPixel(averageCol + column, averageRow + row, new Color(0,0,0));
                        }
                        else
                        {
                            texture.SetPixel(averageCol + column, averageRow + row, new Color(1,1,1));
                        }
                    }
                }

                texture.SetPixel(column, row, new Color(value,value,value));
            }
        }

        forceDo = false;
    }

    public void Transform(int x, int y)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
        
        if (x < 0)
        {
            if (y < 0)
            {
                for (int column = texture.width - 1 + x; column > 0; column--)
                {
                    for (int row = texture.height - 1 + y; row > 0; row--)
                    {
                        float[] sum = new float[3];
                        
                        for (int channel = 0; channel < 3; channel++)
                        {
                            sum[channel] = texA.GetPixel(column - x, row - y)[channel];
                        }
                        
                        texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
                    }
                }
            }
            else
            {
                for (int column = texture.width - 1 + x; column > 0; column--)
                {
                    for (int row = y; row < texture.height; row++)
                    {
                        float[] sum = new float[3];
                        
                        for (int channel = 0; channel < 3; channel++)
                        {
                            sum[channel] = texA.GetPixel(column - x, row - y)[channel];
                        }
                        
                        texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
                    }
                }
            }
        }
        else
        {
            if (y < 0)
            {
                for (int column = x; column < texture.width; column++)
                {
                    for (int row = texture.height - 1 + y; row > 0; row--)
                    {
                        float[] sum = new float[3];
                        
                        for (int channel = 0; channel < 3; channel++)
                        {
                            sum[channel] = texA.GetPixel(column - x, row - y)[channel];
                        }
                        
                        texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
                    }
                }
            }
            else
            {
                for (int column = x; column < texture.width; column++)
                {
                    for (int row = y; row < texture.height; row++)
                    {
                        float[] sum = new float[3];
                        
                        for (int channel = 0; channel < 3; channel++)
                        {
                            sum[channel] = texA.GetPixel(column - x, row - y)[channel];
                        }
                        
                        texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
                    }
                }
            }
        }

        forceDo = false;
    }

    public void PointDetection()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start;

        int type = 3;
        
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

                    x *= ((type * type) - 1); 
                    
                    float value = x - y;

                    sum[channel] = 0;
                    if (value > offsetSlider.value)
                    {
                        sum[channel] = value;
                    }
                }

                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }

        forceDo = false;
    } 
    
    public void LineDetection(int angle)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start;

        int type = 3;
        
        start = (int)Math.Floor(type / 2.0f);

        if (angle == 90)
        {
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

                        float x = mat[4] + mat[5] + mat[6];
                        float y = 0;

                        for (int i = 0; i < mat.Count(); i++)
                        {
                            y += mat[i];
                        }

                        y -= x;

                        x *= 2; 
                    
                        float value = x - y;

                        sum[channel] = 0;
                        if (value > lineDetectionSlider.value)
                        {
                            sum[channel] = value;
                        }
                    }

                    texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
                }
            }
        }
        else if (angle == 0)
        {
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

                        float x = mat[1] + mat[4] + mat[7];
                        float y = 0;

                        for (int i = 0; i < mat.Count(); i++)
                        {
                            y += mat[i];
                        }

                        y -= x;

                        x *= 2; 
                    
                        float value = x - y;

                        sum[channel] = 0;
                        if (value > offsetSlider.value)
                        {
                            sum[channel] = value;
                        }
                    }

                    texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
                }
            }

        }
        else if (angle == 135)
        {
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

                        float x = mat[6] + mat[4] + mat[2];
                        float y = 0;

                        for (int i = 0; i < mat.Count(); i++)
                        {
                            y += mat[i];
                        }

                        y -= x;

                        x *= 2; 
                    
                        float value = x - y;

                        sum[channel] = 0;
                        if (value > offsetSlider.value)
                        {
                            sum[channel] = value;
                        }
                    }

                    texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
                }
            }
        }
        else if (angle == 45)
        {
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

                        float x = mat[0] + mat[4] + mat[8];
                        float y = 0;

                        for (int i = 0; i < mat.Count(); i++)
                        {
                            y += mat[i];
                        }

                        y -= x;

                        x *= 2; 
                    
                        float value = x - y;

                        sum[channel] = 0;
                        if (value > offsetSlider.value)
                        {
                            sum[channel] = value;
                        }
                    }

                    texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
                }
            }
        }
        
        forceDo = false;
    }

    public void CMYK(int color)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float r = texA.GetPixel(column, row)[0];
                float g = texA.GetPixel(column, row)[1];
                float b = texA.GetPixel(column, row)[2];

                float k = 1 - Mathf.Max(Mathf.Max(r, g), b);
                float c = (1 - r - k) / (1 - k);
                float m = (1 - g - k) / (1 - k);
                float y = (1 - b - k) / (1 - k);


                switch (color)
                {
                    case 0:
                        texture.SetPixel(column, row, new Color(0, c, c));
                        break;
                    
                    case 1:
                        texture.SetPixel(column, row, new Color(m, 0, m));
                        break;
                    
                    case 2:
                        texture.SetPixel(column, row, new Color(y, y, 0));
                        break;
                    
                    case 3:
                        texture.SetPixel(column, row, new Color(k, k, k));
                        break;
                    
                }
            }
        }
    }

    public void Roberts()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                
                for (int channel = 0; channel < 3; channel++)
                {
                    float[] pixel = new float[4];

                    int k = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            pixel[k] = texA.GetPixel(column + i, row + j)[channel];
                            
                            k++;
                        }
                    }

                    float x = pixel[0] - pixel[2];
                    float y = pixel[0] - pixel[1];

                    sum[channel] = Mathf.Abs(x) + Mathf.Abs(y);
                }
                
                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        Clean();
    }
    
    public void CrossRoberts()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                
                for (int channel = 0; channel < 3; channel++)
                {
                    float[] pixel = new float[4];

                    int k = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            pixel[k] = texA.GetPixel(column + i, row + j)[channel];
                            
                            k++;
                        }
                    }

                    float x = pixel[0] - pixel[3];
                    float y = pixel[1] - pixel[2];

                    sum[channel] = Mathf.Abs(x) + Mathf.Abs(y);
                }
                
                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        Clean();
    }
    
    public void PrewittGx()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                
                for (int channel = 0; channel < 3; channel++)
                {
                    float[] pixel = new float[9];

                    int k = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            pixel[k] = texA.GetPixel(column + i, row + j)[channel];
                            
                            k++;
                        }
                    }

                    float x = pixel[2] + pixel[5] + pixel[8];
                    float y = pixel[0] + pixel[3] + pixel[6];

                    sum[channel] = x - y;
                }
                
                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }

        Clean();
    }
    
    public void PrewittGy()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                
                for (int channel = 0; channel < 3; channel++)
                {
                    float[] pixel = new float[9];

                    int k = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            pixel[k] = texA.GetPixel(column + i, row + j)[channel];
                            
                            k++;
                        }
                    }

                    float x = pixel[6] + pixel[7] + pixel[8];
                    float y = pixel[1] + pixel[2] + pixel[3];

                    sum[channel] = x - y;
                }
                
                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        Clean();
    }
    
    public void SobelGx()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                
                for (int channel = 0; channel < 3; channel++)
                {
                    float[] pixel = new float[9];

                    int k = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            pixel[k] = texA.GetPixel(column + i, row + j)[channel];
                            
                            k++;
                        }
                    }

                    float x = pixel[2] + (2 * pixel[5]) + pixel[8];
                    float y = pixel[0] + (2 * pixel[3]) + pixel[6];

                    sum[channel] = x - y;
                }
                
                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        Clean();
    }
    
    public void SobelGy()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                
                for (int channel = 0; channel < 3; channel++)
                {
                    float[] pixel = new float[9];

                    int k = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            pixel[k] = texA.GetPixel(column + i, row + j)[channel];
                            
                            k++;
                        }
                    }

                    float x = pixel[6] + (2 * pixel[7]) + pixel[8];
                    float y = pixel[0] + (2 * pixel[1]) + pixel[2];

                    sum[channel] = x - y;
                }
                
                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        Clean();
    }
    
    public void Krish()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                
                for (int channel = 0; channel < 3; channel++)
                {
                    float[] pixel = new float[9];

                    int k = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            pixel[k] = texA.GetPixel(column + i, row + j)[channel];
                            
                            k++;
                        }
                    }

                    List<float> masks = new List<float>();

                    float x = 5 * (pixel[0] + pixel[3] + pixel[6]);
                    float y = 3 * (pixel[1] + pixel[2] + pixel[5] + pixel[7] + pixel[8]);
                    masks.Add(x - y);
                    
                    x = 5 * (pixel[3] + pixel[6] + pixel[7]);
                    y = 3 * (pixel[0] + pixel[1] + pixel[2] + pixel[5] + pixel[8]);
                    masks.Add(x - y);
                    
                    x = 5 * (pixel[6] + pixel[7] + pixel[8]);
                    y = 3 * (pixel[0] + pixel[1] + pixel[2] + pixel[3] + pixel[5]);
                    masks.Add(x - y);
                    
                    x = 5 * (pixel[5] + pixel[7] + pixel[8]);
                    y = 3 * (pixel[0] + pixel[1] + pixel[2] + pixel[3] + pixel[6]);
                    masks.Add(x - y);
                    
                    x = 5 * (pixel[3] + pixel[5] + pixel[8]);
                    y = 3 * (pixel[0] + pixel[1] + pixel[3] + pixel[6] + pixel[7]);
                    masks.Add(x - y);
                    
                    x = 5 * (pixel[1] + pixel[2] + pixel[5]);
                    y = 3 * (pixel[0] + pixel[3] + pixel[6] + pixel[7] + pixel[8]);
                    masks.Add(x - y);
                    
                    x = 5 * (pixel[0] + pixel[1] + pixel[2]);
                    y = 3 * (pixel[3] + pixel[5] + pixel[6] + pixel[7] + pixel[8]);
                    masks.Add(x - y);
                    
                    x = 5 * (pixel[0] + pixel[1] + pixel[3]);
                    y = 3 * (pixel[2] + pixel[5] + pixel[6] + pixel[7] + pixel[8]);
                    masks.Add(x - y);

                    sum[channel] = masks.Max();
                }
                
                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        Clean();
    }    
    
    public void Robison()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                
                for (int channel = 0; channel < 3; channel++)
                {
                    float[] pixel = new float[9];

                    int k = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            pixel[k] = texA.GetPixel(column + i, row + j)[channel];
                            
                            k++;
                        }
                    }

                    List<float> masks = new List<float>();

                    float x = pixel[0] + 2*pixel[3] + pixel[6];
                    float y = pixel[2] + 2*pixel[5] + pixel[8];
                    masks.Add(x - y);

                    x = pixel[3] + 2*pixel[6] + pixel[7];
                    y = pixel[1] + 2*pixel[2] + pixel[5];
                    masks.Add(x - y);

                    x = pixel[6] + 2*pixel[7] + pixel[8];
                    y = pixel[0] + 2*pixel[1] + pixel[2];
                    masks.Add(x - y);

                    x = pixel[7] + 2*pixel[8] + pixel[5];
                    y = pixel[3] + 2*pixel[0] + pixel[1];
                    masks.Add(x - y);

                    x = pixel[2] + 2*pixel[5] + pixel[8];
                    y = pixel[0] + 2*pixel[3] + pixel[6];
                    masks.Add(x - y);

                    x = pixel[2] + 2*pixel[5] + pixel[8];
                    y = pixel[0] + 2*pixel[3] + pixel[6];
                    masks.Add(x - y);

                    x = pixel[1] + 2*pixel[2] + pixel[5];
                    y = pixel[3] + 2*pixel[6] + pixel[7];
                    masks.Add(x - y);

                    x = pixel[0] + 2*pixel[1] + pixel[2];
                    y = pixel[6] + 2*pixel[7] + pixel[8];
                    masks.Add(x - y);

                    x = pixel[3] + 2*pixel[0] + pixel[1];
                    y = pixel[7] + 2*pixel[8] + pixel[5];
                    masks.Add(x - y);

                    sum[channel] = masks.Max();
                }
                
                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        Clean();
    }    
    
    public void Freichen()
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                
                for (int channel = 0; channel < 3; channel++)
                {
                    float[] pixel = new float[9];

                    int k = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            pixel[k] = texA.GetPixel(column + i, row + j)[channel];
                            
                            k++;
                        }
                    }

                    List<float> masks = new List<float>();

                    float x = pixel[0] + Mathf.Sqrt(2)*pixel[1] + pixel[2];
                    float y = pixel[6] + Mathf.Sqrt(2)*pixel[7] + pixel[8];
                    masks.Add(x - y);

                    x = pixel[0] + Mathf.Sqrt(2)*pixel[3] + pixel[6];
                    y = pixel[2] + Mathf.Sqrt(2)*pixel[5] + pixel[8];
                    masks.Add(x - y);

                    x = pixel[3] + Mathf.Sqrt(2)*pixel[2] + pixel[7];
                    y = pixel[1] + Mathf.Sqrt(2)*pixel[6] + pixel[5];
                    masks.Add(x - y);

                    x = pixel[7] + Mathf.Sqrt(2)*pixel[0] + pixel[5];
                    y = pixel[1] + Mathf.Sqrt(2)*pixel[8] + pixel[3];
                    masks.Add(x - y);

                    x = pixel[1] + pixel[7];
                    y = pixel[3] + pixel[5];
                    masks.Add(x - y);

                    x = pixel[2] + pixel[6];
                    y = pixel[0] + pixel[8];
                    masks.Add(x - y);

                    x = pixel[0] + pixel[2] + (pixel[4] * 4) + pixel[6] + pixel[8];
                    y = pixel[1] + pixel[3] + pixel[5] + pixel[7];
                    masks.Add(x - y);

                    x = pixel[1] + pixel[3] + (pixel[4] * 4) + pixel[5] + pixel[7];
                    y = pixel[0] + pixel[2] + pixel[6] + pixel[8];
                    masks.Add(x - y);
                    
                    masks.Add(pixel.Sum());

                    sum[channel] = masks.Max();
                }
                
                texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        Clean();
    }

    public void Laplaciano(int type)
    {
        Image a = images[0];

        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);

        int start;

        start = (int)Math.Floor(3 / 2.0f);

        switch(type)
        {
            case 1:
                for (int column = start; column < texture.width; column++)
                {
                    for (int row = start; row < texture.height; row++)
                    {
                        float[] sum = new float[3];

                        for (int channel = 0; channel < 3; channel++)
                        {
                            float[] pixels = new float[9];

                            int w = 0;
                            for (int i = -start; i <= start; i++)
                            {
                                for (int j = -start; j <= start; j++)
                                {
                                    pixels[w] = texA.GetPixel(column + i, row + j)[channel];
                                    w++;
                                }
                            }

                            float x = 4 * pixels[4];
                            float y = 0;

                            for (int i = 0; i < pixels.Count(); i++)
                            {
                                if ((i + 1) % 2 == 0)
                                {
                                    y += pixels[i];
                                }
                            }

                            sum[channel] = x - y;
                        }

                        texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
                    }
                }
                break;
            
            case 2:
                for (int column = start; column < texture.width; column++)
                {
                    for (int row = start; row < texture.height; row++)
                    {
                        float[] sum = new float[3];

                        for (int channel = 0; channel < 3; channel++)
                        {
                            float[] pixel = new float[9];

                            int w = 0;
                            for (int i = -start; i <= start; i++)
                            {
                                for (int j = -start; j <= start; j++)
                                {
                                    pixel[w] = texA.GetPixel(column + i, row + j)[channel];
                                    w++;
                                }
                            }

                            float x = 20 * pixel[4];
                            float y = 4 * (pixel[1] + pixel[3] + pixel[5] + pixel[7]) + (pixel[0] + pixel[2] + pixel[6] + pixel[8]);

                            sum[channel] = x - y;
                        }

                        texture.SetPixel(column, row, new Color(sum[0], sum[1], sum[2]));
                    }
                }
                break;
        }
        
        forceDo = false;
    }

    public void BrightnessLog()
    {
        {
            Image a = images[0];
     
            var texA = a.sprite.texture;
            texture = new Texture2D(texA.width, texA.height);
     
            float hi = 0.0f;
                 
            for (int column = 0; column < texture.width; column++)
            {
                for (int row = 0; row < texture.height; row++)
                {
                    for (int channel = 0; channel < 3; channel++)
                    {
                        float pixel = texA.GetPixel(column, row)[channel];
                             
                        if (hi < pixel)
                        {
                            hi = pixel;
                        }
                    }
                }
            }
     
            float x = (int)(255.0f / Mathf.Log(1.0f + (hi*255.0f)));
                 
            for (int column = 0; column < texture.width; column++)
            {
                for (int row = 0; row < texture.height; row++)
                {
                    float[] sum = new float[3];
                     
                    for (int channel = 0; channel < 3; channel++)
                    {
                        float pixel = texA.GetPixel(column, row)[channel] * 255;
     
                        sum[channel] = x * Mathf.Log(pixel + 1);
                    }
                     
                    texture.SetPixel(column, row, new Color(sum[0]/255, sum[1]/255, sum[2]/255));
                }
            }
             
            Clean();
        }
    }     
    
    public void BrightnessExp()
    {
        {
            Image a = images[0];
     
            var texA = a.sprite.texture;
            texture = new Texture2D(texA.width, texA.height);
     
            float hi = 0.0f;
                 
            for (int column = 0; column < texture.width; column++)
            {
                for (int row = 0; row < texture.height; row++)
                {
                    for (int channel = 0; channel < 3; channel++)
                    {
                        float pixel = texA.GetPixel(column, row)[channel];
                             
                        if (hi < pixel)
                        {
                            hi = pixel;
                        }
                    }
                }
            }
     
            float x = (int)(255.0f / Mathf.Exp(1.0f + (hi*255.0f)));
                 
            for (int column = 0; column < texture.width; column++)
            {
                for (int row = 0; row < texture.height; row++)
                {
                    float[] sum = new float[3];
                     
                    for (int channel = 0; channel < 3; channel++)
                    {
                        float pixel = texA.GetPixel(column, row)[channel] * 255;
     
                        sum[channel] = x * (Mathf.Exp(pixel) + 1);
                    }
                     
                    texture.SetPixel(column, row, new Color(sum[0]/255, sum[1]/255, sum[2]/255));
                }
            }
             
            Clean();
        }
    }       
    
    public void BrightnessSqrt()
    {
        Image a = images[0];
     
        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
     
        float hi = 0.0f;
                 
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                for (int channel = 0; channel < 3; channel++)
                {
                    float pixel = texA.GetPixel(column, row)[channel];
                             
                    if (hi < pixel)
                    {
                        hi = pixel;
                    }
                }
            }
        }
     
        float x = (int)(255.0f / Mathf.Sqrt(1.0f + (hi*255.0f)));
                 
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                     
                for (int channel = 0; channel < 3; channel++)
                {
                    float pixel = texA.GetPixel(column, row)[channel] * 255;
     
                    sum[channel] = x * Mathf.Sqrt(pixel);
                }
                     
                texture.SetPixel(column, row, new Color(sum[0]/255, sum[1]/255, sum[2]/255));
            }
        }
             
        Clean();
    }      
    
    public void BrightnessSquare()
    {
        Image a = images[0];
     
        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
     
        float hi = 0.0f;
                 
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                for (int channel = 0; channel < 3; channel++)
                {
                    float pixel = texA.GetPixel(column, row)[channel];
                             
                    if (hi < pixel)
                    {
                        hi = pixel;
                    }
                }
            }
        }
     
        float x = (int)(255.0f / Mathf.Pow(1.0f + (hi*255.0f), 2));
                 
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float[] sum = new float[3];
                     
                for (int channel = 0; channel < 3; channel++)
                {
                    float pixel = texA.GetPixel(column, row)[channel] * 255;
     
                    sum[channel] = x * Mathf.Pow(pixel, 2);
                }
                     
                texture.SetPixel(column, row, new Color(sum[0]/255, sum[1]/255, sum[2]/255));
            }
        }
             
        Clean();
    } 
    
    public void YUV(int color)
    {
        Image a = images[0];
     
        var texA = a.sprite.texture;
        texture = new Texture2D(texA.width, texA.height);
     
        float hi = 0.0f;
                 
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
            {
                float r = texA.GetPixel(column, row)[0];
                float g = texA.GetPixel(column, row)[1];
                float b = texA.GetPixel(column, row)[2];

                float y = (0.299f * r) + (0.587f * g) + (0.114f * b);
                float u = 0.492f * (b - y);
                float v = 0.877f * (r - y);
                
                switch (color)
                {
                    case 0:
                        texture.SetPixel(column, row, new Color(y, y, y));
                        break;
                    
                    case 1:
                        texture.SetPixel(column, row, new Color(u, u, 0));
                        break;
                    
                    case 2:
                        texture.SetPixel(column, row, new Color(0, v, v));
                        break;
                }
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
