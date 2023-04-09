using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
	public Transform target;

    public Tilemap theMap;

    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    private float halfHeight;
    private float halfWidth;

    public bool isLocked = false;

    public int musicToPlay;
    private bool musicStarted = false;

    public static CameraController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

		target = PlayerController.instance.transform;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0);
        topRightLimit = theMap.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0);

        PlayerController.instance.SetBounds(theMap.localBounds.min, theMap.localBounds.max);
    }

    // LateUpdate is called once per frame after Update
    void LateUpdate()
    {
        if (isLocked)
            return;

		transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // keep the camera inside the bounds
         transform.position = new Vector3(
             Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
             Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
             transform.position.z);
    }

    void Update()
    {
        if (!musicStarted)
        {
            musicStarted = true;
            PlayMusic();
        }
    }

    public void PlayMusic()
    {
        AudioManager.instance.PlayBGM(musicToPlay);
    }
}
