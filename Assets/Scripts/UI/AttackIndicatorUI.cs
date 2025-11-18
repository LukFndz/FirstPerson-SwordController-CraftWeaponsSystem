using FP.Player.Combat.Attack;
using UnityEngine;
using UnityEngine.UI;

public sealed class AttackIndicatorUI : MonoBehaviour
{
    [SerializeField] private Image _left;
    [SerializeField] private Image _up;
    [SerializeField] private Image _right;

    private readonly Image[] _map = new Image[3];

    private void Awake()
    {
        _map[0] = _left;
        _map[1] = _up;
        _map[2] = _right;
    }

    public void SetDirection(AttackDirection direction)
    {
        for (int i = 0; i < 3; i++)
        {
            _map[i].color = new Color(0, 0, 0, 0.5f);
        }

        switch (direction)
        {
            case AttackDirection.Left:
                _map[0].color = new Color(0, 0, 0, 1f);
                break;

            case AttackDirection.Up:
                _map[1].color = new Color(0, 0, 0, 1f);
                break;

            case AttackDirection.Right:
                _map[2].color = new Color(0, 0, 0, 1f);
                break;
        }
    }
}
