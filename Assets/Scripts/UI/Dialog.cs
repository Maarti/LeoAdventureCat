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
		Sprite portraitLeo, portraitDogCatcher, portraitRat;
		AudioClip catSound, dogCatcherSound, ratSound;
		List<DialogLine> dl;
		Dictionary<DialogEnum,Dialog> dialogDico = new Dictionary<DialogEnum,Dialog> ();

		switch (levelEnum) {

		/* LEVEL 01-STORY */
		case LevelEnum.level_1_story:
			portraitLeo = Resources.Load ("Portraits/leo", typeof(Sprite)) as Sprite;
			portraitDogCatcher = Resources.Load ("Portraits/dogcatcher", typeof(Sprite)) as Sprite;
			catSound = Resources.Load ("Sound/cat_jump", typeof(AudioClip)) as AudioClip;
			dogCatcherSound = Resources.Load ("Sound/dog_catcher_hit", typeof(AudioClip)) as AudioClip;

			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_JUMP", "LEO", portraitLeo, catSound)
			};
			dialogDico.Add (DialogEnum.tuto_jump, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_ATTACK", "LEO", portraitLeo, catSound)
			};
			dialogDico.Add (DialogEnum.tuto_attack, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_KITTYZ", "LEO", portraitLeo, catSound)
			};
			dialogDico.Add (DialogEnum.tuto_kittyz, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_ENNEMY_1", "LEO", portraitLeo, catSound)
				//,new DialogLine ("TUTO_ENNEMY_2", "LEO", portraitLeo, catSound)
			};
			dialogDico.Add (DialogEnum.tuto_ennemy, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("TUTO_CHECKPOINT", "LEO", portraitLeo, catSound)
			};
			dialogDico.Add (DialogEnum.tuto_checkpoint, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("FIRST_HEDGEHOG_1", "LEO", portraitLeo, catSound)
			};
			dialogDico.Add (DialogEnum.first_hedgehog_1, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("FIRST_HEDGEHOG_2", "LEO", portraitLeo, catSound)
			};
			dialogDico.Add (DialogEnum.first_hedgehog_2, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("FIRST_SQUIRREL", "LEO", portraitLeo, catSound)
			};
			dialogDico.Add (DialogEnum.first_squirrel, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("DOG_CATCHER_START", "DOG_CATCHER", portraitDogCatcher, dogCatcherSound)
			};
			dialogDico.Add (DialogEnum.dog_catcher_start, new Dialog (dl));
			break;

		/* LEVEL 02-STORY */
		case LevelEnum.level_2_story:
			portraitLeo = Resources.Load ("Portraits/leo", typeof(Sprite)) as Sprite;
			portraitRat = Resources.Load ("Portraits/croc", typeof(Sprite)) as Sprite;
			catSound = Resources.Load ("Sound/cat_jump", typeof(AudioClip)) as AudioClip;
			ratSound = Resources.Load ("Sound/rat_squeak", typeof(AudioClip)) as AudioClip;

			dl = new List<DialogLine> () {
				new DialogLine ("RAT_ASKS_HELP", "CROC", portraitRat, ratSound)
			};
			dialogDico.Add (DialogEnum.rat_asks_help, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("RAT_THANKS_CAT", "CROC", portraitRat, ratSound)
			};
			dialogDico.Add (DialogEnum.rat_thanks_cat, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("RAT_ASKS_FOLLOW", "CROC", portraitRat, ratSound)
			};
			dialogDico.Add (DialogEnum.rat_asks_follow, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("RAT_ASKS_WAIT", "CROC", portraitRat, ratSound)
			};
			dialogDico.Add (DialogEnum.rat_asks_wait, new Dialog (dl));
			dl = new List<DialogLine> () {
				new DialogLine ("RAT_READY", "CROC", portraitRat, ratSound)
			};
			dialogDico.Add (DialogEnum.rat_ready, new Dialog (dl));
            dl = new List<DialogLine>() {
                new DialogLine ("RAT_FINISHED_CHEESE", "CROC", portraitRat, ratSound)
            };
            dialogDico.Add(DialogEnum.rat_finished_cheese, new Dialog(dl));
            dl = new List<DialogLine>() {
                new DialogLine ("RAT_GO_TO_VENTILATION", "CROC", portraitRat, ratSound)
            };
            dialogDico.Add(DialogEnum.rat_go_to_ventilation, new Dialog(dl));
            dl = new List<DialogLine>() {
                new DialogLine ("RAT_START_VENTILATION_GAME", "CROC", portraitRat, ratSound)
            };
            dialogDico.Add(DialogEnum.rat_start_ventilation_game, new Dialog(dl));
            dl = new List<DialogLine>() {
                new DialogLine ("RAT_START_BULLDOG", "CROC", portraitRat, ratSound)
            };
            dialogDico.Add(DialogEnum.rat_start_bulldog, new Dialog(dl));
            break;

        case LevelEnum.level_2_04:
			portraitRat = Resources.Load ("Portraits/croc", typeof(Sprite)) as Sprite;
			ratSound = Resources.Load ("Sound/rat_squeak", typeof(AudioClip)) as AudioClip;
            dl = new List<DialogLine>() {
                new DialogLine ("RAT_HIDING_PLACE", "CROC", portraitRat, ratSound)
            };
            dialogDico.Add(DialogEnum.rat_hiding_place, new Dialog(dl));
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
	public AudioClip audio = null;
	public string nameStringId, textStringId;

	public DialogLine (string textStringId, string nameStringId, Sprite portrait, AudioClip audio = null)
	{
		this.textStringId = textStringId;
		this.nameStringId = nameStringId;
		this.portrait = portrait;
		if (audio != null)
			this.audio = audio;
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
	dog_catcher_start,
	rat_asks_help,
	rat_thanks_cat,
	rat_asks_follow,
	rat_asks_wait,
	rat_ready,
    rat_finished_cheese,
    rat_go_to_ventilation,
    rat_start_ventilation_game,
    rat_start_bulldog,
    rat_hiding_place
}
	