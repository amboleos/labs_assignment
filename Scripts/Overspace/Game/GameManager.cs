using System.Collections;
using System.Collections.Generic;
using Overspace.Fluid;
using Overspace.Fluid.Container;
using Overspace.Game.State;
using Overspace.GUI;
using Overspace.Pattern.Singleton;
using UnityEngine;

namespace Overspace.Game
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public GameBaseState CurrentState;

        [HideInInspector] public List<FluidData> currentOrder = new();
        [HideInInspector] public FluidBottleContainer selectedBottle;
        [HideInInspector] public FluidPotionContainer selectedPotion;
        [HideInInspector] public FluidShakerContainer shaker;
        [HideInInspector] public bool shouldServe;
        [HideInInspector] public bool isOrderReady;
        [HideInInspector] public int playerMoney;
        [HideInInspector] public int selectedGlass;
        [HideInInspector] public bool correctPotion;
        [HideInInspector] public bool enablePotionSteps = false;
        
        public readonly GameOrderState OrderState = new GameOrderState();
        public readonly GameGlassState GlassState = new GameGlassState();
        public readonly GameBottleState BottleState = new GameBottleState();
        public readonly GamePourState PourState = new GamePourState();
        public readonly GameEndState EndState = new GameEndState();
        public readonly GamePotionState PotionState = new GamePotionState();
        public readonly GamePotionAddState PotionAddState = new GamePotionAddState();

        private void Start()
        {
            CurrentState = OrderState;
            OrderState.Enter();
        }

        private void Update()
        {
            CurrentState?.Tick();
        }
        
        public void ChangeState(GameBaseState state)
        {
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }

        public void ChangeStateAfterDelay(GameBaseState state, float delay)
        {
            StartCoroutine(ChangeStateAfterDelayCo(state, delay));
        }

        public IEnumerator ChangeStateAfterDelayCo(GameBaseState state, float delay)
        {
            yield return new WaitForSeconds(delay);
            ChangeState(state);
        }

        public void GiveMoney(int count)
        {
            playerMoney += count;
            GUIManager.Instance.tipCountText.text = $"${playerMoney}";
        }
    }
}