using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // singleton 

    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null) { Debug.LogError("UI Manager _instance is null"); }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    // handle
    [SerializeField] private Text _collectableText;

    // global var
    private int _totalCollectables;

    private void Start()
    {
        _totalCollectables = GameObject.FindGameObjectsWithTag("Collectable").Length;
        _collectableText.text = "Collect Boxes: " + _totalCollectables;
    }


    public void UICollectableTextUpdate(int collectableCount)
    {
        _collectableText.text = "Collect Boxes: " + (_totalCollectables - collectableCount);
    }

}
