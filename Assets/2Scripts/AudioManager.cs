using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 추가

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume = 1.0f;
    private AudioSource bgmPlayer;

    [Header("#SFX")]
    public List<AudioClip> sfxClips; // 재생할 SFX 오디오 클립 리스트
    public float sfxVolume = 1.0f;
    private AudioSource sfxPlayer;
    private Queue<AudioClip> sfxQueue;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // BGM AudioSource 설정
            bgmPlayer = gameObject.AddComponent<AudioSource>();
            bgmPlayer.clip = bgmClip;
            bgmPlayer.volume = bgmVolume;
            bgmPlayer.loop = true;

            // SFX AudioSource 설정
            sfxPlayer = gameObject.AddComponent<AudioSource>();
            sfxPlayer.volume = sfxVolume;

            // SFX 큐 설정
            sfxQueue = new Queue<AudioClip>(sfxClips);

            // 씬 변경 이벤트 등록
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // BGM 재생
        if (bgmPlayer.clip != null)
        {
            bgmPlayer.Play();
            Debug.Log("BGM 재생 시작");
        }
        else
        {
            Debug.LogError("BGM 클립이 설정되지 않았습니다.");
        }
    }

    void Update()
    {
        // SFX 재생이 끝났을 때 다음 클립 재생
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
                Debug.Log("BGM 재생 시작");
            }
            else
            {
                Debug.LogError("BGM 클립이 설정되지 않았습니다.");
            }
        }
        else
        {
            bgmPlayer.Stop();
            Debug.Log("BGM 재생 중지");
        }
    }

    public void PlaySfx()
    {
        // SFX 큐가 비어 있지 않은 경우 재생
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
        // 씬이 변경되면 BGM과 SFX 재생 중지
        if (bgmPlayer.isPlaying)
        {
            bgmPlayer.Stop();
            Debug.Log("씬 변경으로 BGM 재생 중지");
        }

        if (sfxPlayer.isPlaying)
        {
            sfxPlayer.Stop();
            Debug.Log("씬 변경으로 SFX 재생 중지");
        }
    }

    void OnDestroy()
    {
        // 씬 변경 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
