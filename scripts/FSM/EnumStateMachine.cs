namespace Craiel.Essentials.FSM;

using System;
using System.Globalization;
using Contracts;
using Utils;

public class EnumStateMachine<T, TS, TE> : DefaultStateMachine<T, TS>
    where T : class
    where TS : class, IState<T>
    where TE : Enum, IConvertible
{
    private readonly TS[] states;

    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    public EnumStateMachine(T owner = default, TS initialState = default, TS globalState = default)
        : base(owner, initialState, globalState)
    {
        int maxValues = EnumDef<TE>.Count;

        this.states = new TS[maxValues];
    }

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public void SwitchState(TE value)
    {
        int id = this.GetId(value);
        TS targetState = this.states[id] ?? this.GlobalState;
        if (!this.IsInState(targetState))
        {
            this.ChangeState(targetState);
        }
    }

    public void SetState(TE value, TS state)
    {
        int id = this.GetId(value);
        this.states[id] = state;
    }

    private int GetId(TE value)
    {
        return value.ToInt32(CultureInfo.InvariantCulture);
    }
}