using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialougeManager : MonoBehaviour
{
    private Queue<string> sentences=new Queue<string>();
    public TMP_Text NPCname;
    public TMP_Text dialougeForNpc;
    public bool alreadyInDialouge=false;
    private  Dialouge currentDialouge;
  
   
    
    // Start is called before the first frame update
    void Start()
    {
       sentences.Clear();
       NPCname.text="";
       dialougeForNpc.text="";
    }

    public void StartDialouge(Dialouge dialouge)
    {
       
        if(currentDialouge==dialouge) return;  //if same Dialouge is already playing then go back

        currentDialouge=dialouge;
        //alreadyInDialouge=true;
       StopCoroutine(FadeOut());
        NPCname.text=currentDialouge.name;
        sentences.Clear();

        foreach (string sentence in currentDialouge.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void  DisplayNextSentence()
    {
        if(sentences.Count==0)
        {
            EndDialouge();
            return;
        }

        string sentence=sentences.Dequeue();
        StopAllCoroutines();
       StartCoroutine(TypeSentences(sentence));

    }

    IEnumerator TypeSentences(string sentence)
    {
        dialougeForNpc.text="";
        foreach(char letter in sentence.ToCharArray())
        {
                dialougeForNpc.text+=letter;
                yield return null;
        }
    }

    public void EndDialouge()
    {
        sentences.Clear();
        StartCoroutine(FadeOut());
        currentDialouge=null;
    }

    IEnumerator FadeOut()
    {
        Color c = NPCname.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            NPCname.color = c;
            dialougeForNpc.color=c;
            yield return new WaitForSeconds(.1f);
        }
        //clearing the sentence from screen
        dialougeForNpc.text="";
        NPCname.text="";
        //resting the color back
        c.a=1f;
        NPCname.color = c;
        dialougeForNpc.color=c;

    }
    
  
}


[System.Serializable]
public class Dialouge
{
    public string name;
    [TextArea(3,10)]
    public List<string> sentences;
}

