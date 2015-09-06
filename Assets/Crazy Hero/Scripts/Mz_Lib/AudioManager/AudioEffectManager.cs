using UnityEngine;
using System.Collections;

public class AudioEffectManager : MonoBehaviour {
	
    public AudioSource alternativeEffect_source;
	
    public AudioClip buttonDown_Clip;
	public AudioClip buttonUp_Clip;
	public AudioClip buttonHover_Clip;
	public AudioClip correct_Clip;
	public AudioClip wrong_Clip;
    public AudioClip pop_clip;
	
	
	void Awake() {
		GameObject source = new GameObject("alternativeEffect", typeof(AudioSource));
		source.transform.parent = this.transform;
		
		if(alternativeEffect_source == null)
			alternativeEffect_source = source.GetComponent<AudioSource>();
	}
	
	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().volume = 1;
		alternativeEffect_source.volume = 1;
	}
	
	public void PlayOnecSound(AudioClip sound) {
        this.GetComponent<AudioSource>().Stop();
		this.GetComponent<AudioSource>().PlayOneShot(sound);
	}
	
	public void PlayOnecWithOutStop(AudioClip sound) {
        this.alternativeEffect_source.PlayOneShot(sound);
	}

	public void PlayWithoutStop(ref AudioClip clip) {
		this.alternativeEffect_source.clip = clip;
		this.alternativeEffect_source.Play();
	}
}
