using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public void CloseImage()
    {
        Destroy(gameObject.transform.parent.transform.parent.gameObject);
    }
}
