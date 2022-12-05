using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Animator> wordAnimators;

    public void wordNextLine(int index){
        wordAnimators[index].SetTrigger("NextLineTrigger");
    }
}
