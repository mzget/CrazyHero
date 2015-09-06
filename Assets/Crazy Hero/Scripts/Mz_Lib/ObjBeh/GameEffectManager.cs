using UnityEngine;
using System.Collections;

public class GameEffectManager : MonoBehaviour {
	
//	public const string BLOOMSTAR_EFFECT_PATH = "GameEffects/BloomStar";
//	public const string IRIDESCENT_EFFECT_PATH = "GameEffects/Iridescent";
//	public const string STARINCIDENT_EFFECT_PATH = "GameEffects/StarIncident";
	

	private static GameEffectManager instance;
	public static GameEffectManager Instance { 
		get {
			if(instance == null) {
				var gamecontroller = GameObject.FindGameObjectWithTag("GameController");
				instance = gamecontroller.GetComponent<GameEffectManager>();
			}

			return instance;
		}
	}
	
	public void Create2DSpriteAnimationEffect(string targetName, Transform transform) {
        GameObject effect = Instantiate(Resources.Load(targetName, typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
        effect.transform.parent = transform;
        effect.transform.localScale = Vector3.one;
        effect.transform.position += Vector3.back;
		

        tk2dAnimatedSprite animatedSprite = effect.GetComponent<tk2dAnimatedSprite>();
        animatedSprite.animationCompleteDelegate = delegate(tk2dAnimatedSprite anim, int id) {
            Destroy(effect);
            animatedSprite = null;
        };
	}

	public GameObject CreateParticleEffect(string effectName, Vector3 spawnPoint) {		
		GameObject effect = Instantiate(Resources.Load("ParticleFX/" + effectName, typeof(GameObject)), spawnPoint, Quaternion.identity) as GameObject;

        return effect;
	}

	public GameObject CreateBulletEffect(string effectName, Vector3 spawnPoint) {		
		GameObject effect = Instantiate(Resources.Load("ParticleFX/" + effectName, typeof(GameObject)), spawnPoint, Quaternion.identity) as GameObject;

		return effect;
	}
 }
