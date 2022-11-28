using System;
using System.Collections.Generic;
using Overspace.Pattern.Singleton;
using Overspace.Pooling;
using UnityEngine;

namespace Overspace.Customer
{
    public class CustomerManager : MonoBehaviourSingleton<CustomerManager>
    {
        public GameObject customerPrefab;

        public Transform spawnPoint;
        public Transform despawnPoint;
        public Transform orderPoint;
        public Transform lookPoint;
        
        [HideInInspector] public CustomerController currentCustomer;
        [HideInInspector] public List<CustomerController> customers = new();

        
        public CustomerController GetCurrentCustomer()
        {
            if (currentCustomer == null || currentCustomer.Equals(null)) currentCustomer = CreateCustomer();
            return currentCustomer;
        }
        
        public CustomerController CreateCustomer()
        {
            GameObject customerObject = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
            return customerObject.GetComponent<CustomerController>();
        }
        
        public void DestroyCustomer(CustomerController customer)
        {
            if (currentCustomer == customer)
            {
                currentCustomer = null;
            }
            
            customer.MoveTo(despawnPoint.position);
            customer.shouldDestroy = true;
        }
    }
}