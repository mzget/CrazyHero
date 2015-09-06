using UnityEngine;
using System.Collections;

public class AudioDescribeManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void PlayOnecSound(AudioClip sound)
    {
        this.GetComponent<AudioSource>().Stop();
        this.GetComponent<AudioSource>().PlayOneShot(sound);
    }

    public void PlayOnecWithOutStop(AudioClip sound)
    {
        this.GetComponent<AudioSource>().PlayOneShot(sound);
    }
}
