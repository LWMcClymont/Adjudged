using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

using TMPro;

public class HUD : MonoBehaviour {

    // Container for our messages strings
    public string[] messages;

    // UI Text element
    //public Text textObject;

    //private TextMesh textObject;

    private TextMeshPro textObject;

    // Maximum display speed of text
    public float displaySpeedMax = 0.3f;

    // Minimum display speed of text
    public float displaySpeedMin = 0.1f;

    // Next game time when the next character will be displayed
    private float nextCharacterTime = -1;

    // Current message index in message container
    private int messageIndex = -1;

    // Current character index in current message
    private int curCharacterIndex = -1;

    // Audio that'll be played as text is appearing on screen
    public AudioSource audioSource;

    void Start()
    {
        // If we don't have a text object assigned then display a message
        if (textObject == null)
        {
            // Display error message
            print("No text component attached to HUD script!");
        }

        // If we don't have an audioSource then display a message
        if (audioSource == null)
        {
            // Display error message
            print("No audio source attached to HUD script!");
        }

        //textObject = GetComponent<TextMesh>();

        textObject = GetComponent<TextMeshPro>();

        // Clear text from the textObject
        ClearText();
    }

    //void LateUpdate()
    //{
    //
    //    textObject.transform.LookAt (-player.position);
    //}

    void Update()
    {
        // When it's time to print the next character and the current message index is valid
        if (Time.time > nextCharacterTime && messageIndex != -1)
        {
            // Set the next character display time
            nextCharacterTime = Time.time + Random.Range(displaySpeedMin, displaySpeedMax);

            // Add the next character to the textObject
            textObject.text += (messages[messageIndex][curCharacterIndex]);
            
            // If our audioSource exists then play the sound
            if (audioSource != null)
            {
                // Play the sound 
                PlaySound();
            }

            // If we've reached the end of the message
            if ((curCharacterIndex + 1) >= messages[messageIndex].Length)
            {
                // Set the current character index to a sentinal value
                curCharacterIndex = -1;

                // Set the message index to a sentinal value
                messageIndex = -1;

                // Break out of the function
                return;
            }

            // Increment the character Index
            ++curCharacterIndex;
        }
    }

    // Setup to start displaying the indicated message
    public void DisplayMessage(int index)
    {
        // if the index is invalid display error
        if (index >= messages.Length)
        {
            // Display and error message
            print("Message index is invalid on HUD script!");

            // Exit out of this function
            return;
        }

        // Clear the text from textObject
        ClearText();

        // Set the next character display time
        nextCharacterTime = Time.time + Random.Range(displaySpeedMin, displaySpeedMax);

        // Set the message index
        messageIndex = index;

        // Set the current character index to the beginning of the message
        curCharacterIndex = 0;
    }

    // Clear text from the text object
    public void ClearText()
    {
        // Make sure our textObject is valid
        if (textObject != null)
        {
            // Set the text to nothing
            textObject.text = "";
        }
    }

    // Play sound
    private void PlaySound()
    {
        // Play the sound 
        audioSource.Play();
    }
}
