using System.Linq;
using Overspace.Customer;
using Overspace.Fluid;
using Overspace.GUI;
using Overspace.GUI.View;
using UnityEngine;

namespace Overspace.Game.State
{
    public class GameOrderState : GameBaseState
    {
        public override string StateName => "ORDER";
        
        public override void Enter()
        {
            FluidManager.Instance.bottles.ForEach((bottle) => bottle.Fill());
            
            GameManager.Instance.shouldServe = false;
            GameManager.Instance.isOrderReady = false;
            
            GUIManager.Instance.backButton.gameObject.SetActive(false);
            GUIManager.Instance.serveButton.gameObject.SetActive(false);
            
            GUIManager.Instance.orderView.Clear();
            GameManager.Instance.currentOrder.Clear();
            CustomerController currentCustomer = CustomerManager.Instance.GetCurrentCustomer();
            currentCustomer.MoveTo(CustomerManager.Instance.orderPoint.position);
        }

        public override void Exit()
        {
        }

        public override void Tick()
        {
            CustomerController currentCustomer = CustomerManager.Instance.GetCurrentCustomer();

            if (currentCustomer.hasOrdered)
            {
                GameManager.Instance.ChangeState(GameManager.Instance.GlassState);
                return;
            }
            
            if (currentCustomer.IsAtPosition(CustomerManager.Instance.orderPoint.position))
            {
                currentCustomer.LookAt(CustomerManager.Instance.lookPoint.position);

                if (!currentCustomer.hasOrdered && !currentCustomer.isOrdering && currentCustomer.IsLookingAt(CustomerManager.Instance.lookPoint.position))
                {
                    OrderView orderView = GUIManager.Instance.orderView;
                    orderView.Clear();
                    orderView.gameObject.SetActive(true);
                    
                    FluidData[] newOrder = new FluidData[4];
                    
                    for (int i = 0; i < 4; i++)
                    {
                        FluidData rand = FluidManager.Instance.GetRandomFluid();

                        int breaker = 0;
                        while (newOrder.Contains(rand) && breaker < 256)
                        {
                            rand = FluidManager.Instance.GetRandomFluid();
                            breaker++;
                        }
                        
                        newOrder[i] = rand;
                    }
                    
                    currentCustomer.StartOrdering(newOrder);
                }
            }
        }
    }
}