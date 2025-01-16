using System;
using System.Collections;
using System.Collections.Concurrent; // For thread-safe queue
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Right : MonoBehaviour
{
    public string esp32WebSocketUrl = "ws://192.168.0.97:81"; // Replace with your ESP32 IP and WebSocket port
    private WebSocket webSocket;

    private float right_index_degree = 0;
    private float right_middle_degree = 0;
    private float right_ring_degree = 0;
    private float right_thumb_degree = 0;
    private float right_pinky_degree = 0;

    private GameObject[] right_index;
    private GameObject[] right_pinky;
    private GameObject[] right_ring;
    private GameObject[] right_middle;
    private GameObject[] right_thumb;
    private GameObject right_hand;

    private float[] right_hand_trans = new float[] { 0.0f, 0.0f, 0.0f };
    private float[] right_hand_rot = new float[] { 0.0f, 0.0f, 0.0f };

    private float val;
    public GameObject cube;
    public List<GameObject> listofCubes = new List<GameObject>();

    private bool isConnected = false;

    // Thread-safe queue for main thread actions
    private ConcurrentQueue<Action> mainThreadActions = new ConcurrentQueue<Action>();

    private float ClampRotation(float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }

    void Start()
    {
        InitializeHandParts();
        InitializeWebSocket();
    }

    void Update()
    {
        // Process all actions queued for the main thread
        while (mainThreadActions.TryDequeue(out var action))
        {
            action?.Invoke();
        }

        if (isConnected)
        {
            ProcessCubes();
        }
    }

    // Initialize hand parts
    private void InitializeHandParts()
    {
        right_index = GameObject.FindGameObjectsWithTag("right_index");
        right_pinky = GameObject.FindGameObjectsWithTag("right_pinky");
        right_ring = GameObject.FindGameObjectsWithTag("right_ring");
        right_middle = GameObject.FindGameObjectsWithTag("right_middle");
        right_thumb = GameObject.FindGameObjectsWithTag("right_thumb");
        right_hand = GameObject.FindWithTag("right_hand");
    }

    // Initialize WebSocket connection
    private void InitializeWebSocket()
    {
        webSocket = new WebSocket(esp32WebSocketUrl);

        webSocket.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connected.");
            isConnected = true;
        };

        webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log("Received message: " + e.Data);

            // Enqueue the message processing to the main thread
            mainThreadActions.Enqueue(() =>
            {
                try
                {
                    string[] serialOutputs = e.Data.Split(':');
                    if (serialOutputs.Length >= 3 && float.TryParse(serialOutputs[2], out val))
                    {
                        ProcessData(serialOutputs);
                    }
                    else
                    {
                        Debug.LogWarning("Invalid data format received.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error processing data: " + ex.Message);
                }
            });
        };

        webSocket.OnError += (sender, e) =>
        {
            Debug.LogError("WebSocket error: " + e.Message);
        };

        webSocket.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket closed.");
            isConnected = false;
        };

        Debug.Log("Connecting to WebSocket...");
        webSocket.Connect();
    }

    // Process parsed data
    private void ProcessData(string[] serialOutputs)
    {
        if (serialOutputs[0] == "Right")
        {
            switch (serialOutputs[1])
            {
                case "Index":
                    val = ClampRotation(val, -30f, 120f); // Lock rotation to 0° - 90°
                    RotateFinger(right_index, val - right_index_degree);
                    right_index_degree = val;
                    break;

                case "Middle":
                    val = ClampRotation(val, -30f, 120f);
                    RotateFinger(right_middle, val - right_middle_degree);
                    right_middle_degree = val;
                    break;

                case "Ring":
                    val = ClampRotation(val, -30f, 120f);
                    RotateFinger(right_ring, val - right_ring_degree);
                    right_ring_degree = val;
                    break;

                case "Pinky":
                    val = ClampRotation(val, -30f, 120f);
                    RotateFinger(right_pinky, val - right_pinky_degree);
                    right_pinky_degree = val;
                    break;

                case "Thumb":
                    val = ClampRotation(val, -30f, 120f);
                    RotateFinger(right_thumb, val - right_thumb_degree);
                    right_thumb_degree = val;
                    break;

                case "RotateX":
                    RotateHand(right_hand, val - right_hand_rot[0], 0, 0); // Adjust axis mapping
                    right_hand_rot[0] = val;
                    break;
                case "RotateY":
                    RotateHand(right_hand, 0, val - right_hand_rot[1], 0);
                    right_hand_rot[1] = val;
                    break;
                case "RotateZ":
                    RotateHand(right_hand, 0, 0, val - right_hand_rot[2]);
                    right_hand_rot[2] = val;
                    break;

                case "TranslateX":
                    // Translate along X-axis; scaling adjusted for ESP32 range
                    TranslateHand(right_hand, val / 3000.0f, 0, 0);
                    right_hand_trans[0] = val;
                    break;

                case "TranslateY":
                    // Translate along Y-axis
                    TranslateHand(right_hand, 0, (val - right_hand_trans[1]) / 3000.0f, 0);
                    right_hand_trans[1] = val;
                    break;

                case "TranslateZ":
                    // Translate along Z-axis
                    TranslateHand(right_hand, 0, 0, (val - right_hand_trans[2]) / 3000.0f);
                    right_hand_trans[2] = val;
                    break;

                default:
                    Debug.LogWarning($"Unknown data type received: {serialOutputs[1]}");
                    break;
            }
        }
    }

    // Finger rotation
    private void RotateFinger(GameObject[] bones, float rotationValue)
    {
        foreach (GameObject bone in bones)
        {
            bone.transform.Rotate(0, 0, rotationValue * -70 / 180);
        }
    }

    // Hand rotation
    private void RotateHand(GameObject hand, float xRotation, float yRotation, float zRotation)
    {
        hand.transform.Rotate(xRotation, yRotation, zRotation);
    }

    // Hand translation
    private void TranslateHand(GameObject hand, float xTranslation, float yTranslation, float zTranslation)
    {
        hand.transform.Translate(xTranslation, yTranslation, zTranslation);
    }

    // Handle cube interactions
    private void ProcessCubes()
    {
        if (right_ring_degree > 40) CreateCube(-1, 2, 4);
        if (right_index_degree > 40) CreateCube(1, 2, 4);
        if (right_middle_degree > 40) CreateCube(0, 2, 4);

        if (right_middle_degree > 60 && right_index_degree > 60 && right_ring_degree > 60 && right_thumb_degree > 60)
        {
            DestroyCubes();
        }

        if (right_hand_rot[2] > 20)
        {
            ReverseGravity();
        }
    }

    // Create a cube
    private void CreateCube(int x, int y, int z)
    {
        GameObject newCube = Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
        listofCubes.Add(newCube);
    }

    // Destroy all cubes
    private void DestroyCubes()
    {
        foreach (GameObject cube in listofCubes)
        {
            Destroy(cube);
        }
        listofCubes.Clear();
    }

    // Reverse gravity
    private void ReverseGravity()
    {
        foreach (GameObject cube in listofCubes)
        {
            cube.GetComponent<Rigidbody>().AddForce(0, 30F, 0);
        }
    }

    void OnDestroy()
    {
        if (webSocket != null && webSocket.IsAlive)
        {
            webSocket.Close();
        }
    }
}
