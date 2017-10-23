﻿using NormandErwan.MasterThesisExperiment.States;
using NormandErwan.MasterThesisExperiment.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace NormandErwan.MasterThesisExperiment.GUI
{
    public class DeviceGUI : MonoBehaviour
    {
        // Editor Fields

        public StateManager stateManager;
        public Text progressText;
        public Text stateTitleText;
        public Text stateInstructionsText;
        public Button okButton;

        // Methods

        protected virtual void Start()
        {
            if (stateManager.CurrentState != null)
            {
                StateManager_CurrentStateUpdated(stateManager.CurrentState);
            }
            stateManager.CurrentStateUpdated += StateManager_CurrentStateUpdated;

            okButton.onClick.AddListener(okButton_onClik);
        }

        protected virtual void OnDestroy()
        {
            stateManager.CurrentStateUpdated -= StateManager_CurrentStateUpdated;
            okButton.onClick.RemoveListener(okButton_onClik);
        }

        protected virtual void StateManager_CurrentStateUpdated(State currentState)
        {
            progressText.text = "Progression : " + (stateManager.StatesProgress * 100f / stateManager.StatesTotal).ToString("F1") + "%";

            stateTitleText.gameObject.SetActive(true);
            stateInstructionsText.gameObject.SetActive(true);
            okButton.gameObject.SetActive(true);

            stateTitleText.text = stateManager.CurrentState.title;
            stateInstructionsText.text = stateManager.CurrentState.instructions;

            stateInstructionsText.text += "\n\n";
            foreach (var independentVariable in stateManager.independentVariables)
            {
                var ivDistance = independentVariable as IVClassificationDistance;
                if (ivDistance != null)
                {
                    stateInstructionsText.text += " - " + ivDistance.title + " : " + ivDistance.CurrentCondition.title + "\n\n";
                }

                var ivTextSize = independentVariable as IVTextSize;
                if (ivTextSize != null)
                {
                    stateInstructionsText.text += " - " + ivTextSize.title + " : " + ivTextSize.CurrentCondition.title + "\n\n";
                }

                var ivTechnique = independentVariable as IVTechnique;
                if (ivTechnique != null)
                {
                    stateInstructionsText.text += " - " + ivTechnique.title + " : " + ivTechnique.CurrentCondition.title + "\n";
                    stateInstructionsText.text += "   " + ivTechnique.CurrentCondition.instructions + "\n\n";
                }
            }
        }

        protected virtual void okButton_onClik()
        {
            if (stateManager.CurrentState.id == stateManager.taskTrialState.id)
            {
                stateTitleText.gameObject.SetActive(false);
                stateInstructionsText.gameObject.SetActive(false);
                okButton.gameObject.SetActive(false);
            }
            else
            {
                stateManager.NextState();
            }
        }
    }
}