﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InDevelopment.Mechanics.ObjectDistraction
{
    public class DistractionAbility : MonoBehaviour
    {
        //TODO: apply outline to object when it is being looked at.
        private GameControls controls;

        [Header("Camera")]
        [SerializeField]
        private Camera camera;

        private bool hasObjectToThrow;
        private bool predictingThrow = false;

        [Header("Throw Settings")]
        public float PickupRange;

        public LayerMask pickupObjectLayer;
        private GameObject throwableObjectPrefab;
        private GameObject oldThrowableObjectPrefab;
        public float throwForce;

        [Header("Line Renderer Settings")]
        [SerializeField]
        private Transform handPosition;

        public Color lineRendererStartColor;
        public Color lineRendererEndColor;
        private List<Vector3> points = new List<Vector3>();

        private LineRenderer lineRenderer;

        [Header("Arc Setting")]
        [SerializeField]
        private int numberOfPoints;

        private Vector3 throwDirection;

        [SerializeField]
        private float yOffset;

        [SerializeField]
        private LayerMask groundLayerMask;

        private void Awake()
        {
            SetUpControls();
            SetUpLineRenderer();
            hasObjectToThrow = false;
            predictingThrow = false;
        }

        private void SetUpLineRenderer()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startColor = lineRendererStartColor;
            lineRenderer.endColor = lineRendererEndColor;
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
            if (predictingThrow)
            {
                PredictPath();
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

        private void RotateHand()
        {
            Vector3 mousePosition = controls.InGame.MousePosition.ReadValue<Vector2>();

            Ray ray1 = camera.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray1, out hitInfo, throwForce + 50f))
            {
                handPosition.LookAt(hitInfo.point + new Vector3(0, yOffset, 0));
            }
            else
            {
                handPosition.LookAt(ray1.GetPoint(throwForce + 50f));
            }
        }

        /// <summary>
        /// handles predicting the trajectory of the throw arc
        /// </summary>
        private void PredictPath()
        {
            RotateHand();
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
                    //the two points are used to determine the way that he point that it is in the line
                    Vector3 point1 = PointPosition(loopCount * 0.1f);
                    Vector3 point2 = PointPosition((loopCount + 1f) * 0.1f);

                    Ray ray = new Ray(point1, point2 - point1);
                    RaycastHit hit;

                    //need to change to a box cast along the raycast line

                    // if (Physics.BoxCast(transform.position, transform.lossyScale / 2, throwDirection,
                    //     out hit, Quaternion.identity))
                    // {
                    //     hitGround = true;
                    // }

                    //basically if the raycast hits anything just make that the last point, rather than the points continuing through the ground
                    if (Physics.Raycast(ray, out hit, Vector3.Distance(point1, point2), groundLayerMask))
                    {
                        hitGround = true;
                    }

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
            Vector3 currentPosition = handPosition.position + (throwDirection * throwForce * time) +
                                      0.5f * Physics.gravity * (time * time);
            return currentPosition;
        }

        private void ThrowObject()
        {
            //when you want to throw an object, it simply, moves the object
            //to where the hand position is and then just applies force to the rigidbody on the object

            GameObject throwableObject = throwableObjectPrefab;
            Rigidbody rb = throwableObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;

            throwableObject.transform.position = handPosition.position;
            throwableObject.transform.rotation = handPosition.rotation;
            throwableObject.SetActive(true);

            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

            //these two need to be in there
            throwableObjectPrefab = null;
            hasObjectToThrow = false;
        }

        private void TryToPickupObject()
        {
            //if really neede this top few lines can be moved to update to do a raycast every frame but won't be efficient
            
            //reads the mouse position and converts it into a ray position for the raycast to use
            Vector3 mousePosition = controls.InGame.MousePosition.ReadValue<Vector2>();

            Ray ray = camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            //this is just the part the "picks up" the objects in the level
            //to save on performance, the object that is raycast to if it is a throwable object then turn it off and "add" it
            //to the players hand, however instead it simply turns it off 
            if (Physics.Raycast(ray, out hit, PickupRange, pickupObjectLayer))
            {
                //when animations need to be added just have the object destroy delayed for the duration of the animations
                Debug.Log("UseObject");
                var hitObject = hit.collider.gameObject;

                if (throwableObjectPrefab == null)
                {
                    throwableObjectPrefab = hit.collider.gameObject;
                }

                oldThrowableObjectPrefab = throwableObjectPrefab;

                oldThrowableObjectPrefab.transform.position = hit.collider.transform.position;

                Rigidbody rb = oldThrowableObjectPrefab.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                oldThrowableObjectPrefab.transform.rotation = Quaternion.identity;
                oldThrowableObjectPrefab.SetActive(true);

                throwableObjectPrefab = hit.collider.gameObject;

                hitObject.SetActive(false);
                hasObjectToThrow = true;
            }
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }
    }
}