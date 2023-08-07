using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTest : MonoBehaviour
{
    public TMP_Text SpeechText;
    public List<Button> AnswerOptions;
    private StoryDataManager m_StoryDataManager;
    private StoryStep m_CurrStoryStep;
    // Start is called before the first frame update
    void Start()
    {
        HideAllChoiceButtons();
        m_StoryDataManager = GetComponent<StoryDataManager>();
        m_StoryDataManager.InitializeStoryData();
        var firstStepGuid = m_StoryDataManager.GetFirstStepGuid();
        m_CurrStoryStep = m_StoryDataManager.InstantiateStoryStep(firstStepGuid);
        PopulateDialogue();
    }

    private void PopulateDialogue()
    {
        HideAllChoiceButtons();
        for (int i = 0; i < m_CurrStoryStep.storyActions.Length; i++)
        {
            AnswerOptions[i].gameObject.SetActive(true);
            AnswerOptions[i].GetComponentInChildren<TMP_Text>().text = m_CurrStoryStep.storyActions[i].actionName;
            AnswerOptions[i].onClick.RemoveAllListeners();
            var str = m_CurrStoryStep.storyActions[i].ResultingStepNodeGuid;
            AnswerOptions[i].onClick.AddListener(()=>DoNextStep(str));
        }
        SpeechText.text = m_CurrStoryStep.storyText;
    }

    private void DoNextStep(string stepId)
    {
        
        m_CurrStoryStep = m_StoryDataManager.InstantiateStoryStep(stepId);
        if (m_CurrStoryStep != null) PopulateDialogue();
        else
        {
            HideAllChoiceButtons();
            SpeechText.text = "END";
        }
    }

    private void HideAllChoiceButtons()
    {
        foreach (var b in AnswerOptions)
        {
            b.gameObject.SetActive(false);
        }
    }
}
