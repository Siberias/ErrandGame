﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniScript : MonoBehaviour {

    //input variables
    [SerializeField]
    private Text whiteboardText;
    [SerializeField]
    private Text laptopText;
    private List<string> words = new List<string>();
    [SerializeField]
    private TextAsset wordsAsset;
    private string[] possibleColours = new string[4] { "red", "blue", "green", "yellow" };
    private KeyCode[] keyInputs = new KeyCode[4] { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow };
    [SerializeField]
    private AudioClip correctSound;
    [SerializeField]
    private AudioClip incorrectSound;
    [SerializeField]
    private AudioClip endGameSound;
    [SerializeField]
    private AudioSource audioPlayer;

    //tracking variables
    private int wordCounter = 1;
    private int letterCounter = 1;
    [SerializeField]
    private int FinalWord = 10;
    int currentWordIndex;
    List<int> letterColours = new List<int>();//0 is left, 1 is up, 2 is down, 3 is right
    string currentWhiteboardWord;

	// Use this for initialization
	void Start () {

        words = new List<string>(wordsAsset.text.Split('\n'));

        //clear laptop screen
        laptopText.text = "";

        //assign the first word
        SetNextWord();
	}

    //when user types, update text
    private void Update() {
        
        if (CheckInput(0) || CheckInput(1) || CheckInput(2) || CheckInput(3)) {
            laptopText.text += words[currentWordIndex][(letterCounter - 1)];
            letterCounter++;

            if (letterCounter >= words[currentWordIndex].Length) {
                wordCounter++;
                StartCoroutine(WaitNextWord());

                audioPlayer.clip = correctSound;
                audioPlayer.Play();

                if (wordCounter >= FinalWord) {
                    endGameSound();
                }
            }

        } else if (Input.anyKeyDown) {

            //reset laptop text and counter
            laptopText.text = "";
            letterCounter = 1;

            audioPlayer.clip = incorrectSound;
            audioPlayer.Play();
        }
    }

    //sets the next whiteboard word
    void SetNextWord() {

        //reset text
        laptopText.text = "";
        letterCounter = 1;

        //select the next word
        currentWordIndex = Random.Range(0 + ((words.Count / FinalWord) *  (wordCounter - 1)), (words.Count / FinalWord) * wordCounter);

        //setup word colours
        letterColours = new List<int>();
        currentWhiteboardWord = "";
        int thisColour;

        for (int i = 0; i < words[currentWordIndex].Length; i++) {

            thisColour = Random.Range(0, possibleColours.Length);

            letterColours.Add(thisColour);
            currentWhiteboardWord += ("<Color=" + possibleColours[thisColour] + ">" + words[currentWordIndex][i] + "</Color> ");
        }

        whiteboardText.text = currentWhiteboardWord;
    }

    //check the given input
    private bool CheckInput(int direction) {

        if (Input.GetKeyDown(keyInputs[direction]) && letterColours[(letterCounter - 1)] == direction) {
            return true;
        }
        return false;
    }

    //ends this minigame
    private void EndGame() {

        audioPlayer.clip = endGameSound;

        audioPlayer.Play();
    }

    //wait before moving to next word
    IEnumerator WaitNextWord() {

        yield return new WaitForSeconds(0.1f);

        SetNextWord();
    }
}
