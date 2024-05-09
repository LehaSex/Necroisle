using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necroisle
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance = null;
        public static GameManager Instance { get { return instance; } }
        private float chunkSize = 10f;
        // Start is called before the first frame update

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(this);
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void setChunkSize(float size)
        {
            chunkSize = size;
        }

        public float getChunkSize()
        {
            return chunkSize;
        }
    }
}

