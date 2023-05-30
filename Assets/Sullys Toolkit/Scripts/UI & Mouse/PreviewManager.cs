using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public class PreviewManager : MonoBehaviour
    {
        //Declarations
        [SerializeField] private List<GameObject> _previewList;


        //Monos
        private void Awake()
        {
            InitializeUtils();
        }

        private void Start()
        {
            RebuildPreviewDisplay();
        }



        //Internal Utils
        private void InitializeUtils()
        {

        }

        private void RebuildPreviewDisplay()
        {

        }



        //Getters, Setters & Commands

    }
}

