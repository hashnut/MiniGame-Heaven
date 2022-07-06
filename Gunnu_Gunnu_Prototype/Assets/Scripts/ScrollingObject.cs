using UnityEngine;

// 게임 오브젝트를 계속 왼쪽으로 움직이는 스크립트
public class ScrollingObject : MonoBehaviour 
{
    private float elapsed = 0.0f;

    private float amplitude = 1.5f;

    private float minAmplitude = 1.5f;
    private float maxAmplitude = 5.5f;
    
    private float minAmplitudeVar = 0.2f;
    private float maxAmplitudeVar = 0.5f;
    

    private void Start()
    {
        
    }

    private void Update() 
    {
        if (!GameManager.instance.isGameover)
        {
            elapsed += Time.deltaTime;
            float x = amplitude * Mathf.Sin(elapsed) - amplitude;
            transform.Translate(Vector3.left * x * Time.deltaTime);

            if (elapsed % 60.0f == 0.0f)
            {
                elapsed = Mathf.Clamp(elapsed + Random.Range(minAmplitudeVar, maxAmplitudeVar), minAmplitude, maxAmplitude);
            }
        }
    }
}