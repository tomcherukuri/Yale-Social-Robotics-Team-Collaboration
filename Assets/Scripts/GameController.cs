using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

	public Text countdownTimer;
	public int roundTimeGiven;
	public int roundsGiven;

	private float timeElapsed = 0.0f;
	private float remainingRoundTime = 0.0f;
	private int roundsLeft = 0;
	private int remainingTimeSec = 0;

	public GameObject[] pieces;
	public GameObject pieceMenu;

	public Slot[] slots;
	public int columns; 
	public int rows;
	public GameObject startPiecePrefab;
	public int startPosition;
	private int activeSlotPos;

	void Awake() {
		roundsLeft = roundsGiven;
		remainingRoundTime = roundTimeGiven;
		for (int i = 0; i < pieces.Length; i++) {
			GameObject pipe = (GameObject)GameObject.Instantiate (pieces [i]);
			pipe.transform.SetParent(pieceMenu.transform, false);
		}
		// subscribe to the event that alerts the game manager of pieces added to the rocket
		Slot.OnPieceAdded += PieceAdded;

		columns = 7;
		rows = 7;
		for (int i = 0; i < slots.Length; i++) {
			slots[i].setActive(false);
		}
		GameObject startPiece = (GameObject)GameObject.Instantiate (startPiecePrefab);
		startPiece.transform.SetParent (slots [startPosition].transform, false);
		activeSlotPos = startPosition++;
		slots [activeSlotPos].setActive (true);
	}

	// Use this for initialization
	void Start () {
		SetCountdownTimerText(roundTimeGiven);
	}

	// Update is called once per frame
	void Update () {
		remainingRoundTime = roundTimeGiven - timeElapsed;
		if (roundsLeft != 0) {
			if (remainingRoundTime > 0.0) { //There still is time in the round
				if (GetSeconds (remainingRoundTime) != remainingTimeSec) {
					// get remaining seconds
					remainingTimeSec = GetSeconds (remainingRoundTime);
					// set the timer text
					SetCountdownTimerText (remainingTimeSec);
				}
			} else { // The end of the round
				roundsLeft--;
				remainingTimeSec = roundTimeGiven;
				SetCountdownTimerText (remainingTimeSec);
				timeElapsed = 0.0f;
			}
		} else { // End the game
			EndGame();
		}
		// increment the time elapsed
		timeElapsed += Time.deltaTime;
	}

	void PieceAdded(GameObject pieceAdded) {
		if (slots [activeSlotPos].getOutflow () == "top")
			slots [activeSlotPos - columns].setActive(true);
		else if (slots [activeSlotPos].getOutflow () == "right")
			slots [activeSlotPos + 1].setActive(true);
		else if (slots [activeSlotPos].getOutflow () == "bottom")
			slots [activeSlotPos + columns].setActive(true);
		else if (slots [activeSlotPos].getOutflow () == "left")
			slots [activeSlotPos - 1].setActive(true);
		slots [activeSlotPos].setActive(false);
	}

	void EndGame() {
		
	}

	void SetCountdownTimerText(int timerSec)
	{
		string timerText = FormatTime2 (timerSec);
		countdownTimer.text = timerText;
	}

	int GetSeconds(float value) {
		TimeSpan t = TimeSpan.FromSeconds (value);
		return (t.Minutes * 60 + t.Seconds);
	}

	string FormatTime(float value) {
		TimeSpan t = TimeSpan.FromSeconds (value);
		return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
	}

	string FormatTime2(int value) 
	{
		int min, sec;
		min = value / 60;
		sec = value % 60;
		return string.Format("{0:D2}:{1:D2}", min, sec);
	}
}
	//		if (pieceAdded.tag == "Horizontal") {
	//
	//		}
	//		else if (pieceAdded.tag == "LowerL") {
	//			
	//		}
	//		else if (pieceAdded.tag == "LowerR") {
	//		}
	//		else if (pieceAdded.tag == "UpperL") {
	//		}
	//		else if (pieceAdded.tag == "UpperR") {
	//		}
	//		else if (pieceAdded.tag == "Vertical") {
	//		}

