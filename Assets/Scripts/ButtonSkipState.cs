using Harmonies.Enviroment;
using Harmonies.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ButtonSkipState : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TextMeshProUGUI _text;
    private Image _image;

    [Header("Texts")]
    [SerializeField]
    private string _skipPickEnv;
    [SerializeField]
    private string _skipPickAnimals;

    private TurnManager _turnManager;

    [Inject]
    private void Construct(TurnManager turnManager) => _turnManager = turnManager;

    private void Start()
    {
        _image = GetComponent<Image>();
        DisableButton();
        _turnManager.SubsribeOnStateMachine(OnStatusChange);
    }

    private void DisableButton()
    {
        _text.text = string.Empty;
        _image.enabled = false;
    }

    private void OnStatusChange(IState state)
    {
        _image.enabled = true;
        _button.onClick.RemoveAllListeners();
        if (state is AnimalsEnvironmentSelectState)
        {
            _text.text = _skipPickEnv;
            _button.onClick.AddListener(() => _turnManager.WasSelectedOrSkipedAnimalsEnviroment());
        } else if (state is AnimalsSelectState)
        {
            _text.text = _skipPickAnimals;
            _button.onClick.AddListener(() => _turnManager.WasAnimalsSkiped());
        } else
            DisableButton();
    }
}
