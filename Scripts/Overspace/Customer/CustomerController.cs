using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Overspace.Fluid;
using Overspace.Game;
using Overspace.GUI;
using Overspace.GUI.View;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Overspace.Customer
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class CustomerController : MonoBehaviour
    {
        public Canvas orderCanvas;
        public Image orderIconImage;
        
        public float rotationSpeed = 2.5f;
        public float orderDelay = 1f;

        public Transform holdPoint;
        
        public ParticleSystem negativeParticle;
        public ParticleSystem positiveParticle;
        
        private NavMeshAgent agent;
        private Animator animator;
        
        [HideInInspector] public bool isOrdering = false;
        [HideInInspector] public bool hasOrdered = false;
        [HideInInspector] public bool shouldDestroy = false;

        
        private bool lastReaction;
        private List<FluidData> currentOrder;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int ShouldDrink = Animator.StringToHash("ShouldDrink");
        private static readonly int ShouldPositiveGesture = Animator.StringToHash("ShouldPositiveGesture");
        private static readonly int ShouldNegativeGesture = Animator.StringToHash("ShouldNegativeGesture");

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            animator.SetBool(IsWalking, agent.velocity.magnitude > .25f);
            
            if (shouldDestroy && IsAtPosition(CustomerManager.Instance.despawnPoint.position))
            {
                Destroy(gameObject);
            }
        }

        public void MoveTo(Vector3 pos)
        {
            if (agent.destination != pos) agent.SetDestination(pos);
        }

        public void LookAt(Vector3 destination)
        {
            Vector3 lookPos = destination - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        public bool IsLookingAt(Vector3 destination)
        {
            Vector3 lookPos = destination - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            return Quaternion.Angle(rotation, transform.rotation) < 1f;
        }

        public void StartOrdering(IEnumerable<FluidData> order)
        {
            currentOrder = order.ToList();
            StartCoroutine(Order());
        }

        public IEnumerator Order()
        {
            isOrdering = true;
            orderCanvas.gameObject.SetActive(true);

            int index = 0;
            while (currentOrder.Any())
            {
                FluidData orderElement = currentOrder.First();
                Debug.Log(orderElement.fluidName);
                
                orderIconImage.sprite = orderElement.fluidIcon;
                GameManager.Instance.currentOrder.Add(orderElement);
                currentOrder.Remove(orderElement);

                OrderView orderView = GUIManager.Instance.orderView;
                orderView.SetOrderIcon(index, orderElement.fluidIcon);

                index++;
                yield return new WaitForSeconds(orderDelay);
            }

            orderCanvas.gameObject.SetActive(false);
            isOrdering = false;
            hasOrdered = true;
        }

        public void StartDrinking(bool didMatch)
        {
            lastReaction = didMatch;
            StartCoroutine(DrinkAndReact());
        }
        
        public IEnumerator DrinkAndReact()
        {
            animator.SetTrigger(ShouldDrink);

            yield return new WaitForSeconds(0.5f);

            while (animator.GetCurrentAnimatorStateInfo(0).IsName("Drinking"))
            {
                GameManager.Instance.shaker.fillPercentage -= 0.2f * Time.deltaTime;
                GameManager.Instance.shaker.fillPercentage = Mathf.Clamp01(GameManager.Instance.shaker.fillPercentage);
                yield return null;
            }
            
            Destroy(GameManager.Instance.shaker.gameObject);
            GameManager.Instance.shaker = null;

            if (lastReaction)
            {
                animator.SetTrigger(ShouldPositiveGesture);
                positiveParticle.Play();
            }
            else
            {
                animator.SetTrigger(ShouldNegativeGesture);
                negativeParticle.Play();
            }
            
            yield return new WaitForSeconds(0.5f);

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                yield return null;
            }

            if (lastReaction)
            {
                GameManager.Instance.GiveMoney(Random.Range(15, 51));
            }
            
            CustomerManager.Instance.DestroyCustomer(this);
            GameManager.Instance.ChangeState(GameManager.Instance.OrderState);

            yield return null;
        }

        public bool IsAtPosition(Vector3 pos, float minDistance = 0.1f)
        {
            return Vector3.Distance(transform.position, pos) < minDistance;
        }
    }
}