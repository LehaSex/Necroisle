using UnityEngine;

public enum CameraMovementMode
{
    PingPong,
    Loop,
    Random
}

public enum InterpolationMethod
{
    Linear,
    Bezier,
    SmoothStep,
    EaseIn,
    EaseOut
}

public class CameraMovement : MonoBehaviour
{
    public float minX = -45f; // Минимальное значение X
    public float maxX = 45f; // Максимальное значение X
    public float minZ = -45f; // Минимальное значение Z
    public float maxZ = 45f; // Максимальное значение Z
    public float speed = 1f; // Скорость перемещения
    public float smoothness = 2f; // Плавность перемещения
    public CameraMovementMode movementMode = CameraMovementMode.PingPong; // Режим перемещения камеры
    [SerializeField] public InterpolationMethod interpolationMethod = InterpolationMethod.Linear; // Метод интерполяции

    public Vector3 targetPosition;
    public Vector3 controlPoint1;
    public Vector3 controlPoint2;
    public float startTime;
    

    void Start()
    {
        // Устанавливаем начальную позицию камеры
        targetPosition = transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        switch (movementMode)
        {
            case CameraMovementMode.PingPong:
                targetPosition = new Vector3(
                    Mathf.PingPong(Time.time * speed, maxX - minX) + minX,
                    transform.position.y,
                    Mathf.PingPong(Time.time * speed, maxZ - minZ) + minZ
                );
                break;
            case CameraMovementMode.Loop:
                targetPosition = new Vector3(
                    Mathf.PingPong(Time.time * speed, maxX - minX * 2f) + minX,
                    transform.position.y,
                    Mathf.PingPong(Time.time * speed, maxZ - minZ * 2f) + minZ
                );
                break;
            case CameraMovementMode.Random:
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    targetPosition = new Vector3(
                        Random.Range(minX, maxX),
                        transform.position.y,
                        Random.Range(minZ, maxZ)
                    );
                }
                break;
        }

        // Выбор метода интерполяции
        switch (interpolationMethod)
        {
            case InterpolationMethod.Linear:
                transform.position = LinearInterpolation(transform.position, targetPosition, smoothness * Time.deltaTime);
                break;
            case InterpolationMethod.Bezier:
                // Рассчитываем контрольные точки для кривой Безье
                controlPoint1 = transform.position + (targetPosition - transform.position) * 0.33f;
                controlPoint2 = transform.position + (targetPosition - transform.position) * 0.66f;
                // Плавно перемещаем камеру по кривой Безье
                transform.position = BezierCurve(transform.position, controlPoint1, controlPoint2, targetPosition, smoothness * Time.deltaTime);
                break;
            case InterpolationMethod.SmoothStep:
                transform.position = SmoothStepInterpolation(transform.position, targetPosition, smoothness * Time.deltaTime);
                break;
            case InterpolationMethod.EaseIn:
                transform.position = EaseInInterpolation(transform.position, targetPosition, smoothness * Time.deltaTime);
                break;
            case InterpolationMethod.EaseOut:
                transform.position = EaseOutInterpolation(transform.position, targetPosition, smoothness * Time.deltaTime);
                break;
        }
    }

    // Функция для линейной интерполяции
    public Vector3 LinearInterpolation(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, t);
    }

    // Функция для вычисления кривой Безье
    public Vector3 BezierCurve(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * start;
        p += 3 * uu * t * control1;
        p += 3 * u * tt * control2;
        p += ttt * end;

        return p;
    }

    // Функция для интерполяции методом SmoothStep
    public Vector3 SmoothStepInterpolation(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        t = t * t * (3f - 2f * t);
        return Vector3.Lerp(start, end, t);
    }

    // Функция для интерполяции методом EaseIn
    public Vector3 EaseInInterpolation(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
        return Vector3.Lerp(start, end, t);
    }

    // Функция для интерполяции методом EaseOut
    public Vector3 EaseOutInterpolation(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        return Vector3.Lerp(start, end, t);
    }
}
