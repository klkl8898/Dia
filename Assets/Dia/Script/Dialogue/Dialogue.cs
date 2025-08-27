using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using DG.Tweening;
using System;
namespace Dia
{
  public class Dialogue : InstanceMono<Dialogue>
  {
    public TextAsset inkfile;
    private Story _inkStory;
    public KeyCode presskey = KeyCode.Z;
    public Text Comment, Character;
    public GameObject DialogueBox;
    [SerializeField] Canvas canvas;
    [SerializeField] private float TypingSpeed = 0.05f,SoundSpeed = 0.05f;
    [SerializeField] GameObject ChoiceBox;
    [SerializeField] private int _TextPadding = 10;
    [SerializeField] private Vector2 ChoicePos;
    [SerializeField] public bool IsPlaying;
    [SerializeField] public List<GameObject> Choices = new List<GameObject>();
    [SerializeField] private bool Waiting = false;
    [SerializeField] private AudioClip _sound;
    void Start()
    {
      _inkStory = new Story(inkfile.text);
      IsPlaying = false;
      Waiting = true;
      //测试：
      StartDialogue();
    }
    void Update()
    {
      if (IsPlaying)
      {
        if (Input.GetKeyDown(presskey))
        {
          ContinueDialogue();
        }
      }
    }
    private void StartDialogue()
    {
      IsPlaying = true;
      Waiting = false;
      ContinueDialogue();
    }
    private void ContinueDialogue()
    {
      if (!Waiting)
      {
        Refresh();
        if (_inkStory.canContinue)
        {
          Waiting = true;
          Comment.text = "";
          Character.text = "";
          string Current = _inkStory.Continue();
          if (_inkStory.currentTags.Count > 0)
          {
            Character.text = _inkStory.currentTags[0];
            if (Character.text == "none")
            {
              Character.text = "";
            }
           }
          if (_inkStory.currentTags.Count > 1)
          {
            _sound = Resources.Load<AudioClip>("Sounds/" + _inkStory.currentTags[1]);
            DialogueAudioManager.Instance.Typing(_sound, SoundSpeed, Current.Length*TypingSpeed);
          }
          Comment.DOText(Current, TypingSpeed * Current.Length).SetEase(Ease.Linear).OnComplete(() => Waiting = false);
        }
        else
        {
          if (_inkStory.currentChoices.Count > 0)
          {
            Waiting = true;
            for (int i = 0; i < _inkStory.currentChoices.Count; i++)
            {
              Choice choice = _inkStory.currentChoices[i];
              GameObject Choice = Instantiate(ChoiceBox);
              Choices.Add(Choice);
              Choice.transform.SetParent(canvas.transform);
              Choice.GetComponent<RectTransform>().localPosition = new Vector2(ChoicePos.x + _TextPadding * i, ChoicePos.y);
              Choice.transform.GetChild(0).GetComponent<Text>().text = choice.text;

            }
            StartCoroutine(PlayerChoice());
          }
          else
          {
            ExitDialogue();
          }
        }
      }
    }
    private IEnumerator PlayerChoice()
    {
      Waiting = true;
      int index = 1;
      TurnChoice(0, ref index);
      yield return null;
      while (!Input.GetKeyDown(presskey))
      {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
          TurnChoice(0, ref index);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
          TurnChoice(1, ref index);
        }
        yield return null;
      }
      string path = _inkStory.currentChoices[index].pathStringOnChoice;
      _inkStory.ChoosePathString(path);
      Waiting = false;
      ContinueDialogue();
    }
    void TurnChoice(int mode, ref int index)
    {
      if (mode == 1)
      {
        if (index > 0) index--;
        else if (index == 0) index = Choices.Count - 1;
        Choices[index].transform.GetChild(0).GetComponent<Text>().color = Color.white;
        foreach (GameObject c in Choices)
        {
          if(Choices.IndexOf(c)!=index)c.transform.GetChild(0).GetComponent<Text>().color = Color.gray;
        }
      }
      else
      {
        if (index < Choices.Count) index++;
        if (index == Choices.Count) index = 0;
        Choices[index].transform.GetChild(0).GetComponent<Text>().color = Color.white;
        foreach (GameObject c in Choices)
        {
          if(Choices.IndexOf(c)!=index)c.transform.GetChild(0).GetComponent<Text>().color = Color.gray;
        }
      }
    }
    private void ExitDialogue()
    {
      Refresh();
      IsPlaying = false;
      Waiting = false;
      DialogueBox.SetActive(false);
      _inkStory = null;
      inkfile = null;
    }
    private void Refresh()
    {
      Comment.text = "";
      Character.text = "";
      foreach (GameObject o in Choices)
      {
        Destroy(o);
      }
      Choices.Clear();
    }

  }

}
