using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class StateMachine<T> : MonoBehaviour where T : Enum
{
    [Tooltip("最初のステート"), SerializeField]
    private State<T> _initilizeState = default;
    /// <summary> 現在のステートを表現する値 </summary>
    public ReactiveProperty<State<T>> CurrentState { get; private set; } = new ReactiveProperty<State<T>>(null);
    /// <summary> この値を変更する事で状態を遷移する。 </summary>
    public T Conditions { get; protected set; } = default;
    /// <summary> ステートを格納しておくコンテナ </summary>
    private Dictionary<Type, State<T>> _states = new Dictionary<Type, State<T>>();
    /// <summary> ステートを格納しておくコンテナ </summary>
    public IReadOnlyDictionary<Type, State<T>> States => _states;

    protected virtual void Start()
    {
        // 最初のステートを割り当てる
        var state = ChangeState(_initilizeState);
        state.ExecuteEnter();
    }

    public State<T> ChangeState(State<T> nextState)
    {
        //
        // Debug.Log($"ステートが変わったよ {nextState.name}");
        // ディクショナリに登録されている時は登録されているオブジェクトをそのまま利用する
        if (_states.TryGetValue(nextState.GetType(), out State<T> value))
        {
            return CurrentState.Value = value;
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
            return CurrentState.Value = clone;
        }
    }
    protected virtual void Update()
    {
        CurrentState.Value.Execute();
    }
    public abstract void ClearCondition();
    public abstract void AddCondition(T condition);
    public abstract void RemoveCondition(T condition);
}