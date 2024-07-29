using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchingGame : MonoBehaviour
{
    public List<Button> conceptButtons;
    public List<Button> definitionButtons;
    public Image feedbackImage;
    public GameObject linePrefab;

    private Dictionary<string, string> correctMatches = new Dictionary<string, string>();
    private Button selectedConceptButton;
    private Button selectedDefinitionButton;
    private int correctMatchCount = 0;
    private List<GameObject> lines = new List<GameObject>();
    private GameObject currentLine;
    public RectTransform lineParent;
    public Light m_Light;

    public MissionCompletionInteraction missionInteraction;
    private void Start()
    {
        correctMatches.Add("Red Cable", "Red Socket");
        correctMatches.Add("Blue Cable", "Blue Socket");
        correctMatches.Add("Green Cable", "Green Socket");

        foreach (Button btn in conceptButtons)
        {
            btn.onClick.AddListener(() => OnConceptButtonClicked(btn));
        }
        foreach (Button btn in definitionButtons)
        {
            btn.interactable = false;
            btn.onClick.AddListener(() => OnDefinitionButtonClicked(btn));
        }

        feedbackImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentLine != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            DrawLine(currentLine.GetComponent<RectTransform>(), selectedConceptButton.transform.position, mousePosition);
        }
    }

    private void OnConceptButtonClicked(Button conceptButton)
    {
        selectedConceptButton = conceptButton;
        selectedConceptButton.image.color = Color.yellow;

        if (currentLine == null)
        {
            currentLine = Instantiate(linePrefab, lineParent);
            DrawLine(currentLine.GetComponent<RectTransform>(), selectedConceptButton.transform.position, selectedConceptButton.transform.position);

            foreach (Button btn in definitionButtons)
            {
                btn.interactable = true;
            }
        }

        CheckMatch();
    }

    private void OnDefinitionButtonClicked(Button definitionButton)
    {
        selectedDefinitionButton = definitionButton;
        CheckMatch();
    }

    private void CheckMatch()
    {
        if (selectedConceptButton != null && selectedDefinitionButton != null)
        {
            string concept = selectedConceptButton.GetComponentInChildren<TextMeshProUGUI>().text;
            string definition = selectedDefinitionButton.GetComponentInChildren<TextMeshProUGUI>().text;

            if (correctMatches.ContainsKey(concept) && correctMatches[concept] == definition)
            {
                selectedConceptButton.image.color = Color.green;
                selectedDefinitionButton.image.color = Color.green;
                correctMatchCount++;

                FinalizeLine(selectedConceptButton.transform.position, selectedDefinitionButton.transform.position, Color.green);
            }
            else
            {
                selectedConceptButton.image.color = Color.red;
                selectedDefinitionButton.image.color = Color.red;

                RemoveCurrentLine();
            }

            foreach (Button btn in definitionButtons)
            {
                btn.interactable = false;
            }
            StartCoroutine(ClearSelectionAfterDelay(0.5f));
        }
    }

    private IEnumerator ClearSelectionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (selectedConceptButton != null && selectedConceptButton.interactable)
        {
            selectedConceptButton.image.color = Color.white;
        }
        if (selectedDefinitionButton != null && selectedDefinitionButton.interactable)
        {
            selectedDefinitionButton.image.color = Color.white;
        }

        selectedConceptButton = null;
        selectedDefinitionButton = null;

        feedbackImage.gameObject.SetActive(false);

        if (correctMatchCount >= correctMatches.Count)
        {
            gameObject.SetActive(false);
            GameManager.Instance.ResumeChracterControls();
            m_Light.gameObject.SetActive(true);
            m_Light.enabled = true;
            missionInteraction.OnMissionCompleted();
            ClearAllLines();
        }
    }

    private void FinalizeLine(Vector3 start, Vector3 end, Color color)
    {
        if (currentLine != null)
        {
            DrawLine(currentLine.GetComponent<RectTransform>(), start, end);
            currentLine.GetComponent<Image>().color = color;
            lines.Add(currentLine);
            currentLine = null;
        }
    }

    private void RemoveCurrentLine()
    {
        if (currentLine != null)
        {
            Destroy(currentLine);
            currentLine = null;
        }
    }

    private void ClearAllLines()
    {
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
        lines.Clear();
    }

    private void DrawLine(RectTransform line, Vector3 start, Vector3 end)
    {
        Vector3 differenceVector = end - start;
        line.sizeDelta = new Vector2(differenceVector.magnitude, 5f);
        line.pivot = new Vector2(0, 0.5f);
        line.position = start;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        line.rotation = Quaternion.Euler(0, 0, angle);
    }
}
