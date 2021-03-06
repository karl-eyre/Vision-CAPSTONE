﻿using System;
using System.Collections;
using System.Collections.Generic;
using InDevelopment.Mechanics.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InDevelopment.Mechanics.ObjectDistraction
{
    public class DistractionAbility : MonoBehaviour
    {
        private GameControls controls;

        [Header("Camera")]
        [SerializeField]
        private Camera camera;

        private bool hasObjectToThrow;
        private bool predictingThrow;

        public bool useThrowArc;

        [SerializeField]
        private GameObject throwableObjectPrefab;

        [SerializeField]
        private GameObject oldThrowableObjectPrefab;

        public float throwForce;
        private float defaultThrowForce;

        [Header("Line Renderer Settings")]
        [SerializeField]
        private Transform handPosition;

        private List<Vector3> points = new List<Vector3>();

        private LineRenderer lineRenderer;

        [Header("Arc Setting")]
        [SerializeField]
        private int numberOfPoints;

        private Vector3 throwDirection;

        // [SerializeField]
        private float yOffset;

        [SerializeField]
        private LayerMask groundLayerMask;

        private SelectionOutline selectionOutline;

        [SerializeField]
        private GameObject hitObject;

        private bool pickingUp;
        // public float pickupDelay = 0.5f;

        public Animator animator;
        public AnimationClip grabClip;
        public AnimationClip throwClip;
        public GameObject armMesh;


        private void Awake()
        {
            SetUpControls();
            SetUpLineRenderer();

            selectionOutline = GetComponent<SelectionOutline>();
            defaultThrowForce = throwForce;
        }

        public void Start()
        {
            armMesh.SetActive(false);
            hasObjectToThrow = false;
            predictingThrow = false;
            pickingUp = false;
        }

        private void SetUpLineRenderer()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
        }

        private void SetUpControls()
        {
            controls = new GameControls();
            controls.Enable();
            controls.InGame.PickUpObject.performed += UseObjectInput;
            controls.InGame.ThrowObject.started += PredictPathInput;
            controls.InGame.ThrowObject.canceled += ThrowObjectInput;
        }

        private void Update()
        {
            if (useThrowArc)
            {
                if (predictingThrow && hasObjectToThrow)
                {
                    CalculateThrowForce();
                    PredictPath();
                }
            }

            if (hasObjectToThrow)
            {
                // throwableObjectPrefab.GetComponent<Rigidbody>().isKinematic = true;
                hitObject.transform.position = handPosition.transform.position;
                hitObject.transform.rotation = handPosition.transform.rotation;
            }
        }

        private void ThrowObjectInput(InputAction.CallbackContext obj)
        {
            if (!hasObjectToThrow)
            {
                //maybe a ui message could appear saying you have nothing to throw?
                return;
            }

            predictingThrow = false;
            lineRenderer.positionCount = 0;
            ThrowObject();
            throwForce = defaultThrowForce;
        }

        private void CalculateThrowForce()
        {
            //have both tested to see if these are necessary
            // throwForce = defaultThrowForce / 2 + points.Count;
            throwForce = points.Count;
        }

        private void PredictPathInput(InputAction.CallbackContext obj)
        {
            if (!hasObjectToThrow)
            {
                //maybe a ui message could appear saying you have nothing to throw?
                return;
            }

            predictingThrow = true;
        }

        private void UseObjectInput(InputAction.CallbackContext obj)
        {
            TryToPickupObject();
        }

        private void RotateThrowPos()
        {
            Vector3 mousePosition = controls.InGame.MousePosition.ReadValue<Vector2>();

            Ray ray1 = camera.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo;

            //just makes the hand rotation look at the hit position of the raycast to ensure that the arc for the throw doesn't go on forever
            if (Physics.Raycast(ray1, out hitInfo, throwForce + 50f, groundLayerMask))
            {
                handPosition.LookAt(hitInfo.point);
            }
            else
            {
                handPosition.LookAt(ray1.GetPoint(throwForce + 50f));
            }
        }

        private void PredictPath()
        {
            RotateThrowPos();
            throwDirection = handPosition.transform.forward;

            points.Clear();

            bool hitGround = false;
            float loopCount = 0;

            while (!hitGround)
            {
                if (loopCount >= numberOfPoints)
                {
                    hitGround = true;
                }
                else
                {
                    //sets the current points for the loop
                    //the two points are used to determine the way that he point that it is in the line
                    Vector3 point1 = PointPosition(loopCount * 0.1f);
                    Vector3 point2 = PointPosition((loopCount + 1f) * 0.1f);


                    Ray ray = new Ray(point1, point2 - point1);
                    RaycastHit hit;

                    //just see it if it hits the ground if so then that is the end point for the line renderer
                    if (Physics.Raycast(ray, out hit, Vector3.Distance(point1, point2), groundLayerMask))
                        // if (Physics.Raycast(ray, out hit, Vector3.Distance(point1, point2)))
                    {
                        hitGround = true;
                    }

                    // Vector3 position = PointPosition(loopCount * 0.1f);
                    Vector3 position = PointPosition(loopCount * 0.1f);
                    points.Add(position);

                    if (hitGround)
                    {
                        points.Add(hit.point);
                    }
                }

                loopCount += 1f;
            }

            //sets up line renderer positions based on points list
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }

        /// <summary>
        /// when using this function times by 0.1f otherwise weird things happen
        /// this is used to predict where each point in the throw arc should be
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private Vector3 PointPosition(float time)
        {
            //used for calculating the point positions for the line renderer
            Vector3 currentPosition = handPosition.position + (throwDirection * (throwForce * time)) +
                                      Physics.gravity * (0.5f * (time * time));
            return currentPosition;
        }

        private Vector3 CalculateThrowDirection(Vector3 direction)
        {
            return direction = camera.transform.forward;
        }

        private void ThrowObject()
        {
            //when you want to throw an object, it simply, moves the object
            //to where the hand position is and then just applies force to the rigidbody on the object

            GameObject throwableObject = throwableObjectPrefab;
            Rigidbody rb = throwableObject.GetComponent<Rigidbody>();

            StartCoroutine(ThrowObj(rb, throwableObject));
        }


        private void TryToPickupObject()
        {
            //this is just the part the "picks up" the objects in the level
            //to save on performance, the object that is raycast to if it is a throwable object then turn it off and "add" it
            //to the players hand, however instead it simply turns it off 
            if (!(selectionOutline is null) && selectionOutline.pickupIsHit)
            {
                //when animations need to be added just have the object destroy delayed for the duration of the animations
                // Debug.Log("UseObject");
                //grabs the selected object from the object selection outline script
                hitObject = selectionOutline.selectedObject;

                if (throwableObjectPrefab == null)
                {
                    throwableObjectPrefab = selectionOutline.selectedObject;
                }

                oldThrowableObjectPrefab = throwableObjectPrefab;

                oldThrowableObjectPrefab.transform.position = hitObject.transform.position;

                Rigidbody rb = oldThrowableObjectPrefab.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                oldThrowableObjectPrefab.transform.rotation = Quaternion.identity;
                oldThrowableObjectPrefab.GetComponent<Collider>().enabled = true;
                oldThrowableObjectPrefab.SetActive(true);

                throwableObjectPrefab = selectionOutline.selectedObject;

                // hitObject.SetActive(false);
                // hitObject.transform.position = handPosition.position;
                if (!pickingUp)
                {
                    pickingUp = true;
                    armMesh.SetActive(true);
                    StartCoroutine(PickUpObject());
                }
            }
        }

        private void OnEnable()
        {
            if (controls != null) controls.Enable();
        }

        private void OnDestroy()
        {
            if (controls != null) controls.Disable();
        }

        IEnumerator PickUpObject()
        {
            animator.SetBool("grabbing", true);
            yield return new WaitForSeconds(grabClip.length - 0.5f);
            animator.SetBool("grabbing", false);
            hasObjectToThrow = true;
            throwableObjectPrefab.GetComponent<ThrowableObject>().thrown = false;
            throwableObjectPrefab.GetComponent<BoxCollider>().enabled = false;
            pickingUp = false;
        }

        IEnumerator ThrowObj(Rigidbody rb, GameObject throwableObject)
        {
            //throw the objects
            animator.SetBool("throwing", true);
            yield return new WaitForSeconds(throwClip.length - 0.4f);
            animator.SetBool("throwing", false);
            armMesh.SetActive(false);

            rb.velocity = Vector3.zero;
            
            throwableObject.transform.position = handPosition.position;
            throwableObject.transform.rotation = handPosition.rotation;
            throwableObject.SetActive(true);
            rb.AddForce(camera.transform.forward * throwForce, ForceMode.Impulse);

            throwableObjectPrefab.GetComponent<BoxCollider>().enabled = true;
            throwableObjectPrefab.GetComponent<ThrowableObject>().thrown = true;
            throwableObjectPrefab = null;
            hasObjectToThrow = false;
        }
    }
}