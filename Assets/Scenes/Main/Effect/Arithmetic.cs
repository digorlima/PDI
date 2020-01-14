using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arithmetic : MonoBehaviour
{
    public static Texture2D Add(Image a, Image b) {
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
        
        return texSum;
    }
    
    public static Texture2D Sub(Image a, Image b) {
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
        
        return texSum;
    }
    
    public static Texture2D Mult(Image a, Image b) {
        float[] sum = new float[3];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        var texSum = new Texture2D(texA.width, texA.height);

        for(int row = 0; row < texA.width; row++) {
            for (int column = 0; column < texA.height; column++) {
                for (int channel = 0; channel < 3; channel++) {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);
                    int pixel = pixelA * pixelB;

                    sum[channel] = pixel / 255.0f;
                }

                texSum.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        texSum.Apply();
        
        return texSum;
    }
    
    public static Texture2D Div(Image a, Image b) {
        float[] sum = new float[3];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        var texSum = new Texture2D(texA.width, texA.height);

        for(int row = 0; row < texA.width; row++) {
            for (int column = 0; column < texA.height; column++) {
                for (int channel = 0; channel < 3; channel++) {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);

                    int pixel = 0;

                    if(pixelA != 0 && pixelB != 0){
                        pixel = pixelA / pixelB;
                    }
                    
                    sum[channel] = pixel / 255.0f;
                }

                texSum.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        texSum.Apply();
        
        return texSum;
    }

    public static Texture2D And(Image a, Image b) {
        float[] sum = new float[3];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        var texSum = new Texture2D(texA.width, texA.height);

        for(int row = 0; row < texA.width; row++) {
            for (int column = 0; column < texA.height; column++) {
                for (int channel = 0; channel < 3; channel++) {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);
                    int pixel = pixelA & pixelB;

                    sum[channel] = pixel / 255.0f;
                }

                texSum.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        texSum.Apply();
        
        return texSum;
    }
    
    public static Texture2D Or(Image a, Image b) {
        float[] sum = new float[3];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        var texSum = new Texture2D(texA.width, texA.height);

        for(int row = 0; row < texA.width; row++) {
            for (int column = 0; column < texA.height; column++) {
                for (int channel = 0; channel < 3; channel++) {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);
                    int pixel = pixelA | pixelB;

                    sum[channel] = pixel / 255.0f;
                }

                texSum.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        texSum.Apply();
        
        return texSum;
    }

    public static Texture2D Xor(Image a, Image b) {
        float[] sum = new float[3];

        var texA = a.sprite.texture;
        var texB = b.sprite.texture;

        var texSum = new Texture2D(texA.width, texA.height);

        for(int row = 0; row < texA.width; row++) {
            for (int column = 0; column < texA.height; column++) {
                for (int channel = 0; channel < 3; channel++) {
                    int pixelA = (int)(texA.GetPixel(row, column)[channel] * 255);
                    int pixelB = (int)(texB.GetPixel(row, column)[channel] * 255);
                    int pixel = pixelA ^ pixelB;

                    sum[channel] = pixel / 255.0f;
                }

                texSum.SetPixel(row, column, new Color(sum[0], sum[1], sum[2]));
            }
        }
        
        texSum.Apply();
        
        return texSum;
    }
}
