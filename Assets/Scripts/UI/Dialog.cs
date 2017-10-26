using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dialog
{

	public List<DialogLine> lines;
	public int currentLine = 0;
	public bool isFinished = false;

	public Dialog (List<DialogLine> dl)
	{
		this.lines = dl;
	}

	public DialogLine ReadLine ()
	{
		int ret = 0;
		if (currentLine < lines.Count) {
			ret = currentLine;
			if (currentLine == 0 && isFinished == true)
				isFinished = false;
			if (currentLine == lines.Count - 1) {
				isFinished = true;
				currentLine = 0;
			} else
				currentLine++;
			return lines [ret];
		} else {
			currentLine = 0;
			return lines [0];			
		}
	}

	// Manage the dialogs that have to be loaded for each level
	public static Dictionary<DialogEnum,Dialog> InstantiateDialogs (LevelEnum levelEnum)
	{
		Dictionary<DialogEnum,Dialog> dialogDico = new Dictionary<DialogEnum,Dialog> ();
		switch (levelEnum) {
		case LevelEnum.level_1_story:
			Sprite portraitLeo = Resources.Load ("Portraits/leo", typeof(Sprite)) as Sprite;
			Sprite portraitDogCatcher = Resources.Load ("Portraits/dogcatcher", typeof(Sprite)) as Sprite;
			List<DialogLine> dl = new List<DialogLine> () {
				new DialogLine ("TUTO_JUMP", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.tuto_jump, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_ATTACK", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.tuto_attack, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_KITTYZ", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.tuto_kittyz, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_ENNEMY_1", "LEO", portraitLeo),
				new DialogLine ("TUTO_ENNEMY_2", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.tuto_ennemy, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_CHECKPOINT", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.tuto_checkpoint, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("FIRST_HEDGEHOG_1", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.first_hedgehog_1, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("FIRST_HEDGEHOG_2", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.first_hedgehog_2, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("FIRST_SQUIRREL", "LEO", portraitLeo)
			};
			dialogDico.Add (DialogEnum.first_squirrel, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("DOG_CATCHER_START", "DOG_CATCHER", portraitDogCatcher)
			};
			dialogDico.Add (DialogEnum.dog_catcher_start, new Dialog (dl));
			break;
		default:
			break;
		}
		return dialogDico;
	}

}

public class DialogLine
{
	public Sprite portrait;
	public string nameStringId, textStringId;

	public DialogLine (string textStringId, string nameStringId, Sprite portrait)
	{
		this.textStringId = textStringId;
		this.nameStringId = nameStringId;
		this.portrait = portrait;
	}
}

public enum DialogEnum
{
	tuto_jump,
	tuto_attack,
	tuto_kittyz,
	tuto_ennemy,
	tuto_checkpoint,
	in_progress,
	first_hedgehog_1,
	first_hedgehog_2,
	first_squirrel,
	dog_catcher_start
}
	