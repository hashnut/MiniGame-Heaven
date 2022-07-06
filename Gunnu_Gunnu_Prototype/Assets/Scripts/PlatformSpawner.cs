using UnityEngine;
using UnityEngine.Events;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public PlayerController playerController;

    public BoxCollider2D CenterPoint;

    private int totalCount = 30; // 생성할 발판의 개수

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -50); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막 배치 시점

    private float stride; // 플랫폼 사이의 간격 (단위로 활용)
    private int minLeap = 1; // 최소 간격 배수
    private int maxLeap = 2; // 최대 간격 배수
    private float randomLeap; // 플랫폼 배치시 랜덤 부여 간격


    private float lastPlatformX;  // 마지막 플랫폼의 X 위치
    private float platformY = -1.5f; // 배치할 플랫폼의 Y 위치


    private float screenWidth; // 스크린 넓이

    public PlatformTriggerEvent platformTriggerEvent;

    private void Awake()
    {
        if (playerController == null)
        {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        }
        stride = playerController.GetLeapDist();

        lastPlatformX = playerController.GetInitialX();

        if (CenterPoint == null)
        {
            CenterPoint = GameObject.Find("Center Point").GetComponent<BoxCollider2D>();
        }

        screenWidth = Screen.width;

        Debug.Log("screenWidth : " + screenWidth);
    }

    void Start() 
    {
        platforms = new GameObject[totalCount];
        for (int i = 0; i < totalCount; ++i)
        {
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);

            // Add Trigger Event
            platforms[i].AddComponent<PlatformTriggerEvent>();
            platforms[i].GetComponent<PlatformTriggerEvent>().onTriggerEnter =  new UnityEvent<Collider2D>();
            platforms[i].GetComponent<PlatformTriggerEvent>().onTriggerEnter.AddListener(OnTheTriggerEnterMethod);
        }

        // 초기 플랫폼 생성
        platforms[currentIndex++].transform.position = new Vector2(lastPlatformX, platformY);

        for (currentIndex = 1; currentIndex < totalCount; ++currentIndex)
        {
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            randomLeap = Random.Range(minLeap, maxLeap+1) * stride;

            platforms[currentIndex].transform.position = new Vector2(lastPlatformX + randomLeap, platformY);

            lastPlatformX += randomLeap;
        }

        currentIndex = 0;

        platformTriggerEvent = GameObject.Find("Platform_Safe").GetComponent<PlatformTriggerEvent>();

        // OnEnable();
    }

    void Update() 
    {
        if (GameManager.instance.isGameover)
        {
            return;
        }

    }

    void LocatePlatform()
    {
        platforms[currentIndex].SetActive(false);
        platforms[currentIndex].SetActive(true);

        randomLeap = Random.Range(minLeap, maxLeap+1) * stride;

        platforms[currentIndex].transform.position = new Vector2(lastPlatformX + randomLeap, platformY);

        lastPlatformX += randomLeap;

        currentIndex += 1;
        if (currentIndex == totalCount)
        {
            currentIndex = 0;
        }
    }
    void OnTheTriggerEnterMethod(Collider2D other)
    {
        if (other.gameObject.tag == "Dead")
        {
            // Debug.Log("Platform touches DeadZone Left!");
            LocatePlatform();
        }
    }

}