using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 배경 음악과 대사를 담을 큐
    Queue<AudioClip> backgroundMusicQueue = new Queue<AudioClip>();
    Queue<AudioClip> dialogueQueue = new Queue<AudioClip>();

    // 현재 재생 중인 배경 음악과 대사의 AudioSource
    AudioSource backgroundMusicSource;
    AudioSource dialogueSource;

    // 배경 음악을 담을 AudioClip 배열
    public AudioClip[] backgroundMusicClips;
    // 대사를 담을 AudioClip 배열
    public AudioClip[] dialogueClips;

    void Start()
    {
        // AudioSource 컴포넌트를 가져와서 초기화
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        dialogueSource = gameObject.AddComponent<AudioSource>();

        // 배경 음악을 큐에 추가
        foreach (var clip in backgroundMusicClips)
        {
            backgroundMusicQueue.Enqueue(clip);
        }

        // 대사를 큐에 추가
        foreach (var clip in dialogueClips)
        {
            dialogueQueue.Enqueue(clip);
        }

        // 첫 번째 배경 음악 재생
        PlayNextBackgroundMusic();
    }

    void Update()
    {
        // 대사 재생이 끝나면 다음 대사 재생
        if (!dialogueSource.isPlaying && dialogueQueue.Count > 0)
        {
            PlayNextDialogue();
        }
    }

    // 다음 배경 음악 재생
    void PlayNextBackgroundMusic()
    {
        if (backgroundMusicQueue.Count > 0)
        {
            var nextClip = backgroundMusicQueue.Dequeue();
            backgroundMusicSource.clip = nextClip;
            backgroundMusicSource.loop = true; // 루프 재생
            backgroundMusicSource.Play();
        }
    }

    // 다음 대사 재생
    void PlayNextDialogue()
    {
        if (dialogueQueue.Count > 0)
        {
            var nextClip = dialogueQueue.Dequeue();
            dialogueSource.clip = nextClip;
            dialogueSource.Play();
        }
    }

    // 씬이 변경될 때 호출되는 함수
    void OnDestroy()
    {
        // AudioSource 파괴
        Destroy(backgroundMusicSource);
        Destroy(dialogueSource);
    }
}
