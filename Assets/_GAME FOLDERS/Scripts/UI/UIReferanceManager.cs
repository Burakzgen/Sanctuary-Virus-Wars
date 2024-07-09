using UnityEngine;

public class UIReferanceManager : SingleReference<UIReferanceManager>
{
    [SerializeField] GameObject[] ControlPanels;
    public GameObject m_Bar { get; set; }
    public GameObject m_UIInteractionPanel { get; set; }
    public GameObject m_ModelInteractionPanel { get; set; }

    protected override void Initialize()
    {
        base.Initialize();

        m_Bar = ControlPanels[0];
        m_UIInteractionPanel = ControlPanels[1];
        m_ModelInteractionPanel = ControlPanels[2];
    }
}
