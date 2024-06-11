using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // ��� ���ǰ� ��縦 ���� ť
    Queue<AudioClip> backgroundMusicQueue = new Queue<AudioClip>();
    Queue<AudioClip> dialogueQueue = new Queue<AudioClip>();

    // ���� ��� ���� ��� ���ǰ� ����� AudioSource
    AudioSource backgroundMusicSource;
    AudioSource dialogueSource;

    // ��� ������ ���� AudioClip �迭
    public AudioClip[] backgroundMusicClips;
    // ��縦 ���� AudioClip �迭
    public AudioClip[] dialogueClips;

    void Start()
    {
        // AudioSource ������Ʈ�� �����ͼ� �ʱ�ȭ
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        dialogueSource = gameObject.AddComponent<AudioSource>();

        // ��� ������ ť�� �߰�
        foreach (var clip in backgroundMusicClips)
        {
            backgroundMusicQueue.Enqueue(clip);
        }

        // ��縦 ť�� �߰�
        foreach (var clip in dialogueClips)
        {
            dialogueQueue.Enqueue(clip);
        }

        // ù ��° ��� ���� ���
        PlayNextBackgroundMusic();
    }

    void Update()
    {
        // ��� ����� ������ ���� ��� ���
        if (!dialogueSource.isPlaying && dialogueQueue.Count > 0)
        {
            PlayNextDialogue();
        }
    }

    // ���� ��� ���� ���
    void PlayNextBackgroundMusic()
    {
        if (backgroundMusicQueue.Count > 0)
        {
            var nextClip = backgroundMusicQueue.Dequeue();
            backgroundMusicSource.clip = nextClip;
            backgroundMusicSource.loop = true; // ���� ���
            backgroundMusicSource.Play();
        }
    }

    // ���� ��� ���
    void PlayNextDialogue()
    {
        if (dialogueQueue.Count > 0)
        {
            var nextClip = dialogueQueue.Dequeue();
            dialogueSource.clip = nextClip;
            dialogueSource.Play();
        }
    }

    // ���� ����� �� ȣ��Ǵ� �Լ�
    void OnDestroy()
    {
        // AudioSource �ı�
        Destroy(backgroundMusicSource);
        Destroy(dialogueSource);
    }
}
