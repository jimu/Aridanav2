﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649 


public class Panel : MonoBehaviour
{
    // A single step
    [System.Serializable]
    public class Step
    {
        public string Notes;

        public string stepSpanish;
        public string stepEnglish;
        public GameObject[] stepObjects;
    }


    // List of all steps for this panel
    [SerializeField] public Step[] steps;

    // The current step
    int state;

    // List of steps text (mutli-line Text)
    Text stepsText;

    // List of all artwork contained in the steps for this panel
    // It also has a boolean flag that will used in SetState to
    // hide/show the artwork for the current step
    Dictionary<GameObject,bool> artwork = new Dictionary<GameObject, bool>();

    // Language
    static bool isSpanish = true;
    GameObject spanishButton, englishButton;



    void Awake()
    {
        FindAllArtwork();

        stepsText = GameObject.Find("StepsText").GetComponent<Text>();
        stepsText.text = "";

        InitLanguage();

        SetState(0);
    }


    // Store a list all artwork contained in the steps in Dictionary "artwork"
    void FindAllArtwork()
    {
        foreach (Step step in steps)
            foreach (GameObject art in step.stepObjects)
                artwork[art] = false;
    }


    // Called when a clickable object has been clicked. The game object name is supplied
    public void OnClick(GameObject art)
    {
        Debug.Log("OnClick(" + art.name + ") state=" + state);

        if (art == steps[state].stepObjects[0])
            SetState(state + 1);
    }


    void InitLanguage()
    {
        spanishButton = GameObject.Find("SpanishButton");
        englishButton = GameObject.Find("EnglishButton");
        SetSpanish(true);
    }


    public void SetSpanish(bool newIsSpanish)
    {
        isSpanish = newIsSpanish;
        spanishButton.SetActive(isSpanish);
        englishButton.SetActive(!isSpanish);

        if (stepsText.text != "")
        {
            stepsText.text = "";
            for (int i = 0; i <= state; ++i)
                DisplayStepText(i);
        }
    }


    // Show and hide artwork for this step
    // Also update step text
    void SetState(int newState)
    {
        state = newState;

        Debug.Log("SetState(" + state + ")");

        foreach (GameObject art in artwork.Keys.ToList())
            artwork[art] = false;

        foreach (GameObject art in steps[state].stepObjects)
            artwork[art] = true;

        foreach (var item in artwork)
            item.Key.SetActive(item.Value);

        DisplayStepText(state);
    }


    void DisplayStepText(int state)
    {
        string text = isSpanish ? steps[state].stepSpanish : steps[state].stepEnglish;
        if (!string.IsNullOrEmpty(text))
            stepsText.text += (state + 1).ToString() + $". {text}\n";
    }
}