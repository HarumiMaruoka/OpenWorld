using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<T> : MonoBehaviour where T : Enum
{
    [Tooltip("最初のステート"), SerializeField]
    private State<T> _initilizeState = default;
    /// <summary> 現在のステートを表現する値 </summary>
    public State<T> CurrentState { get; private set; } = null;
    /// <summary> この値を変更する事で状態を遷移する。 </summary>
    public T Conditions { get; protected set; } = default;
    /// <summary> ステートを格納しておくコンテナ </summary>
    private Dictionary<Type, State<T>> _states = new Dictionary<Type, State<T>>();
    /// <summary> ステートを格納しておくコンテナ </summary>
    public IReadOnlyDictionary<Type, State<T>> States => _states;

    private void Start()
    {
        // 最初のステートを割り当てる
        var state = ChangeState(_initilizeState);
        state.ExecuteEnter();
    }

    public State<T> ChangeState(State<T> nextState)
    {
        // ディクショナリに登録されている時は登録されているオブジェクトをそのまま利用する
        if (_states.TryGetValue(nextState.GetType(), out State<T> value))
        {
            return CurrentState = value;
        }
        // 登録されていない場合
        else
        {
            // スクリプタブルオブジェクトの複製を作成する
            var clone = (State<T>)ScriptableObject.CreateInstance(nextState.GetType());
            // クローンのセットアップ処理を実行
            clone.Setup(this, nextState);
            clone.ThisAwake();
            // ディクショナリに登録する
            _states.Add(clone.GetType(), clone);
            // 現在ステートを更新する
            return CurrentState = clone;
        }
    }
    private void Update()
    {
        CurrentState.Execute();
    }
    public abstract void ClearCondition();
    public abstract void AddCondition(T condition);
    public abstract void RemoveCondition(T condition);
}
/// <summary>
/// 遷移の条件を表現する列挙体
/// </summary>
//[Flags, Serializable]
//public enum Conditions
//{
//    None = 0,
//    Everything = -1,
//    Idle = 1,
//    Move = 2,
//    Jump = 4,
//}