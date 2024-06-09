using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager�� ����ϱ� ���� �߰�

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume = 1.0f;
    private AudioSource bgmPlayer;

    [Header("#SFX")]
    public List<AudioClip> sfxClips; // ����� SFX ����� Ŭ�� ����Ʈ
    public float sfxVolume = 1.0f;
    private AudioSource sfxPlayer;
    private Queue<AudioClip> sfxQueue;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // BGM AudioSource ����
            bgmPlayer = gameObject.AddComponent<AudioSource>();
            bgmPlayer.clip = bgmClip;
            bgmPlayer.volume = bgmVolume;
            bgmPlayer.loop = true;

            // SFX AudioSource ����
            sfxPlayer = gameObject.AddComponent<AudioSource>();
            sfxPlayer.volume = sfxVolume;

            // SFX ť ����
            sfxQueue = new Queue<AudioClip>(sfxClips);

            // �� ���� �̺�Ʈ ���
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // BGM ���
        if (bgmPlayer.clip != null)
        {
            bgmPlayer.Play();
            Debug.Log("BGM ��� ����");
        }
        else
        {
            Debug.LogError("BGM Ŭ���� �������� �ʾҽ��ϴ�.");
        }
    }

    void Update()
    {
        // SFX ����� ������ �� ���� Ŭ�� ���
        if (!sfxPlayer.isPlaying && sfxQueue.Count > 0)
        {
            PlayNextSfx();
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            if (bgmPlayer.clip != null)
            {
                bgmPlayer.Play();
                Debug.Log("BGM ��� ����");
            }
            else
            {
                Debug.LogError("BGM Ŭ���� �������� �ʾҽ��ϴ�.");
            }
        }
        else
        {
            bgmPlayer.Stop();
            Debug.Log("BGM ��� ����");
        }
    }

    public void PlaySfx()
    {
        // SFX ť�� ��� ���� ���� ��� ���
        if (sfxQueue.Count > 0)
        {
            AudioClip nextClip = sfxQueue.Dequeue();
            sfxPlayer.clip = nextClip;
            sfxPlayer.Play();
        }
    }

    private void PlayNextSfx()
    {
        if (sfxQueue.Count > 0)
        {
            AudioClip nextClip = sfxQueue.Dequeue();
            sfxPlayer.clip = nextClip;
            sfxPlayer.Play();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� ����Ǹ� BGM�� SFX ��� ����
        if (bgmPlayer.isPlaying)
        {
            bgmPlayer.Stop();
            Debug.Log("�� �������� BGM ��� ����");
        }

        if (sfxPlayer.isPlaying)
        {
            sfxPlayer.Stop();
            Debug.Log("�� �������� SFX ��� ����");
        }
    }

    void OnDestroy()
    {
        // �� ���� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
