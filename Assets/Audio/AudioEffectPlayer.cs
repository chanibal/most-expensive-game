using UnityEngine;
using ChanibaL;

[RequireComponent(typeof(AudioSource))]
public class AudioEffectPlayer : MonoBehaviour {

	public AudioClip[] hurt;
	public AudioClip[] record;
	public AudioClip[] survived;
	public AudioClip[] taunt;
	public AudioClip[] thrown;

	RandomGenerator rng=new RandomGenerator();

	void PlayRandom(AudioClip[] clips) {
		var src = GetComponent<AudioSource> ();
		src.PlayOneShot (clips.GetRandomElement (rng));
	}

	[ContextMenu("play hurt")]
	public void PlayHurt() { PlayRandom (hurt); }
	public void PlayRecord() { PlayRandom (record); }
	public void PlaySurvived() { PlayRandom (survived); }
	public void PlayTaunt() { PlayRandom (taunt); }
	public void PlayThrown() { PlayRandom (thrown); }

}
