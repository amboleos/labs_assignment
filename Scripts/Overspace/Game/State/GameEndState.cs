using System.Collections.Generic;
using Overspace.Camera;
using Overspace.Customer;
using Overspace.Fluid;
using Overspace.Fluid.Container;
using UnityEngine;

namespace Overspace.Game.State
{
    public class GameEndState : GameBaseState
    {
        public override string StateName => "END";
        
        public override void Enter()
        {
            CameraManager.Instance.SetCamera(CameraManager.Instance.tableCam);
            
            FluidShakerContainer shaker = GameManager.Instance.shaker;
            bool didMatch = shaker.CompareToOrder(GameManager.Instance.currentOrder) && GameManager.Instance.correctPotion;
            
            CustomerController customer = CustomerManager.Instance.currentCustomer;
            shaker.transform.parent = customer.holdPoint;
            shaker.transform.localPosition = Vector3.zero;
            shaker.transform.localRotation = Quaternion.identity;
            customer.StartDrinking(didMatch);
        }

        public override void Exit()
        {
            GameManager.Instance.correctPotion = false;
        }

        public override void Tick()
        {
            if (CustomerManager.Instance.currentCustomer == null)
            {
                GameObject.Destroy(GameManager.Instance.shaker.gameObject);
                GameManager.Instance.shaker = null;
                GameManager.Instance.currentOrder = new List<FluidData>();
                GameManager.Instance.ChangeState(GameManager.Instance.OrderState);
            }
        }
    }
}