using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LockPickYay : MonoBehaviour
{
   
    public Camera cam;
    public Transform innerLock;
    public Transform pickPosition;

    public float maxAngle = 90;
    public float lockSpeed = 10;
    float currentTime = 0f;
    float startingtime = 0f;
    public GameObject MenuUI;
    private Scene scene;

    [Range(1, 25)]
    public float lockRange = 10;

    private float EuAngle;
    private float unlockAngle;
    private Vector2 LockRange;
    bool gamestart;
    [SerializeField] Text countdownText;

    private float PressTime = 0;
    private float lockdifficulty = 0;
   

    private bool PickRotation = true;

    // Start is called before the first frame update
    void Start()
    {
        NewLock();
        lockdifficulty = 0;
        gamestart = false;
        scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = pickPosition.position;

        if (PickRotation)
        {
            Vector3 dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);

            EuAngle = Vector3.Angle(dir, Vector3.up);

            Vector3 cross = Vector3.Cross(Vector3.up, dir);
            if (cross.z < 0) { EuAngle = -EuAngle; }

            EuAngle = Mathf.Clamp(EuAngle, -maxAngle, maxAngle);

            Quaternion rotateTo = Quaternion.AngleAxis(EuAngle, Vector3.forward);
            transform.rotation = rotateTo;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            PickRotation = false;
            PressTime = 1;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            PickRotation = true;
            PressTime = 0;
        }
        if (gamestart == true)
        {
            currentTime -= 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("");
        }
        if (currentTime < 0)
        {
            SceneManager.LoadScene(0);
        }

        float percentage = Mathf.Round(100 - Mathf.Abs(((EuAngle - unlockAngle) / 100) * 100));
        float lockRotation = ((percentage / 100) * maxAngle) * PressTime;
        float maxRotation = (percentage / 100) * maxAngle;

        float @lock = Mathf.Lerp(innerLock.eulerAngles.z, lockRotation, Time.deltaTime * lockSpeed);
        innerLock.eulerAngles = new Vector3(0, 0, @lock);

        if (@lock >= maxRotation - 1)
        {
            if (EuAngle < LockRange.y && EuAngle > LockRange.x)
            {
                Debug.Log("Unlocked!");
                
                lockdifficulty = 0;
                NewLock();

                PickRotation = true;
                PressTime = 0;
                SceneManager.LoadScene(0);
            }
            else
            {
                float RandRot = Random.insideUnitCircle.x;
                transform.eulerAngles += new Vector3(0, 0, Random.Range(-RandRot, RandRot));
            }
        }
    }
    public void SetDifficultyeasy()
    {
        lockdifficulty = 10;
        MenuUI.SetActive(false);
        startingtime = 30;
        currentTime = startingtime;
        gamestart = true;
        

    }
    public void SetDifficultymedium()
    {
        lockdifficulty = 50;
        MenuUI.SetActive(false);
        startingtime = 20;
        currentTime = startingtime;
        gamestart = true;
    }
   public void SetDifficultyhard()
    {
       lockdifficulty = 80;
        MenuUI.SetActive(false);
        startingtime = 10;
        currentTime = startingtime;
        gamestart = true;
    }

    void NewLock()
    {
        unlockAngle = Random.Range(-maxAngle + lockRange + lockdifficulty, maxAngle - lockRange - lockdifficulty);
        LockRange = new Vector2(unlockAngle - lockRange - lockdifficulty, unlockAngle + lockRange + lockdifficulty); ;
    }
}
