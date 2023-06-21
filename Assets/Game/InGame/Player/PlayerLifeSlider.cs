// 日本語対応
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeSlider : MonoBehaviour
{
    [SerializeField]
    private PlayerLife _playerLife = default;
    [SerializeField]
    private Slider _slider = default;

    private void Awake()
    {
        _playerLife.CurrentLife.Subscribe(value =>
        {
            _slider.value = value / _playerLife.MaxLife;
        }).AddTo(this);

    }
}
