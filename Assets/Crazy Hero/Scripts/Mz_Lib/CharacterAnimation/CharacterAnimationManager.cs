using UnityEngine;
using System.Collections;
using System;

public class CharacterAnimationManager : MonoBehaviour {

	public tk2dSpriteAnimator animatedSprite;
	public SkeletonAnimation skeletonAnim;
	public Animator animator;
    public GameObject animation_data_obj;
	internal Vector3 original_animationScale;

	public enum List_animations {
		Idle = 0,
		Walk,
		Dead,
		Attack,
        CastSkill,
        CastSkill1,
        CastSkill2,
    };

    #region <@-- Event Handle.

    public event EventHandler animationComplete_Event;
    private void OnAnimationComplete(EventArgs e) {
        if (animationComplete_Event != null)
            animationComplete_Event(this, e);
    }

    #endregion


    // Use this for initialization
    void Awake() {
        if (animation_data_obj) 
		{
            original_animationScale = animation_data_obj.transform.localScale;
            skeletonAnim = animation_data_obj.GetComponent<SkeletonAnimation>();
            
			if(skeletonAnim != null)
				return;
        }

		//<@-- Find animatorSprite;
		animatedSprite = this.GetComponent<tk2dSpriteAnimator>();
		if(animatedSprite != null)
			return;
        else if(animatedSprite == null)  {
            Transform anim = this.transform.Find("animation_data");
			if(anim != null) {
				animatedSprite = anim.GetComponent<tk2dSpriteAnimator>();

				if(animatedSprite != null)
					return;
			}
        }

		//<@-- Find animator..
		animator = this.GetComponent<Animator>();
        if (animator == null)
        {
            Transform anim = this.transform.Find("Animation_data");
            if (anim != null)
                animator = anim.GetComponent<Animator>();
            else
                Debug.LogError("No animation info..");
		}
	}
	
	void Start() {
		if(skeletonAnim != null) {
            skeletonAnim.state.AddAnimation(0, List_animations.Idle.ToString(), true, 0.1f);
		}
	}

	public void PlayAnimationByName(List_animations nameAnimation) {
        tk2dSpriteAnimationClip clip = animatedSprite.GetClipByName(nameAnimation.ToString());
        if(clip != null)
            animatedSprite.Play(clip);
	}

	public void PlayAnimationByName(List_animations nameAnimation, bool loop, float speed = 1f) {
        Spine.Animation _animation = skeletonAnim.state.Data.SkeletonData.FindAnimation(nameAnimation.ToString());
        if (_animation == null) {
            Debug.LogWarning(this.name + ": No have animation name = " + nameAnimation.ToString());
            return;
        }
        skeletonAnim.state.SetAnimation(0, nameAnimation.ToString(), loop);
		skeletonAnim.timeScale = speed;
	}

	public void StopAnimation() {
        if (animatedSprite != null) {
            animatedSprite.Paused = true;
        }
        else
            skeletonAnim.timeScale = 0;
	}

	public void StartAnimation() {
        if(animatedSprite != null) {
            animatedSprite.Paused = false;
        }
        else
            skeletonAnim.timeScale = 1;
	}
	
	void Update() {
		if(skeletonAnim != null && skeletonAnim.state != null) {
			if(skeletonAnim.loop == false) {
				skeletonAnim.state.Complete += (state, trackIndex, loopCount) => {
					this.OnAnimationComplete(EventArgs.Empty);
				};
			}
		}
	}
}
